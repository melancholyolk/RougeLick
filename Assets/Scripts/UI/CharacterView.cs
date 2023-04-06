using System.Collections.Generic;
using RougeLike.Battle.Configs;
using Sirenix.OdinInspector;
using TMPro;

namespace UI
{
    public class CharacterView : SerializedMonoBehaviour
    {
        public List<Attribute> attributes;
        
        public TextMeshProUGUI text_SkillPoint;
        
        private int m_SkillPoint = 10;
        private ConfigCharacter m_PlayerConfig;
        private List<ConfigCharacter> players => MonoLoginView.instance.players;
        private bool m_HasConfirmed;
        private void Awake()
        {
            m_PlayerConfig = players[0];
        }

        private void Start()
        {
            if (m_HasConfirmed)
            {
                for (int i = 0; i < attributes.Count; i++)
                {
                    string format = string.Empty;
                    var data = GetRealData(i);
                    switch (i)
                    {
                        case 0:
                            format = $"<color=black>{data:N0}</color>";
                            break;
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                            format = $"<color=black>{data:P}</color>";
                            break;
                    }

                    attributes[i].SetData(m_PlayerConfig.attributeLevels[i], format);
                }
            }
            else
            {
                m_SkillPoint = 10;
                for (int i = 0; i < attributes.Count; i++)
                {
                    string format = string.Empty;
                    var data = GetData(m_PlayerConfig.attributeLevels[i], i);
                    switch (i)
                    {
                        case 0:
                            format = $"<color=black>{data:N0}</color>";
                            break;
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                            format = $"<color=black>{data:P}</color>";
                            break;
                    }

                    attributes[i].SetData(m_PlayerConfig.attributeLevels[i], format);
                }
            }
            text_SkillPoint.text = m_SkillPoint.ToString();
        }

        public void SetData()
        {
            m_PlayerConfig.m_HP = (int)GetData(attributes[0].level, 0);
            m_PlayerConfig.m_damageBonus = GetData(attributes[1].level, 1);
            m_PlayerConfig.m_expBonus = GetData(attributes[2].level, 2);
            m_PlayerConfig.m_criticalBonus = GetData(attributes[3].level, 3);
            m_PlayerConfig.m_criticalDamageBonus = GetData(attributes[4].level, 4);
            m_PlayerConfig.m_speedBonus = GetData(attributes[5].level, 5);
            m_PlayerConfig.m_burialBonus = GetData(attributes[6].level, 6);
            m_PlayerConfig.m_defenseBonus = GetData(attributes[7].level, 7);
            // m_PlayerConfig.m_recoverHP = GetData(attributes[8].level, 8);
            for (int i = 0; i < m_PlayerConfig.attributeLevels.Length; i++)
            {
                m_PlayerConfig.attributeLevels[i] = attributes[i].level;
            }
            m_HasConfirmed = true;
        }

        public void Add(int index)
        {
            var level = attributes[index].level;
            string format = string.Empty;
            var data = GetData(++level, index);
            switch (index)
            {
                case 0:
                    format = $"<color=green>{data:N0}</color>";
                    break;
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                    format = $"<color=green>{data:P}</color>";
                    break;
            }

            attributes[index].SetData(level, format);
            m_SkillPoint--;
            text_SkillPoint.text = m_SkillPoint.ToString();
            if (m_SkillPoint != 0) return;
            foreach (var attribute in attributes)
            {
                attribute.button_Add.interactable = false;
            }
        }

        float GetData(int level, int index)
        {
            float res = 0;
            float promote = (level - 1) * 0.02f;
            switch (index)
            {
                case 0:
                    res = m_PlayerConfig.HP * (1 +promote);
                    break;
                case 1:
                    res = m_PlayerConfig.damageBonus + promote;
                    break;
                case 2:
                    res = m_PlayerConfig.expBonus + promote;
                    break;
                case 3:
                    res = m_PlayerConfig.criticalBonus + promote;
                    break;
                case 4:
                    res = m_PlayerConfig.criticalDamageBonus + promote;
                    break;
                case 5:
                    res = m_PlayerConfig.speedBonus + promote;
                    break;
                case 6:
                    res = m_PlayerConfig.burialBonus + promote;
                    break;
                case 7:
                    res = m_PlayerConfig.defenseBonus + promote;
                    break;
            }

            return res;
        }
        float GetRealData(int index)
        {
            float res = 0;
            switch (index)
            {
                case 0:
                    res = m_PlayerConfig.m_HP;
                    break;
                case 1:
                    res = m_PlayerConfig.m_damageBonus;
                    break;
                case 2:
                    res = m_PlayerConfig.m_expBonus;
                    break;
                case 3:
                    res = m_PlayerConfig.m_criticalBonus;
                    break;
                case 4:
                    res = m_PlayerConfig.m_criticalDamageBonus;
                    break;
                case 5:
                    res = m_PlayerConfig.m_speedBonus;
                    break;
                case 6:
                    res = m_PlayerConfig.m_burialBonus;
                    break;
                case 7:
                    res = m_PlayerConfig.m_defenseBonus;
                    break;
            }

            return res;
        }
        public void ChoosePlayer(int index)
        {
            m_PlayerConfig = players[index];
            Start();
        }
    }
}