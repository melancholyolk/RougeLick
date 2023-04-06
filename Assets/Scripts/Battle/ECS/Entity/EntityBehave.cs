using Fundamental;
using System.Collections.Generic;

namespace RougeLike.Battle
{
	public partial class EntityBehave
	{
		private static uint s_oid;
		public uint oid;

		public CompInput compInput;
		public CompTransform compTransform;
		public CompPhysic compPhysic;
		public CompWeapon compWeapon;
		public CompAnimator compAnimator;
		public CompBullet compBullet;
		public CompTime compTime;
		public CompMonster compMonster;
		public CompCharacter compCharacter;
		public CompSkill compSkill;

		public EntityBehave()
		{
			oid = s_oid;
			s_oid++;
		}

		public static EntityBehave GetCharacterEntityBehave()
		{
			var entityBehave = s_EntityPool.Get();

			entityBehave.compInput = s_InputPool.Get();
			entityBehave.compPhysic = s_PhysicsPool.Get();
			entityBehave.compWeapon = s_WeaponPool.Get();
			entityBehave.compTransform = s_TransformPool.Get();
			entityBehave.compAnimator = s_AnimatorPool.Get();
			entityBehave.compTime = s_TimePool.Get();
			entityBehave.compCharacter = s_CharacterPool.Get();
			entityBehave.compSkill = s_SkillPool.Get();

			return entityBehave;
		}

		public static EntityBehave GetMonsterEntityBehave()
		{
			var entityBehave = s_EntityPool.Get();

			entityBehave.compTransform = s_TransformPool.Get();
			entityBehave.compPhysic = s_PhysicsPool.Get();
			entityBehave.compWeapon = s_WeaponPool.Get();
			entityBehave.compAnimator = s_AnimatorPool.Get();
			entityBehave.compTime = s_TimePool.Get();
			entityBehave.compMonster = s_MonsterPool.Get();

			return entityBehave;
		}

		public static EntityBehave GetBulletEntityBehave()
		{
			var entityBehave = s_EntityPool.Get();

			entityBehave.compTransform = s_TransformPool.Get();
			entityBehave.compPhysic = s_PhysicsPool.Get();
			entityBehave.compBullet = s_BulletPool.Get();

			return entityBehave;
		}
	}
}