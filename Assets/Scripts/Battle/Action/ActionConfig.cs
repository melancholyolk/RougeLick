using Cysharp.Threading.Tasks;
using RougeLike.Tool;
using System;
using ACT.Battle;
using RougeLike.Battle.Configs;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace RougeLike.Battle.Action
{
	public struct Memory
	{
		public EntityBehave caster;//释放者
		public EntityBehave target;//目标
		public int damage;
		public bool isCritical;
	}
	public class ActionConfig
	{
		public virtual void Do(Memory memory)
		{
		}
	}

	public class ActionLog : ActionConfig
	{
		public string logInfo;
		public override void Do(Memory memory)
		{
			Debug.Log(logInfo);
		}
	}
	/// <summary>
	/// 重新设置物理
	/// </summary>
	public class ActionResetCompPhysics : ActionConfig
	{
		public struct PhysicsParam
		{
			public Vector3 velocity;
			public Vector3 acceleration;
		}

		public bool useLocal;
		public PhysicsParam param;
		public override void Do(Memory memory)
		{
			var physics = memory.caster.compPhysic;
			if (useLocal)
			{
				physics.Velocity = memory.caster.compTransform.transform.TransformVector(param.velocity);
				physics.Accelerate = memory.caster.compTransform.transform.TransformVector(param.acceleration);
			}
			else
			{
				physics.Velocity = param.velocity;
				physics.Accelerate = param.acceleration;
			}
		}
	}

	public class ActionBounce : ActionConfig
	{
		public bool revertX;
		public bool revertY;
		public bool revertZ;
		public override void Do(Memory memory)
		{
			var v = memory.caster.compPhysic.Velocity;
			if (revertX)
				v.x *= -1;
			if (revertY)
				v.y *= -1;
			if (revertZ)
				v.z *= -1;
			memory.caster.compPhysic.Velocity = v;
		}
	}
	public class ActionPlayEffect : ActionConfig
	{
		public GameObject effect;
		public override async void Do(Memory memory)
		{
			var go = EntityPool.Instance.GetGameObject(effect, out _);
			go.transform.position = memory.caster.compTransform.position;
			var particle = go.GetComponentsInChildren<ParticleSystem>();
			await UniTask.WaitUntil(() =>
			{
				bool isDone = true;
				foreach (var prop in particle)
				{
					if (!prop.isStopped)
						return false;
				}
				return isDone;
			});
			EntityPool.Instance.ReleaseGameObject(go);
		}
	}
	public class ActionCreateBullet : ActionConfig
	{
		public float lifeTime;
		public int damage;
		public ConfigBullet bullet;
		public override void Do(Memory memory)
		{
			var tuple = SpawnEntity.Instance.CreateBullet(bullet.effect,bullet.bulletName,bullet.useObjectPool);
			var go = tuple.Item1;
			var entity = tuple.Item2;
			go.transform.position = memory.caster.compTransform.position;
			entity.compTransform.transform = go.transform;
			entity.compBullet.lifeTime = lifeTime;
			entity.compBullet.owner = memory.caster.compBullet.owner;
			entity.compBullet.config = bullet;
			entity.compBullet.damage = damage;
			foreach (var action in entity.compBullet.config.onLaunch)
			{
				action.Do(new Memory(){caster = entity});
			}
			MonoECS.instance.EnqueueBehave(entity);
#if UNITY_EDITOR
			var box = new GameObject("DebugBullet");
			var boxComp = box.AddComponent<CapsuleCollider>();
			boxComp.direction = 2;
			boxComp.isTrigger = true;
			box.layer = 6;
			boxComp.radius = bullet.colliderConfig.radius;
			boxComp.height = bullet.colliderConfig.height;
			box.transform.SetParent(go.transform);
			box.transform.localPosition = Vector3.zero;
			box.transform.localRotation = Quaternion.identity;
			entity.compBullet.OnRelease.AddListener(() => {GameObject.Destroy(box);});
#endif
		}
	}
	public class ActionRemoveBullet : ActionConfig
	{
		public override void Do(Memory memory)
		{
			memory.caster.compBullet.isToBeRemove = true;
		}
	}
	public class ActionPlayParticle : ActionConfig
	{
		private string m_AssetAddress = "Assets/Res/Effect/CustomParticle.prefab";
		public override void Do(Memory memory)
		{
			var handle = Addressables.InstantiateAsync(m_AssetAddress);
			var position = memory.target.compTransform.position;
			handle.Completed += (l) =>
			{
				var textParticle = handle.Result.GetComponent<TextParticleSystem>();
				textParticle.SpawnDamageTextParticle(new CmdDamageNumber() { damage = memory.damage,position = position,isCriticle = memory.isCritical});
				Object.Destroy(handle.Result,5);
			};
		}
	}

	public class ActionCameraShake : ActionConfig
	{
		public override void Do(Memory memory)
		{
			MonoECS.instance.CameraShake();
		}
	}

	public class ActionAudio : ActionConfig
	{
		public enum EntityType
		{
			Monster,
			Bullet,
			Player
		}

		public EntityType type;
		public AudioClip Clip;
		public float scale;
		public override void Do(Memory memory)
		{
			switch (type)
			{
				case EntityType.Monster:
					memory.target.compTransform.transform.GetComponent<AudioSource>().PlayOneShot(Clip, scale);
					break;
				case EntityType.Bullet:
					memory.caster.compTransform.transform.GetComponent<AudioSource>().PlayOneShot(Clip, scale);
					break;
				case EntityType.Player:
					MonoECS.instance.mainCamera.GetComponent<AudioSource>().PlayOneShot(Clip, scale);
					break;
			}
		}
	}
}