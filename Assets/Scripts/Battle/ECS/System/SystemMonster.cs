
using UnityEngine;

namespace RougeLike.Battle
{
	public class SystemMonster : ECSSystem
	{
		private MonoEntity mainEntity;
		public override void FixedUpdate()
		{
			mainEntity = MonoECS.instance.mainEntity;
			var mainPos = mainEntity.entity.compTransform.position;
			foreach (var entity in MonoECS.instance.GetAllMonster())
			{
				if (entity.compMonster == null || entity.compMonster.m_dead)
					continue;
				if (Vector3.Distance(entity.compTransform.position, mainPos) > 35f)
				{
					entity.compMonster.ResetPos();
				}
				else if (Vector3.Distance(entity.compTransform.position, mainPos) < 1.2f)
				{
					mainEntity.entity.compCharacter.DoDamage(0.1f * (1 - mainEntity.entity.compCharacter.defenseBonus) * entity.compMonster.info.damage);
				}
			}
		}
	}
}