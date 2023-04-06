using Sirenix.OdinInspector;
using UnityEngine;

namespace RougeLike.Battle.Configs
{
	public class ConfigSkill : SerializedScriptableObject
	{
		public enum Type
		{
			Weapon,
			Attribute,
		}
		[ReadOnly]
		public Type type;
		[ReadOnly]
		public uint uid;

		public string weaponName;

		public Sprite sprite;

		public static uint oid = 0;

		public virtual int GetLevelInfo() { return 0; }
	}
}
