using RougeLike.Battle;
using RougeLike.Battle.Configs;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace RougeLike.Battle
{
	public class CompWeapon : ECSComponent
	{
		/// <summary>
		/// 当前entity身上所有的武器
		/// </summary>
		public Dictionary<uint, ConfigWeapon> weapons = new Dictionary<uint, ConfigWeapon>();

		public class runTimeInfo
		{
			public uint uid;
			public float CD;
			public int level;
			public List<EntityBehave> currentBullet;
			public bool isForever = false;
		}
		public List<runTimeInfo> weaponRunTimeInfo = new List<runTimeInfo>();
		public List<(uint,ConfigWeapon)> toAdd = new List<(uint,ConfigWeapon)>();

		public void SetWeapon(uint i, ConfigWeapon weapon)
		{
			weapons[i] = weapon;
			var t = weaponRunTimeInfo.Find((runtime) => runtime.uid == i);
			if (t == null)
			{
				weaponRunTimeInfo.Add(new runTimeInfo() { uid = i, level = 0, currentBullet = new List<EntityBehave>()});
				Debug.Log($"添加武器成功，等级1！{weapon.name}");
			}
			else
			{
				t.isForever = false;
				for (int j = t.currentBullet.Count - 1; j >= 0; j--)
				{
					var bullet = t.currentBullet[j];
					if (bullet.compBullet == null)
					{
						t.currentBullet.Remove(bullet);
						continue;
					}
					if(!weapon.configs[t.level].isRepeat)
						bullet.compBullet.isToBeRemove = true;
				}
				t.level++;
				t.CD = 0;
				Debug.Log($"已经有该武器，等级加1！当前等级{weapon.name+ "=>" + t.level}");
			}
		}

		public ConfigWeapon GetWeapon(uint i)
		{
			if (weapons.ContainsKey(i))
				return weapons[i];
			return null;
		}

		public void Reset()
		{
			weapons.Clear();
			weaponRunTimeInfo.Clear();
		}
	}
}

