using RougeLike.Battle;
using RougeLike.Battle.Action;
using UnityEngine;

public class SystemWeapon : ECSSystem
{
	public override void Update()
	{
		foreach (var entity in MonoECS.instance.GetAllEntities())
		{
			if (entity.compWeapon != null)
			{
				OverrideWeaponData(entity);
			}
		}
	}

	/// <summary>
	/// 遍历所有武器，看看是否满足发射条件
	/// </summary>
	void OverrideWeaponData(EntityBehave owner)
	{
		foreach (var newWeapon in owner.compWeapon.toAdd)
		{
			owner.compWeapon.SetWeapon(newWeapon.Item1,newWeapon.Item2);
		}
		owner.compWeapon.toAdd.Clear();
		foreach (var weapon in owner.compWeapon.weaponRunTimeInfo)
		{
			if(weapon.isForever)
				continue;
			var configData = owner.compWeapon.weapons[weapon.uid];
			if (weapon.CD <= 0)
			{
				foreach (var action in owner.compWeapon.weapons[weapon.uid].onLaunch)
				{
					action.Do(new Memory(){caster = owner});
				}
				
				//根据配置生成子弹
				weapon.CD = configData.configs[weapon.level].CD * (1- MonoECS.instance.mainEntity.entity.compCharacter.burialBonus);
				//需要生成子弹吗，还是改变timer？
				SpawnEntity.Instance.SpawnBullet(owner, configData, weapon);
			}
			else
			{
				weapon.CD -= Time.deltaTime;
			}
		}
	}
}
