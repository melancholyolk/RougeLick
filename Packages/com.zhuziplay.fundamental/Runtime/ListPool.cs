using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Fundamental
{
	/// <summary>
	/// 抄Unity的。
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public static class ListPool<T>
	{
		// Object pool to avoid allocations.
		private static readonly ObjectPool<List<T>> s_ListPool = new ObjectPool<List<T>>(null, Clear, 10);
		
		static void Clear(List<T> l) { l.Clear(); }

		public static List<T> Get()
		{
			return s_ListPool.Get();
		}

		public static void Release(List<T> toRelease)
		{
			s_ListPool.Release(toRelease);
		}
	}

	public class ArrayPool<T>
	{
		private Stack<T[]> m_Stack = new Stack<T[]>();
		int m_Count;

		public int countAll { get; private set; }
		public int countActive { get { return countAll - countInactive; } }
		public int countInactive { get { return m_Stack.Count; } }
		protected int m_Mul = 1;

		public ArrayPool(int count)
		{
			m_Count = count;
		}

		public T[] Get()
		{
			T[] element;
			if (m_Stack.Count == 0)
			{
				element = new T[m_Count];
				countAll++;
				if (countAll > 3 * m_Mul)
				{
					m_Mul++;
					OptionalLog.EditorLog(typeof(T) + "[] 申请太多了，代码有问题？" + " all " + countAll + " active " + countActive);
				}
			}
			else
			{
				element = m_Stack.Pop();
			}

			return element;
		}

		public void Release(T[] element)
		{
			m_Stack.Push(element);
		}
	}

	public class ObjectPool
	{

	}

	public class ObjectPool<T> : ObjectPool where T : new()
	{
		private readonly Stack<T> m_Stack = new Stack<T>();
		private readonly UnityAction<T> m_ActionOnGet;
		private readonly UnityAction<T> m_ActionOnRelease;

		public int countAll { get; private set; }
		public int countActive { get { return countAll - countInactive; } }
		public int countInactive { get { return m_Stack.Count; } }

		protected int m_Mul = 1;
		protected int m_Interval;

		public ObjectPool()
		{
			m_ActionOnGet = m_ActionOnRelease = null;
			m_Interval = 20;
		}

		public ObjectPool(UnityAction<T> actionOnGet, UnityAction<T> actionOnRelease, int interval = 10)
		{
			m_ActionOnGet = actionOnGet;
			m_ActionOnRelease = actionOnRelease;
			m_Interval = interval;
		}

		public T Get()
		{
			T element;
			if (m_Stack.Count == 0)
			{
				element = new T();
				countAll++;
				if (countActive > m_Interval * m_Mul)
				{
					m_Mul *= 2;
					OptionalLog.EditorLog(typeof(T) + "申请太多了，代码有问题？" + " all " + countAll + " active " + countActive);
				}
			}
			else
			{
				element = m_Stack.Pop();
			}
			if (m_ActionOnGet != null)
				m_ActionOnGet(element);
			return element;
		}

		public void Release(T element)
		{
			if (m_ActionOnRelease != null)
				m_ActionOnRelease(element);
			if (m_Stack.Count > 0 && m_Stack.Contains(element))
			{
				Debug.LogError("Internal error. Trying to destroy object that is already released to pool.");
				return;
			}
			m_Stack.Push(element);
		}
	}

	public static class StaticObjectPool
	{
		//这个对象池大量使用会产生GC，不建议使用
		private static Dictionary<int, ObjectPool<object>> m_ObjectPool;

		static StaticObjectPool()
		{
			m_ObjectPool = new Dictionary<int, ObjectPool<object>>();
		}
		public static T Get<T>() where T:class,new()
		{
			int hash = typeof(T).GetHashCode();
			if (m_ObjectPool.TryGetValue(hash, out var pool) && pool.countInactive > 0)
			{
				return pool.Get() as T;
			}
			else
			{
				var newEvent = new T();
				return newEvent;
			}
		}

		public static void Release<T>(T e) where T:class
		{
			if (e == null)
			{
				return;
			}
				
			int hash = e.GetType().GetHashCode();
			if (m_ObjectPool.TryGetValue(hash, out var pool))
			{
				pool.Release(e);
			}
			else
			{
				AddNewElement(hash,e);
			}
		}

		private static void AddNewElement(int hash, object e)
		{
			var newEventStack = new ObjectPool<object>();
			newEventStack.Release(e);
			m_ObjectPool.Add(hash,newEventStack);
		}
	}
	
}