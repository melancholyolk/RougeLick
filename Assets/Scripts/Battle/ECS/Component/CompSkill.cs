using RougeLike.Battle.Configs;
using System.Collections.Generic;

namespace RougeLike.Battle
{
	public class CompSkill : ECSComponent
	{
		public EntityBehave entity;
		public Dictionary<uint, int> skillInfo = new Dictionary<uint, int>();
		public void Reset()
		{

		}

		public void AddSkill(ConfigSkill skill)
		{
			uint uid = skill.uid;
			if (skillInfo.ContainsKey(uid))
				skillInfo[uid]++;
			else
				skillInfo.Add(uid, 0);
			switch (skill.type)
			{
				case ConfigSkill.Type.Weapon:
					var weapon = skill as ConfigWeapon;
					entity.compWeapon.SetWeapon(uid, weapon);
					break;
				case ConfigSkill.Type.Attribute:
					var attribute = skill as ConfigAttribute;
					entity.compCharacter.SetAttribute(uid, attribute);
					break;
			}
		}

		public int GetSkillLevel(uint id)
		{
			return skillInfo.ContainsKey(id) ? skillInfo[id] : -1;
		}
	}
}