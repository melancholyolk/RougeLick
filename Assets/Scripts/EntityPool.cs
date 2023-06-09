﻿using Cysharp.Threading.Tasks;
using RougeLike.Battle;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

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
		public async UniTask<GameObject> AsyncGetGameObject(string referenceName)
		{
			if(pool.TryGetValue(referenceName,out var stack))
			{
				if(stack.Count > 0)
				{
					var obj = stack.Pop();
					obj.SetActive(true);
					return obj;
				}
			}
			else
			{
				pool[referenceName] = new Stack<GameObject>();
				var handle = Addressables.InstantiateAsync(referenceName);
				await handle;
				return handle.Result;
			}

			return default;
		}

		public GameObject GetBullet(GameObject gameObject,string name)
		{
			if(pool.TryGetValue(name,out var stack))
			{
				if(stack.Count > 0)
				{
					var obj = stack.Pop();
					obj.SetActive(true);
					return obj;
				}
				return Object.Instantiate(gameObject);
			}
			else
			{
				pool[gameObject.name] = new Stack<GameObject>();
				return Object.Instantiate(gameObject);
			}

			return default;
		}

		public void ReleaseBullet(GameObject gameObject,string name)
		{
			Debug.Assert(gameObject != null,"把空对象放回池子？");
			if(pool.TryGetValue(name,out var stack))
			{
				stack.Push(gameObject);
			}
			else
			{
				pool[name] = new Stack<GameObject>();
				pool[name].Push(gameObject);
			}
			gameObject.SetActive(false);
		}
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
							Debug.Log(mono.entity.compBullet);
							mono.entity.Release();
							GameObject.Destroy(obj);
						}
					}
					
				}
			}
		}
	}
}

