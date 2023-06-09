﻿using Sirenix.OdinInspector;
#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
#endif
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RougeLike.Battle.Configs
{
	public class ConfigCharacter : SerializedScriptableObject
	{
		[Tooltip("要加载的Wrap拖进来")]
		public GameObject prefab;

		public Sprite icon;
		//初始化数据
		public int HP;
		//伤害加成
		public float damageBonus;
		//经验加成
		public float expBonus;
		//暴击加成
		public float criticalBonus;
		//暴击伤害加成
		public float criticalDamageBonus;
		//移速加成
		public float speedBonus;
		//冷却加成
		public float burialBonus;
		//防御加成
		public float defenseBonus;
		//生命恢复
		public float recoverHP;
		
		//真实属性
		//生命值
		[HideInInspector]
		public int m_HP;
		//伤害加成
		[HideInInspector]
		public float m_damageBonus;
		//经验加成
		[HideInInspector]
		public float m_expBonus;
		//暴击加成
		[HideInInspector]
		public float m_criticalBonus;
		//暴击伤害加成
		[HideInInspector]
		public float m_criticalDamageBonus;
		//移速加成
		[HideInInspector]
		public float m_speedBonus;
		//冷却加成
		[HideInInspector]
		public float m_burialBonus;
		//防御加成
		[HideInInspector]
		public float m_defenseBonus;
		//生命恢复
		[HideInInspector] 
		public float m_recoverHP;
		// [HideInInspector] 
		public int[] attributeLevels = new int[8];
		public ConfigWeapon configWeapon1;
		public ConfigWeapon configWeapon2;
		public ConfigWeapon configWeapon3;
		public ConfigWeapon configWeapon4;
		public GameObject vcam;

		public MonsterData monsterInfo;
		public ConfigLevel level;

		public void SetConfig(ConfigCharacter other)
		{
			prefab = other.prefab;
			icon = other.icon;
			HP = other.HP;
			damageBonus = other.damageBonus;
			expBonus = other.expBonus;
			criticalBonus = other.criticalBonus;
			criticalDamageBonus = other.criticalDamageBonus;
			speedBonus = other.speedBonus;
			burialBonus = other.burialBonus;
			defenseBonus = other.defenseBonus;
			recoverHP = other.recoverHP;
			m_HP = other.m_HP;
			m_damageBonus = other.m_damageBonus;
			m_expBonus = other.m_expBonus;
			m_criticalBonus = other.m_criticalBonus;
			m_criticalDamageBonus = other.m_criticalDamageBonus;
			m_speedBonus = other.m_speedBonus;
			m_burialBonus = other.m_burialBonus;
			m_defenseBonus = other.m_defenseBonus;
			m_recoverHP = other.m_recoverHP;
			configWeapon1 = other.configWeapon1;
			configWeapon2 = other.configWeapon2;
			configWeapon3 = other.configWeapon3;
			configWeapon4 = other.configWeapon4;
		}
	}

	public class MonsterData
	{
		public int exp;
		public float damage = 1f;
		public float speed = 1f;
		public GameObject treasure;
		public bool isBoss;
	}

}