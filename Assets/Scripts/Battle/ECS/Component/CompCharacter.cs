
using Cysharp.Threading.Tasks;
using RougeLike.Battle.Configs;
using System.Collections.Generic;
using UnityEngine;

namespace RougeLike.Battle
{
	public class CompCharacter : ECSComponent
	{

		public float HP;

		public float recoverHP;
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

		public List<LevelInfo> levelInfos;

		private float m_HP;

		private float m_recoverHP;
		//伤害加成
		private float m_damageBonus = 0f;
		//经验加成
		private float m_expBonus = 0f;
		//暴击加成
		private float m_criticalBonus = 0f;
		//暴击伤害加成
		private float m_criticalDamageBonus = 0f;
		//移速加成
		private float m_speedBonus = 0f;
		//冷却加成
		private float m_burialBonus = 0f;
		//防御加成
		private float m_defenseBonus = 0f;

		private int m_level = 0;

		private int m_exp;

		private float m_CD;

		private bool m_dead = false;

		private float damageRate = 1;
		public int Level => m_level;
		public int Exp => m_exp;

		public int MaxExp => levelInfos[m_level].needExp;

		public float MaxHP => m_HP;

		public Dictionary<uint, ConfigAttribute> attributes = new Dictionary<uint, ConfigAttribute>();

		public class AttributeInfo
		{
			public uint uid;
			public int level;
		}
		public List<AttributeInfo> attrubuteRunTimeInfo = new List<AttributeInfo>();
		/// <summary>
		/// 初始化基础属性
		/// </summary>
		/// <param name="damage"></param>
		/// <param name="exp"></param>
		/// <param name="critical"></param>
		/// <param name="criticalDamage"></param>
		/// <param name="speed"></param>
		/// <param name="burial"></param>
		/// <param name="defense"></param>
		public void Init(float hp,float damage,float exp,float critical,float criticalDamage,float speed,float burial,float defense)
        {
			MonoECS.instance.frameAction += RecoverHP;
			m_HP = hp;
			HP = m_HP;
			m_damageBonus = damage;
			m_expBonus = exp;
			m_criticalBonus = critical;
			m_criticalDamageBonus = criticalDamage;
			m_speedBonus = speed;
			m_burialBonus = burial;
			m_defenseBonus = defense;
			Reset();
		}

		public void Reset()
		{
			damageBonus = m_damageBonus;
			expBonus = m_expBonus;
			criticalBonus = m_criticalBonus;
			criticalDamageBonus = m_criticalDamageBonus;
			speedBonus = m_speedBonus;
			burialBonus = m_burialBonus;
			defenseBonus = m_defenseBonus;
			
		}

		public void DoDamage(float Damage)
		{
			HP -= Damage;
			if(HP <= 0 && !m_dead)
            {
				m_dead = true;
				MonoECS.instance.GameEnd(false);
				MonoECS.instance.frameAction -= RecoverHP;
			}

		}

		public void AddExp(int num)
		{
			m_exp += Mathf.CeilToInt(num * (1 + expBonus));
			if (levelInfos[m_level].needExp <= m_exp)
			{
				m_exp -= levelInfos[m_level].needExp;
				m_level++;
				MonoECS.instance.OpenSkillChoose();
				// Debug.Log("levelUp:" + m_level);
			}
			//Debug.Log("剩余经验：" + m_exp);
		}

		public void SetAttribute(uint i, ConfigAttribute attribute)
		{
			attributes[i] = attribute;
			var t = attrubuteRunTimeInfo.Find((runtime) => runtime.uid == i);
			if (t == null)
			{
				attrubuteRunTimeInfo.Add(new AttributeInfo() { uid = i, level = 0 });
			}
			else
			{
				t.level++;
				Debug.Log($"已经有该技能，等级加1！当前等级{t.level + 1}");
			}
			CalculateAttribute();
		}

		private void CalculateAttribute()
		{
			Reset();
			foreach (var info in attrubuteRunTimeInfo)
			{
				var v = attributes[info.uid].configs[info.level];
				m_HP += v.hp;
				HP += v.hp;
				damageBonus += v.damageBonus;
				expBonus += v.expBonus;
				criticalBonus += v.criticalBonus;
				criticalDamageBonus += v.criticalDamageBonus;
				speedBonus += v.speedBonus;
				burialBonus += v.burialBonus;
				defenseBonus += v.defenseBonus;
			}
			damageBonus *= Mathf.Max(1,damageRate);
		}

		public ConfigAttribute GetAttribute(uint i)
		{
			if (attributes.ContainsKey(i))
				return attributes[i];
			return null;
		}

		public void RecoverHP()
        {
			m_CD += Time.deltaTime;
			if(m_CD >= 1)
            {
				m_CD = 0;
				HP = Mathf.Min(m_HP, HP + 1);
			}
        }

		public void RecoverHP(float num)
        {
			HP = (num += HP) > m_HP ? m_HP : num + HP;
        }

		public void AddForce()
        {
			damageBonus += 0.2f;
			m_damageBonus += 0.2f;
		}

	}
}

