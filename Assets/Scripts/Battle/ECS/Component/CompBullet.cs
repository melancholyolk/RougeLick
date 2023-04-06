using System.Collections.Generic;
using RougeLike.Battle.Configs;
using UnityEngine;
using UnityEngine.Events;

namespace RougeLike.Battle
{
	public class CompBullet : ECSComponent
	{
		public EntityBehave owner;
		public ConfigBullet config;
		public SystemBullet.BulletGroup bulletGroup;
		public RaycastHit hitInfo;
		public CompWeapon.runTimeInfo weaponInfo;
		public float timer;
		public float lifeTime;
		public bool isForever;
		public float sizeFactor = 1;
		public int hitCounter = 0;
		public bool isToBeRemove;
		public List<HitMemory> memory = Fundamental.ListPool<HitMemory>.Get();
		public List<EntityBehave> cachedEnemy = Fundamental.ListPool<EntityBehave>.Get();
		public UnityEvent OnRelease = new UnityEvent();
		public class HitMemory
		{
			public EntityBehave enemy;
			public float timer;
		}
		public void Reset()
		{
			config = null;
			timer = 0;
			bulletGroup= null;
			weaponInfo = null;
			lifeTime = 0;
			isForever = false;
			sizeFactor = 1;
			hitCounter = 0;
			isToBeRemove = false;
			memory.Clear();
			cachedEnemy.Clear();
			OnRelease.RemoveAllListeners();
		}
	}
}