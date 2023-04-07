
using Cysharp.Threading.Tasks;
using RougeLike.Battle.Configs;
using RougeLike.Tool;
using UnityEngine;

namespace RougeLike.Battle
{
	public class CompMonster : ECSComponent
	{
		public EntityBehave entity;
		public float HP = 1;
		public MonsterData info;
		public int stage;
		public bool m_dead = false;
		public bool isBoss = false;
		private MonoECS mono;
		public void Reset()
		{
			m_dead = false;
			mono = MonoECS.instance;
		}

		public async void Hurt(float num)
		{
			if (m_dead)
				return;
			HP -= num;
			if (HP < 0)
			{
				m_dead = true;
				HP = 0;
				mono.mainEntity.entity.compCharacter.AddExp(entity.compMonster.info.exp);
				mono.AddKillNum();
				entity.compAnimator.animator.SetBool("Dead",true);
				entity.compTransform.transform.GetComponent<Collider>().isTrigger = true;
				await UniTask.Delay(3000);
				entity.compTransform.transform.GetComponent<Collider>().isTrigger = false;
				entity.compAnimator.animator.SetBool("Dead", false);

				if(info.isBoss)
                {
					var obj = GameObject.Instantiate(info.treasure);
					obj.transform.position = entity.compTransform.transform.position;
				}
				if (entity.compMonster.stage == mono.systemProcess.CurStage)
					EntityPool.Instance.ReleaseGameObject(entity.compTransform.transform.gameObject);
				else
				{
					GameObject.Destroy(entity.compTransform.transform.gameObject);
					entity.Release();
				}
				mono.EnqueueRemove(entity);
				mono.systemProcess.MonsterRelease();
			}
		}

		public void ResetPos()
		{
			var mainPos = MonoECS.instance.mainEntity.entity.compTransform.position;
			var mainRotate = MonoECS.instance.mainEntity.entity.compTransform.transform.rotation * Vector3.forward;
			var random = Quaternion.AngleAxis(Random.Range(-60, 60), Vector3.up) * mainRotate;
			entity.compTransform.transform.position = mainPos + random.normalized * 30;
		}
	}
}

