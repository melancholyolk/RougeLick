using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Fundamental
{
	public static class OptUtils
	{
		#region Type Name Cache
		private static Dictionary<Type, string> s_TypeNameMap = new Dictionary<Type, string>();
		private static Dictionary<Type, string> s_TypeQualifiedNameMap = new Dictionary<Type, string>();

		#region Name
		public static string GetTypeName<T>()
		{
			return GetTypeName(typeof(T));
		}

		public static string GetTypeName(Type type)
		{
			string name;
			if (!s_TypeNameMap.TryGetValue(type, out name))
			{
				name = type.Name; // GC Alloc here
				s_TypeNameMap.Add(type, name);
			}
			return name;
		}

		public static string GetCachedName(this Type type)
		{
			return GetTypeName(type);
		}

		public static string GetTypeName(this object obj)
		{
			if (obj == null)
				return "";
			return GetTypeName(obj.GetType());
		}
		#endregion

		#region Qualified Name
		public static string GetTypeQualifiedName<T>()
		{
			return GetTypeQualifiedName(typeof(T));
		}

		public static string GetTypeQualifiedName(Type type)
		{
			string name;
			if (!s_TypeQualifiedNameMap.TryGetValue(type, out name))
			{
				name = type.AssemblyQualifiedName; // GC Alloc here
				s_TypeQualifiedNameMap.Add(type, name);
			}
			return name;
		}

		public static string GetCachedQualifiedName(this Type type)
		{
			return GetTypeQualifiedName(type);
		}

		public static string GetTypeQualifiedName(this object obj)
		{
			if (obj == null)
				return "";
			return GetTypeQualifiedName(obj.GetType());
		}
		#endregion

		public static string GetAssemblyName(Assembly assembly)
		{
			var fullName = assembly.FullName;
			var p = fullName.IndexOf(',');
			if (p != -1)
			{
				return fullName.Remove(p);
			}
			return fullName;
		}
		#endregion

		#region Create List/Array
		public static List<T> CreateList<T>(int capacity)
		{
			if (capacity == 0)
			{
				// 默认构造函数会使用List<T>.EmptyArray，不会创建数组
				return new List<T>();
			}
			// 指定容量的构造函数会创建数组T[capacity]，即使capacity是0也会创建T[0]
			return new List<T>(capacity);
		}

		public static T[] CreateArray<T>(int length)
		{
			if (length == 0)
			{
				return EmptyList<T>.Array;
			}
			return new T[length];
		}
		#endregion
		public static StringBuilder s_StringBuilder = new StringBuilder();
		
		public static string Concat(string a, int b)
		{
			s_StringBuilder.Append(a).Append(b);
			var ret = s_StringBuilder.ToString();
			s_StringBuilder.Clear();
			return ret;
		}
	}

	public class EmptyList<T> : List<T>
	{
		private EmptyList() { }

		public new int Capacity
		{
			get
			{
#if UNITY_EDITOR
				throw new InvalidOperationException();
#else
				return 0;
#endif
			}
			set
			{
#if UNITY_EDITOR
				throw new InvalidOperationException();
#endif
			}
		}

		public new T this[int index]
		{
			get
			{
#if UNITY_EDITOR
				throw new InvalidOperationException();
#else
				return default(T);
#endif
			}
			set
			{
#if UNITY_EDITOR
				throw new InvalidOperationException();
#endif
			}
		}

		public new void Add(T item)
		{
#if UNITY_EDITOR
			throw new InvalidOperationException();
#endif
		}

		public new void AddRange(IEnumerable<T> collection)
		{
#if UNITY_EDITOR
			throw new InvalidOperationException();
#endif
		}

		public static readonly EmptyList<T> Instance = new EmptyList<T>();
		public static readonly T[] Array = new T[0];
	}

	public class EmptySortedList<TKey, TValue> : SortedList<TKey, TValue>
	{
		private EmptySortedList() { }

		public new int Capacity
		{
			get
			{
#if UNITY_EDITOR
				throw new InvalidOperationException();
#else
				return 0;
#endif
			}
			set
			{
#if UNITY_EDITOR
				throw new InvalidOperationException();
#endif
			}
		}

		public new TValue this[TKey key]
		{
			get
			{
#if UNITY_EDITOR
				throw new InvalidOperationException();
#else
				return default(TValue);
#endif
			}
			set
			{
#if UNITY_EDITOR
				throw new InvalidOperationException();
#endif
			}
		}

		public new void Add(TKey key, TValue value)
		{
#if UNITY_EDITOR
			throw new InvalidOperationException();
#endif
		}

		public static readonly EmptySortedList<TKey, TValue> Instance = new EmptySortedList<TKey, TValue>();
	}

	public class EmptyDictionary<TKey, TValue> : Dictionary<TKey, TValue>
	{
		private EmptyDictionary() { }

		public new TValue this[TKey key]
		{
			get
			{
#if UNITY_EDITOR
				throw new InvalidOperationException();
#else
				return default(TValue);
#endif
			}
			set
			{
#if UNITY_EDITOR
				throw new InvalidOperationException();
#endif
			}
		}

		public new void Add(TKey key, TValue value)
		{
#if UNITY_EDITOR
			throw new InvalidOperationException();
#endif
		}

		public static readonly EmptyDictionary<TKey, TValue> Instance = new EmptyDictionary<TKey, TValue>();
	}
}