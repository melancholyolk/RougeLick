using RougeLike.Battle.Action;
using RougeLike.Battle.Configs;
using RougeLike.Tool;
using System.Collections.Generic;
// using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

namespace RougeLike.Battle
{
	/// <summary>
	/// 只更新子弹逻辑
	/// </summary>
	public class SystemBullet : ECSSystem
	{
		public class BulletGroup
		{
			public Transform transform;
			public float angleSpeed;
			public List<EntityBehave> children = Fundamental.ListPool<EntityBehave>.Get();
			public bool isEmpty => children.Count == 0;
		}
		public List<BulletGroup> bulletGroup = Fundamental.ListPool<BulletGroup>.Get();
		private List<EntityBehave> m_CatchedEnemy = Fundamental.ListPool<EntityBehave>.Get();
		private float timeScale => MonoECS.instance.systemTime.timeScale;
		public override void Update()
		{
			var delta = Time.deltaTime * timeScale;

			#region 有主子弹
			foreach (BulletGroup t in bulletGroup)
			{
				var rotate = Quaternion.AngleAxis(t.angleSpeed * delta, Vector3.up);
				t.transform.position = MonoECS.instance.mainEntity.transform.position;
				t.transform.rotation *= rotate;
				//更新每颗子弹的线速度
				for(int i = t.children.Count - 1;i >= 0; i--)
				{
					if (t.children[i].compTransform == null)
					{
						t.children.Remove(t.children[i]);
						continue;
					}
					if (!t.children[i].compTransform.transform.gameObject.activeSelf)
						continue;
					var w = rotate.eulerAngles;
					var r = t.children[i].compTransform.position - t.transform.position;
					t.children[i].compPhysic.Velocity = Vector3.Cross(w, r);
				}
			}
			#endregion
			
			for (int i = MonoECS.instance.m_BulletList.Count - 1; i >= 0; i--)
			{
				var bullet = MonoECS.instance.m_BulletList[i];
				if (!bullet.IsLogicAvailabel)
					continue;
				bullet.compBullet.config.DoMovement(bullet,delta);
				if (bullet.compBullet.config.useVelocityDir)
				{
					bullet.compTransform.transform.rotation = Quaternion.FromToRotation(Vector3.forward, Quaternion.Euler(bullet.compBullet.config.velocityAngle) * bullet.compPhysic.Velocity);
				}
				
				if(!bullet.compBullet.isForever)
					bullet.compBullet.timer += delta;
				
				if (bullet.compBullet.timer <= bullet.compBullet.lifeTime)
				{
					
					#region 改变子弹Transform
					//根据时间改变子弹大小
					bullet.compBullet.sizeFactor = Mathf.Lerp(bullet.compBullet.config.startSize,
						bullet.compBullet.config.endSize, bullet.compBullet.timer / bullet.compBullet.lifeTime);
					var script = bullet.compTransform.transform.GetComponent<MonoFireEffect>();
					if (script)
					{
						script.Resize(bullet.compBullet.sizeFactor * Vector3.one);
					}
					else
					{
						bullet.compTransform.transform.localScale = bullet.compBullet.sizeFactor * Vector3.one;
					}
#if UNITY_EDITOR
			bullet.compTransform.transform.GetChild(0).GetComponentInChildren<Collider>().transform.localScale = bullet.compBullet.sizeFactor * Vector3.one;
#endif
					#endregion

					#region 计算子弹碰撞
					Evaluate(bullet, delta);
					#endregion

					#region 计算伤害
					//计算伤害
					foreach (var enemy in m_CatchedEnemy)
					{
						if (enemy.compMonster == null)
							continue;
						foreach (var onHit in bullet.compBullet.config.onHitEnemy)
						{
							onHit.Do(new Memory() { caster = bullet ,target = enemy});
						}
						MonoECS.instance.DamageCalculate(bullet,enemy);
					}
					m_CatchedEnemy.Clear();
					//避免每颗子弹对一个敌人重复计算伤害,间隔一段时间清空列表
					for (int j = bullet.compBullet.memory.Count - 1; j >= 0; j--)
					{
						var memory = bullet.compBullet.memory[j];
						if (memory.timer >= 0.5f)
						{
							bullet.compBullet.memory.Remove(memory);
						}
						else
						{
							memory.timer += delta;
						}
					}
					#endregion
					continue;
				}
				bullet.compBullet.isToBeRemove = true;
			}

			#region 生命周期结束销毁子弹
			foreach (var bullet in MonoECS.instance.m_BulletList)
			{
				if (!bullet.compBullet.isToBeRemove) continue;
				//销毁之前的回调
				foreach (var t in bullet.compBullet.config.onDestroySelf)
				{
					t.Do(new Memory());
				}
				bullet.compBullet.weaponInfo.currentBullet.Remove(bullet);
				//那么就销毁这个Entity
#if UNITY_EDITOR
				bullet.compBullet.OnRelease.Invoke();
#endif
				// EntityPool.Instance.ReleaseGameObject(bullet.compTransform.transform.gameObject, true);
				MonoECS.instance.EnqueueRemove(bullet);
			}
			#endregion
		}
		/// <summary>
		/// 碰撞分两种
		/// 类似飞行物子弹的碰撞我们发射线检查下一帧路径
		/// 环绕道具判断胶囊体里面的碰撞体
		/// 检测方式在子弹里面配置
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="deltaTime"></param>
		void Evaluate(EntityBehave entity, float deltaTime)
		{
			var config = entity.compBullet.config;
			var position = entity.compTransform.transform.position;
			var forward = entity.compTransform.transform.forward;
			var halfHeight = forward * config.colliderConfig.height / 2;
			if (config.detectType == ConfigBullet.DetectType.Overlap)
			{
				//最多能碰到10个
				Collider[] hited = new Collider[10];
				int hit2 = Physics.OverlapCapsuleNonAlloc(
					halfHeight * entity.compBullet.sizeFactor + position,
					position - halfHeight * entity.compBullet.sizeFactor,
					config.colliderConfig.radius * entity.compBullet.sizeFactor,
					hited,
					1 << 0);
				for (int i = 0; i < hit2; i++)
				{
					var isEntity = hited[i].GetComponent<MonoEntity>();
					//碰到的是谁
					if (isEntity)
					{
						var target = isEntity.entity;
						//碰到玩家不做任何事情
						if (target == MonoECS.instance.mainEntity.entity)
							return;
						else
						{
							if (!entity.compBullet.memory.Exists((l) => l.enemy == target))
							{
								entity.compBullet.memory.Add(new CompBullet.HitMemory(){enemy = target,timer = 0});
								m_CatchedEnemy.Add(target);
								if (entity.compBullet.hitCounter <= 0) continue;
								if (entity.compBullet.hitCounter == 1)
									entity.compBullet.isToBeRemove = true;
								else
									entity.compBullet.hitCounter--;
							}
						}
					}
					else
					{
						//碰到环境了
						//foreach (var onHit in entity.compBullet.config.onHitEnemy)
						//{
						//	onHit.Do(new Memory() { caster = entity });
						//}
					}
				}
			}
			else
			{
				Vector3 disp = entity.compPhysic.Velocity * deltaTime;
				bool hit = Physics.CapsuleCast(
					halfHeight * entity.compBullet.sizeFactor + position,
					position - halfHeight * entity.compBullet.sizeFactor,
					config.colliderConfig.radius * entity.compBullet.sizeFactor,
					entity.compPhysic.Velocity,
					out var moveHit,
					disp.magnitude);
				if(hit)
				{
					entity.compBullet.hitInfo = moveHit;
					var isEntity = moveHit.collider.GetComponent<MonoEntity>();
					//碰到的是谁
					if (isEntity)
					{
						var target = isEntity.entity;
						//碰到玩家不做任何事情
						if (target == MonoECS.instance.mainEntity.entity)
							return;
						else
						{
							//碰到敌人造成伤害
							if (!entity.compBullet.memory.Exists((l) => l.enemy == target))
							{
								entity.compBullet.memory.Add(new CompBullet.HitMemory(){enemy = target,timer = 0});
								m_CatchedEnemy.Add(target);
								if (entity.compBullet.hitCounter <= 0) return;
								if (entity.compBullet.hitCounter == 1)
									entity.compBullet.isToBeRemove = true;
								else
									entity.compBullet.hitCounter--;
							}
						}
					}
					else
					{
						//碰到环境了
						//foreach (var onHit in entity.compBullet.config.onHitEnemy)
						//{
						//	onHit.Do(new Memory() { caster = entity });
						//}
					}
				}
			}
		}
	}
}