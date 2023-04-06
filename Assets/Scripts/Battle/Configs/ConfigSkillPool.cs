using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace RougeLike.Battle.Configs
{
	public class ConfigSkillPool : SerializedScriptableObject
	{
		public List<ConfigSkill> configSkills = new List<ConfigSkill>();
	}
}
