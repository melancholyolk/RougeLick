using Cysharp.Threading.Tasks;
using RougeLike.Tool;
using System;
using ACT.Battle;
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
		public override void Do(Memory memory)
		{
			memory.caster.compPhysic.Velocity *= -1;
		}
	}
	public class ActionPlayEffect : ActionConfig
	{
		public GameObject effect;
		public override async void Do(Memory memory)
		{
			var go = EntityPool.Instance.GetGameObject(effect, out _);
			go.transform.position = memory.target.compTransform.position;
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

	public class ActionRemoveBullet : ActionConfig
	{
		public override void Do(Memory memory)
		{
			Debug.Assert(memory.caster.entityType == EntityBehave.EntityType.Bullet,"试图移除不是子弹的Entity!");
			memory.caster.compBullet.isToBeRemove = true;
		}
	}
}