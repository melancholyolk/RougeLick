using Fundamental;
using System.Collections.Generic;

namespace RougeLike.Battle
{
	public partial class EntityBehave
	{
		// public const int ID = 0;
		public const int Transform = 0;
		// public const int Time = 2;
		public const int Input = 1;
		// public const int Motion = 4;
		public const int Physics = 2;
		public const int Weapon = 3;
		public const int Bullet = 4;
		public const int Animator = 5;
		public const int Time = 6;
		public const int Monster = 7;
		public const int Character = 8;
		public const int Skill = 9;
		// public const int Effect = 10;
		// public const int Velocity = 11;
		// public const int Product = 12;
		// public const int Nav = 13;
		// public const int Trajectory = 14;
		// public const int Aim = 15;
		// public const int Hatred = 16;

		static ObjectPool<EntityBehave> s_EntityPool;

		static ObjectPool<RougeLike.Battle.CompTransform> s_TransformPool;
		static ObjectPool<RougeLike.Battle.CompInput> s_InputPool;
		static ObjectPool<RougeLike.Battle.CompPhysic> s_PhysicsPool;
		static ObjectPool<RougeLike.Battle.CompWeapon> s_WeaponPool;
		static ObjectPool<RougeLike.Battle.CompBullet> s_BulletPool;
		static ObjectPool<RougeLike.Battle.CompAnimator> s_AnimatorPool;
		static ObjectPool<RougeLike.Battle.CompTime> s_TimePool;
		static ObjectPool<RougeLike.Battle.CompMonster> s_MonsterPool;
		static ObjectPool<RougeLike.Battle.CompCharacter> s_CharacterPool;
		static ObjectPool<RougeLike.Battle.CompSkill> s_SkillPool;

		static EntityBehave()
		{
			s_HasFuncs = new List<HasComp>();
			s_HasFuncs.Add(HasTransform);
			s_HasFuncs.Add(HasInput);
			s_HasFuncs.Add(HasPhysics);
			s_HasFuncs.Add(HasWeapon);
			s_HasFuncs.Add(HasBullet);
			s_HasFuncs.Add(HasAnimator);
			s_HasFuncs.Add(HasTime);
			s_HasFuncs.Add(HasMonster);
			s_HasFuncs.Add(HasCharacter);
			s_HasFuncs.Add(HasSkill);

			s_EntityPool = new ObjectPool<EntityBehave>();

			s_TransformPool = new ObjectPool<RougeLike.Battle.CompTransform>();
			s_InputPool = new ObjectPool<RougeLike.Battle.CompInput>();
			s_PhysicsPool = new ObjectPool<RougeLike.Battle.CompPhysic>();
			s_WeaponPool = new ObjectPool<RougeLike.Battle.CompWeapon>();
			s_BulletPool = new ObjectPool<CompBullet>();
			s_AnimatorPool = new ObjectPool<RougeLike.Battle.CompAnimator>();
			s_TimePool = new ObjectPool<RougeLike.Battle.CompTime>();
			s_MonsterPool = new ObjectPool<RougeLike.Battle.CompMonster>();
			s_CharacterPool = new ObjectPool<RougeLike.Battle.CompCharacter>();
			s_SkillPool = new ObjectPool<RougeLike.Battle.CompSkill>();
		}

		public void Release()
		{
			if (compInput != null)
			{
				compInput.Reset();
				s_InputPool.Release(compInput);
			}
			compInput = null;

			if (compPhysic != null)
			{
				compPhysic.Reset();
				s_PhysicsPool.Release(compPhysic);
			}
			compPhysic = null;

			if (compWeapon != null)
			{
				compWeapon.Reset();
				s_WeaponPool.Release(compWeapon);
			}
			compWeapon = null;

			if (compTransform != null)
			{
				compTransform.Reset();
				s_TransformPool.Release(compTransform);
			}
			compTransform = null;

			if (compAnimator != null)
			{
				compAnimator.Reset();
				s_AnimatorPool.Release(compAnimator);
			}
			compAnimator = null;

			if (compTime != null)
			{
				compTime.Reset();
				s_TimePool.Release(compTime);
			}
			compTime = null;

			if (compBullet != null)
			{
				compBullet.Reset();
				s_BulletPool.Release(compBullet);
			}
			compBullet = null;

			if (compMonster != null)
			{
				compMonster.Reset();
				s_MonsterPool.Release(compMonster);
			}
			compMonster = null;

			if (compCharacter != null)
			{
				compCharacter.Reset();
				s_CharacterPool.Release(compCharacter);
			}
			compCharacter = null;

			if (compSkill != null)
			{
				compSkill.Reset();
				s_SkillPool.Release(compSkill);
			}
			compSkill = null;
			entityType = EntityType.Null;
			s_EntityPool.Release(this);
		}

		public static bool HasInput(EntityBehave entity)
		{
			return entity.compInput != null;
		}

		public static bool HasPhysics(EntityBehave entity)
		{
			return entity.compPhysic != null;
		}

		public static bool HasWeapon(EntityBehave entity)
		{
			return entity.compWeapon != null;
		}
		public static bool HasTransform(EntityBehave entity)
		{
			return entity.compTransform != null;
		}

		public static bool HasBullet(EntityBehave entity)
		{
			return entity.compBullet != null;
		}
		public static bool HasAnimator(EntityBehave entity)
		{
			return entity.compAnimator != null;
		}

		public static bool HasTime(EntityBehave entity)
		{
			return entity.compTime != null;
		}

		public static bool HasMonster(EntityBehave entity)
		{
			return entity.compMonster != null;
		}

		public static bool HasCharacter(EntityBehave entity)
		{
			return entity.compCharacter != null;
		}

		public static bool HasSkill(EntityBehave entity)
		{
			return entity.compSkill != null;
		}

		delegate bool HasComp(EntityBehave entity);
		static List<HasComp> s_HasFuncs;
		internal bool HasComponents(int[] allOfIndices)
		{
			for (int i = 0; i < allOfIndices.Length; i++)
			{
				if (!s_HasFuncs[allOfIndices[i]](this))
				{
					return false;
				}
			}
			return true;
		}
	}
}
