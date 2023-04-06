
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace RougeLike.Battle.UI
{
    public class MonoBattleMain : MonoBehaviour
    {
        public Slider expSlider;
        public Slider hpSlider;
        public TextMeshProUGUI lv;
        public TextMeshProUGUI hp;
        public EntityBehave entity;
        public bool canUse;
        public TextMeshProUGUI time;
        public TextMeshProUGUI killNum;

        private float m_time;

        private CompCharacter character;
        public void Init(EntityBehave e)
        {
            entity = e;
            MonoECS.instance.frameAction += AddTime;
            character = entity.compCharacter;
        }
        private void Update()
        {
            if (entity == null)
                return;
            expSlider.value = character.Exp * 1.0f / character.MaxExp;
            hpSlider.value = character.HP / character.MaxHP;
            lv.text =  "LV:" + character.Level;
            hp.text = $"HP:{character.HP:f2}";
            killNum.text = MonoECS.instance.killAllNum + "";
        }
        public void AddTime()
        {
            m_time += Time.deltaTime;
            var min = (int)m_time / 60;
            var second = (int)m_time % 60;
            time.text = $"{ min:00}:{second :00}";
        }
    }
}

