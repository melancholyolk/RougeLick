using RougeLike.Battle.Configs;
using System.Collections.Generic;
using UnityEngine;

namespace RougeLike.Battle.UI
{
    public class MonoSkillPanel : MonoBehaviour
    {
        public List<GameObject> items;

        private List<ConfigSkill> configs = new List<ConfigSkill>();

        private EntityBehave entity;
        public void Init(List<ConfigSkill> cons)
        {
            entity = MonoECS.instance.mainEntity.entity;
            for(int i = 0; i < cons.Count;i++)
            configs.Add(cons[i]);
            SetInfo();
        }

        public void SetInfo()
        {
            for (int i = 0; i < configs.Count; i++)
            {
                items[i].SetActive(true);
                var configskill = configs[i];
                var name = configskill.weaponName;
                var des = "";
                var sprite = configskill.sprite;
                var type = "";
                var level = entity.compSkill.GetSkillLevel(configskill.uid);
                switch (configskill)
                {
                    case ConfigWeapon weapon:
                        des = weapon.configs[level + 1].descript;
                        type = "技能";
                        break;
                    case ConfigAttribute attribute:
                        des = attribute.configs[level + 1].descript;
                        type = "属性";
                        break;
                }
                
                items[i].GetComponent<SkillItem>().SetInfo(sprite,name,des, level+2,type);
            }
            for(int i = configs.Count; i < 3;i++)
            {
                items[i].SetActive(false);
            }
        }

        public void ConfirmSkill(int i)
        {
            entity.compSkill.AddSkill(configs[i]);
            configs.Clear();
            MonoECS.instance.battlemain.gameObject.SetActive(true);
            this.gameObject.SetActive(false);
            MonoECS.instance.PopPause();
        }
    }
}

