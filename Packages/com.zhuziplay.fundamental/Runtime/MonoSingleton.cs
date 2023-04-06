using System;
using UnityEngine;

namespace Fundamental
{

	public class AutoSingletonAttribute : Attribute
	{
		public bool bAutoCreate;

		public AutoSingletonAttribute(bool bCreate)
		{
			this.bAutoCreate = bCreate;
		}
	}

	[AutoSingleton(true)]
	public class MonoSingleton<T> : MonoBehaviour where T : Component
	{
		private static T s_instance;

		private static bool s_destroyed;

		public static T instance
		{
			get
			{
				return MonoSingleton<T>.GetInstance();
			}
		}

		public static T GetInstance()
		{
			if (MonoSingleton<T>.s_instance == null && !MonoSingleton<T>.s_destroyed)
			{
				Type typeFromHandle = typeof(T);
				MonoSingleton<T>.s_instance = (T)((object)GameObject.FindObjectOfType(typeFromHandle));
				if (MonoSingleton<T>.s_instance == null)
				{
					object[] customAttributes = typeFromHandle.GetCustomAttributes(typeof(AutoSingletonAttribute), true);
					if (customAttributes.Length > 0 && !((AutoSingletonAttribute)customAttributes[0]).bAutoCreate)
					{
						return (T)((object)null);
					}
					GameObject gameObject = new GameObject(typeof(T).Name);
					MonoSingleton<T>.s_instance = gameObject.AddComponent<T>();
					GameObject bootObj = GameObject.Find("BootObj");
					if (bootObj != null)
					{
						gameObject.transform.SetParent(bootObj.transform);
					}
				}
			}
			return MonoSingleton<T>.s_instance;
		}

		public static void DestroyInstance()
		{
			if (MonoSingleton<T>.s_instance != null)
			{
				Destroy(MonoSingleton<T>.s_instance.gameObject);
			}
			MonoSingleton<T>.s_destroyed = true;
			MonoSingleton<T>.s_instance = (T)((object)null);
		}

		public static void ClearDestroy()
		{
			MonoSingleton<T>.DestroyInstance();
			MonoSingleton<T>.s_destroyed = false;
		}

		public static bool IsNull()
		{
			return MonoSingleton<T>.s_instance == null;
		}

		protected void Awake()
		{
			if (MonoSingleton<T>.s_instance != null && MonoSingleton<T>.s_instance.gameObject != base.gameObject)
			{
				if (Application.isPlaying)
				{
					Destroy(gameObject);
				}
				else
				{
					DestroyImmediate(gameObject);
				}
				return;
			}
			else if (MonoSingleton<T>.s_instance == null)
			{
				MonoSingleton<T>.s_instance = base.GetComponent<T>();
			}
			DontDestroyOnLoad(gameObject);
			Init();
		}

		protected void OnDestroy()
		{
			UnInit();
			if (MonoSingleton<T>.s_instance != null && MonoSingleton<T>.s_instance.gameObject == gameObject)
			{
				MonoSingleton<T>.s_instance = (T)((object)null);
			}
		}

		public static bool HasInstance()
		{
			return MonoSingleton<T>.s_instance != null;
		}

		protected virtual void Init()
		{

		}

		protected virtual void UnInit()
		{

		}
	}
}