using System;
using RougeLike.Battle.Action;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
#endif

namespace RougeLike.Battle.Configs
{
	/// <summary>
	/// 武器配置
	/// </summary>
	public class ConfigWeapon : ConfigSkill
	{
		public override int GetLevelInfo() => configs.Length;
		public ConfigWeapon()
		{
			type = Type.Weapon;
			uid = oid++;
		}
		
		public class LevelWeaponConfig
		{
#if UNITY_EDITOR
			[LabelText("等级"), ReadOnly]
			public int index;
#endif
			[BoxGroup("武器设置"), LabelText("发射方式")]
			public ConfigAttackPattern attackPattern;
			[BoxGroup("武器设置"), LabelText("冷却时间")]
			public float CD;
			
			[BoxGroup("生成子弹配置"), LabelText("子弹伤害")]
			public int damage;
			[BoxGroup("生成子弹配置"), LabelText("子弹持续时间")]
			public float lifeTime;
			[BoxGroup("生成子弹配置"), LabelText("子弹是否重复释放")]
			public bool isRepeat;
			/// <summary>
			/// -1时没有限制
			/// </summary>
			[BoxGroup("生成运行时配置"), LabelText("最多击中敌人数量")]
			public int hitCount;
			// [BoxGroup("生成子弹初速度")]
			// public Vector3 initSpeed;
			// [BoxGroup("武器设置"), LabelText("增伤百分比"), Range(0, 1)]
			// public float percent;

			public string descript;
		}

		public ActionConfig[] onLaunch;
		[OnCollectionChanged("After")]
		public LevelWeaponConfig[] configs;
#if UNITY_EDITOR
		public void After(CollectionChangeInfo info, object value)
		{
			var col = value as LevelWeaponConfig[];
			for (int i = 0; i < col.Length; i++)
			{
				col[i].index = i + 1;
			}
		}
#endif
	}
}

