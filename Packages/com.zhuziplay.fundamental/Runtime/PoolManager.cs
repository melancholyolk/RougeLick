using System.Collections.Generic;
using UnityEngine;

namespace Fundamental
{
	public class PoolManager
	{
		private static PoolManager instance;
		public static PoolManager GetInstance
		{
			get
			{
				if (instance == null)
					instance = new PoolManager();
				return instance;
			}
		}

		public Dictionary<string, List<GameObject>> pool;

		private PoolManager()
		{
			pool = new Dictionary<string, List<GameObject>>();
		}

		public void PushObj(string name, GameObject obj)
		{
			if (pool.TryGetValue(name, out var list))
			{
				list.Add(obj);
			}
			else
			{
				list = new List<GameObject>();
				list.Add(obj);
				pool.Add(name, list);
			}
		}

		public GameObject GetObj(string name)
		{
			if (pool.TryGetValue(name, out var list) && list.Count > 0)
			{
				GameObject obj;
				obj = list[0];
				list.RemoveAt(0);
				return obj;
			}
			else
			{
				return null;
			}
		}

		public void Clear()
		{
			foreach (var list in pool)
			{
				foreach (var go in list.Value)
				{
					GameObject.Destroy(go);
				}
				list.Value.Clear();
			}
			pool.Clear();
		}
	}

}
