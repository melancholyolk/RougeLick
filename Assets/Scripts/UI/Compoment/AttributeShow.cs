using TMPro;
using UnityEngine;
namespace RougeLike.Battle.UI
{
	public class AttributeShow : MonoBehaviour
	{
		public TextMeshProUGUI lv;

		public TextMeshProUGUI Exp;

		public TextMeshProUGUI HP;
		
		public TextMeshProUGUI recoverHP;
		//�˺��ӳ�
		public TextMeshProUGUI damageBonus;
		//����ӳ�
		public TextMeshProUGUI expBonus;
		//�����ӳ�
		public TextMeshProUGUI criticalBonus;
		//�����˺��ӳ�
		public TextMeshProUGUI criticalDamageBonus;
		//���ټӳ�
		public TextMeshProUGUI speedBonus;
		//��ȴ�ӳ�
		public TextMeshProUGUI burialBonus;
		//�����ӳ�
		public TextMeshProUGUI defenseBonus;

		public void OpenView(EntityBehave entity)
		{

			Exp.text = entity.compCharacter.Exp + "/" + entity.compCharacter.MaxExp;
			HP.text = entity.compCharacter.HP + "/" + entity.compCharacter.MaxHP;
			recoverHP.text = entity.compCharacter.recoverHP + "/S";
			var damage = entity.compCharacter.damageBonus * 100;
			var exp = entity.compCharacter.expBonus * 100;
			var critical = entity.compCharacter.criticalBonus * 100;
			var criticalDamage = entity.compCharacter.criticalDamageBonus * 100;
			var speed = entity.compCharacter.speedBonus * 100;
			var burial = entity.compCharacter.burialBonus * 100;
			var defense = entity.compCharacter.defenseBonus * 100;
			if (damage > 0)
				damageBonus.color = Color.green;
			else
				damageBonus.color = Color.white;
			if (exp > 0)
				expBonus.color = Color.green;
			else
				expBonus.color = Color.white;
			if (critical > 0)
				criticalBonus.color = Color.green;
			else
				criticalBonus.color = Color.white;
			if (criticalDamage > 0)
				criticalDamageBonus.color = Color.green;
			else
				criticalDamageBonus.color = Color.white;
			if (speed > 0)
				speedBonus.color = Color.green;
			else
				speedBonus.color = Color.white;
			if (burial > 0)
				burialBonus.color = Color.green;
			else
				burialBonus.color = Color.white;
			if (defense > 0)
				defenseBonus.color = Color.green;
			else
				defenseBonus.color = Color.white;
			lv.text = "LV:" + entity.compCharacter.Level + "";
			damageBonus.text = damage + "%";
			expBonus.text = exp + "%";
			criticalBonus.text = critical + "%";
			criticalDamageBonus.text = criticalDamage + "%";
			speedBonus.text = speed + "%";
			burialBonus.text = burial + "%";
			defenseBonus.text = defense + "%";
		}
	}
}

