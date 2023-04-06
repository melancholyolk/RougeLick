using Cysharp.Threading.Tasks;
using RougeLike.Battle;
using System.Collections.Generic;
using UnityEngine;

namespace RougeLike.Tool
{
	public class EntityPool
	{
		private static EntityPool _instance;

		public static EntityPool Instance
		{
			get
			{
				if (_instance == null)
					_instance = new EntityPool();
				return _instance;
			}
		}
		public Dictionary<string, Stack<GameObject>> pool = new Dictionary<string, Stack<GameObject>>();
		public GameObject GetGameObject(GameObject gameObject, out bool back)
		{
			GameObject obj = null;
			back = false;
			var name = gameObject.name + "(Clone)";
			if (pool.ContainsKey(name))
			{
				if (pool[name].Count > 0)
				{
					back = true;
					obj = pool[name].Pop();
				}
				if (obj == null)
				{
					back = false;
					obj = GameObject.Instantiate(gameObject);
				}

			}
			else
			{
				pool[name] = new Stack<GameObject>();
				obj = GameObject.Instantiate(gameObject);
			}
			obj.SetActive(true);
			return obj;
		}

		public async void ReleaseGameObject(GameObject gameObject,bool isParticle = false)
		{
			if (isParticle)
			{
				var particle = gameObject.GetComponentsInChildren<ParticleSystem>();
				if(particle.Length > 0)
				{
					foreach (var p in particle)
					{
						p.Stop();
					}
					await UniTask.WaitUntil(() =>
					{
						bool isDone = true;
						foreach (var prop in particle)
						{
							if (!prop.isStopped)
								return false;
						}
						return isDone;
					});
				}
			}
			gameObject.transform.SetParent(null);
			gameObject.SetActive(false);
			// gameObject.GetComponent<MonoEntity>().init = false;
			pool[gameObject.name].Push(gameObject);
		}

		public void ReleaseAll()
		{
			foreach (var kv in pool)
			{
				var v = kv.Value;
				for (int i = 0; i < v.Count; i++)
				{
					var obj = v.Pop();
					if (obj == null)
						continue;
					var mono = obj.GetComponent<MonoEntity>();
					if (mono && mono.entity.compMonster != null)
					{
						if (mono.entity.compMonster.stage != MonoECS.instance.systemProcess.CurStage)
						{
							mono.entity.Release();
							GameObject.Destroy(obj);
						}
					}
					
				}
			}
		}
	}
}

