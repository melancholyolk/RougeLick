using Sirenix.OdinInspector;
#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
#endif
using System;
using static RougeLike.Battle.Configs.ConfigWeapon;

namespace RougeLike.Battle.Configs
{
	/// <summary>
	/// 属性配置
	/// </summary>
	public class ConfigAttribute : ConfigSkill
	{
		public override int GetLevelInfo() => configs.Length;

		public ConfigAttribute()
		{
			type = Type.Attribute;
			uid = oid++;
		}

		[Serializable]
		public class LevelAttributeConfig
		{
#if UNITY_EDITOR
			[LabelText("等级"), ReadOnly]
			public int index;
#endif
			public int hp = 0;
			//伤害加成
			public float damageBonus = 0f;
			//经验加成
			public float expBonus = 0f;
			//暴击加成
			public float criticalBonus = 0f;
			//暴击伤害加成
			public float criticalDamageBonus = 0f;
			//移速加成
			public float speedBonus = 0f;
			//冷却加成
			public float burialBonus = 0f;
			//防御加成
			public float defenseBonus = 0f;

			public float recoverHP = 0f;
			public string descript;
		}
		[OnCollectionChanged("After")]
		public LevelAttributeConfig[] configs;
#if UNITY_EDITOR
		public void After(CollectionChangeInfo info, object value)
		{
			var config = info.Value as LevelAttributeConfig;
			config.index = (value as LevelAttributeConfig[]).Length;
		}
#endif
	}
}

