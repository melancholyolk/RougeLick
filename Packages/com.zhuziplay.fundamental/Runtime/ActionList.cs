using System;
using System.Collections.Generic;

namespace Fundamental
{
	public class ActionList
	{
		private List<Action> m_Actions = new List<Action>();
		private int m_Count;

		public void Clear()
		{
			m_Actions.Clear();
			m_Count = 0;
		}

		public void Invoke()
		{
			for (int i = m_Actions.Count - 1; i >= 0; i--)
			{
				if (m_Actions[i] == null)
					m_Actions.RemoveAt(i);
			}
			var count = m_Actions.Count;
			for (int i = 0; i < count; i++)
			{
				if (m_Actions[i] != null)
					m_Actions[i]();
			}
		}

		public override bool Equals(object obj)
		{
			if (obj is ActionList)
				return this == (ActionList)obj;
			return false;
		}

		public override int GetHashCode()
		{
			return m_Actions.GetHashCode();
		}

		public static ActionList operator +(ActionList list, Action action)
		{
			if ((object)list == null)
			{
				list = new ActionList();
			}
			list.Add(action);
			return list;
		}

		public void Add(Action action)
		{

			if (!m_Actions.Contains(action))
			{
				m_Actions.Add(action);
				m_Count += 1;
			}
		}

		public void Remove(Action action)
		{
			for (int i = 0; i < m_Actions.Count; i++)
			{
				if (m_Actions[i] == action)
				{
					m_Actions[i] = null;
					m_Count -= 1;
					break;
				}
			}
		}

		public static ActionList operator -(ActionList list, Action action)
		{
			if ((object)list != null)
			{
				list.Remove(action);
			}
			return list;
		}

		public static bool operator ==(ActionList a, ActionList b)
		{
			if (((object)a == null || a.m_Count == 0) && ((object)b == null || b.m_Count == 0))
			{
				return true;
			}
			return ReferenceEquals(a, b);
		}

		public static bool operator !=(ActionList a, ActionList b)
		{
			if (((object)a == null || a.m_Count == 0) && ((object)b == null || b.m_Count == 0))
			{
				return false;
			}
			return !ReferenceEquals(a, b);
		}
	}

	public class ActionList<T>
	{
		private List<Action<T>> m_Actions = new List<Action<T>>();
		private int m_Count;

		public void Clear()
		{
			m_Actions.Clear();
			m_Count = 0;
		}

		public void Invoke(T arg)
		{
			for (int i = m_Actions.Count - 1; i >= 0; i--)
			{
				if (m_Actions[i] == null)
					m_Actions.RemoveAt(i);
			}
			var count = m_Actions.Count;
			for (int i = 0; i < count; i++)
			{
				if (m_Actions[i] != null)
					m_Actions[i](arg);
			}
		}

		public override bool Equals(object obj)
		{
			if (obj is ActionList<T>)
				return this == (ActionList<T>)obj;
			return false;
		}

		public override int GetHashCode()
		{
			return m_Actions.GetHashCode();
		}

		public void Add(Action<T> action)
		{
			if (!m_Actions.Contains(action))
			{
				m_Actions.Add(action);
				m_Count += 1;
			}
		}

		public void Remove(Action<T> action)
		{
			for (int i = 0; i < m_Actions.Count; i++)
			{
				if (m_Actions[i] == action)
				{
					m_Actions[i] = null;
					m_Count -= 1;
					break;
				}
			}
		}

		public static ActionList<T> operator +(ActionList<T> list, Action<T> action)
		{
			if ((object)list == null)
			{
				list = new ActionList<T>();
			}
			list.Add(action);
			return list;
		}

		public static ActionList<T> operator -(ActionList<T> list, Action<T> action)
		{
			if ((object)list != null)
			{
				list.Remove(action);
			}
			return list;
		}

		public static bool operator ==(ActionList<T> a, ActionList<T> b)
		{
			if (((object)a == null || a.m_Count == 0) && ((object)b == null || b.m_Count == 0))
			{
				return true;
			}
			return ReferenceEquals(a, b);
		}

		public static bool operator !=(ActionList<T> a, ActionList<T> b)
		{
			if (((object)a == null || a.m_Count == 0) && ((object)b == null || b.m_Count == 0))
			{
				return false;
			}
			return !ReferenceEquals(a, b);
		}
	}

	public class ActionList<T1, T2>
	{
		private List<Action<T1, T2>> m_Actions = new List<Action<T1, T2>>();
		private int m_Count;

		public void Invoke(T1 arg1, T2 arg2)
		{
			for (int i = m_Actions.Count - 1; i >= 0; i--)
			{
				if (m_Actions[i] == null)
					m_Actions.RemoveAt(i);
			}
			var count = m_Actions.Count;
			for (int i = 0; i < count; i++)
			{
				if (m_Actions[i] != null)
					m_Actions[i](arg1, arg2);
			}
		}

		public override bool Equals(object obj)
		{
			if (obj is ActionList<T1, T2>)
				return this == (ActionList<T1, T2>)obj;
			return false;
		}

		public override int GetHashCode()
		{
			return m_Actions.GetHashCode();
		}

		public void Clear()
		{
			m_Actions.Clear();
			m_Count = 0;
		}

		public static ActionList<T1, T2> operator +(ActionList<T1, T2> list, Action<T1, T2> action)
		{
			if ((object)list == null)
			{
				list = new ActionList<T1, T2>();
			}
			if (!list.m_Actions.Contains(action))
			{
				list.m_Actions.Add(action);
				list.m_Count += 1;
			}
			return list;
		}

		public static ActionList<T1, T2> operator -(ActionList<T1, T2> list, Action<T1, T2> action)
		{
			if ((object)list != null)
			{
				for (int i = 0; i < list.m_Actions.Count; i++)
				{
					if (list.m_Actions[i] == action)
					{
						list.m_Actions[i] = null;
						list.m_Count -= 1;
						break;
					}
				}
			}
			return list;
		}

		public static bool operator ==(ActionList<T1, T2> a, ActionList<T1, T2> b)
		{
			if (((object)a == null || a.m_Count == 0) && ((object)b == null || b.m_Count == 0))
			{
				return true;
			}
			return ReferenceEquals(a, b);
		}

		public static bool operator !=(ActionList<T1, T2> a, ActionList<T1, T2> b)
		{
			if (((object)a == null || a.m_Count == 0) && ((object)b == null || b.m_Count == 0))
			{
				return false;
			}
			return !ReferenceEquals(a, b);
		}
	}

	public class ActionList<T1, T2, T3>
	{
		private List<Action<T1, T2, T3>> m_Actions = new List<Action<T1, T2, T3>>();
		private int m_Count;

		public void Invoke(T1 arg1, T2 arg2, T3 arg3)
		{
			for (int i = m_Actions.Count - 1; i >= 0; i--)
			{
				if (m_Actions[i] == null)
					m_Actions.RemoveAt(i);
			}
			var count = m_Actions.Count;
			for (int i = 0; i < count; i++)
			{
				if (m_Actions[i] != null)
					m_Actions[i](arg1, arg2, arg3);
			}
		}

		public override bool Equals(object obj)
		{
			if (obj is ActionList<T1, T2, T3>)
				return this == (ActionList<T1, T2, T3>)obj;
			return false;
		}

		public override int GetHashCode()
		{
			return m_Actions.GetHashCode();
		}

		public static ActionList<T1, T2, T3> operator +(ActionList<T1, T2, T3> list, Action<T1, T2, T3> action)
		{
			if ((object)list == null)
			{
				list = new ActionList<T1, T2, T3>();
			}
			if (!list.m_Actions.Contains(action))
			{
				list.m_Actions.Add(action);
				list.m_Count += 1;
			}
			return list;
		}

		public static ActionList<T1, T2, T3> operator -(ActionList<T1, T2, T3> list, Action<T1, T2, T3> action)
		{
			if ((object)list != null)
			{
				for (int i = 0; i < list.m_Actions.Count; i++)
				{
					if (list.m_Actions[i] == action)
					{
						list.m_Actions[i] = null;
						list.m_Count -= 1;
						break;
					}
				}
			}
			return list;
		}

		public static bool operator ==(ActionList<T1, T2, T3> a, ActionList<T1, T2, T3> b)
		{
			if (((object)a == null || a.m_Count == 0) && ((object)b == null || b.m_Count == 0))
			{
				return true;
			}
			return ReferenceEquals(a, b);
		}

		public static bool operator !=(ActionList<T1, T2, T3> a, ActionList<T1, T2, T3> b)
		{
			if (((object)a == null || a.m_Count == 0) && ((object)b == null || b.m_Count == 0))
			{
				return false;
			}
			return !ReferenceEquals(a, b);
		}
	}

	public class ActionList<T1, T2, T3, T4>
	{
		private List<Action<T1, T2, T3, T4>> m_Actions = new List<Action<T1, T2, T3, T4>>();
		private int m_Count;

		public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
		{
			for (int i = m_Actions.Count - 1; i >= 0; i--)
			{
				if (m_Actions[i] == null)
					m_Actions.RemoveAt(i);
			}
			var count = m_Actions.Count;
			for (int i = 0; i < count; i++)
			{
				if (m_Actions[i] != null)
					m_Actions[i](arg1, arg2, arg3, arg4);
			}
		}

		public override bool Equals(object obj)
		{
			if (obj is ActionList<T1, T2, T3, T4>)
				return this == (ActionList<T1, T2, T3, T4>)obj;
			return false;
		}

		public override int GetHashCode()
		{
			return m_Actions.GetHashCode();
		}

		public static ActionList<T1, T2, T3, T4> operator +(ActionList<T1, T2, T3, T4> list, Action<T1, T2, T3, T4> action)
		{
			if ((object)list == null)
			{
				list = new ActionList<T1, T2, T3, T4>();
			}
			if (!list.m_Actions.Contains(action))
			{
				list.m_Actions.Add(action);
				list.m_Count += 1;
			}
			return list;
		}

		public static ActionList<T1, T2, T3, T4> operator -(ActionList<T1, T2, T3, T4> list, Action<T1, T2, T3, T4> action)
		{
			if ((object)list != null)
			{
				for (int i = 0; i < list.m_Actions.Count; i++)
				{
					if (list.m_Actions[i] == action)
					{
						list.m_Actions[i] = null;
						list.m_Count -= 1;
						break;
					}
				}
			}
			return list;
		}

		public static bool operator ==(ActionList<T1, T2, T3, T4> a, ActionList<T1, T2, T3, T4> b)
		{
			if (((object)a == null || a.m_Count == 0) && ((object)b == null || b.m_Count == 0))
			{
				return true;
			}
			return ReferenceEquals(a, b);
		}

		public static bool operator !=(ActionList<T1, T2, T3, T4> a, ActionList<T1, T2, T3, T4> b)
		{
			if (((object)a == null || a.m_Count == 0) && ((object)b == null || b.m_Count == 0))
			{
				return false;
			}
			return !ReferenceEquals(a, b);
		}
	}

	public class PriorityQueue<T>
	{
		private readonly List<T> m_Actions;
		private readonly IComparer<T> m_cmp;

		public int Count => m_Actions.Count;
		public int Capacity => m_Actions.Capacity;
		public bool IsEmpty => m_Actions.Count <= 0;

		public PriorityQueue() : this(8, Comparer<T>.Default) { }

		public PriorityQueue(int size) : this(size, Comparer<T>.Default) { }

		public PriorityQueue(int size, IComparer<T> cmp)
		{
			m_Actions = new List<T>(size);
			m_cmp = cmp;
		}
		public void Invoke(T arg)
		{
			for (int i = m_Actions.Count - 1; i >= 0; i--)
			{
				if (m_Actions[i] == null)
					m_Actions.RemoveAt(i);
			}
			var count = m_Actions.Count;
			for (int i = 0; i < count; i++)
			{
				if (m_Actions[i] != null)
					m_Actions[i]=arg;
			}
		}
		public void Enqueue(T item)
		{
			m_Actions.Add(item);
			PushHeap(m_Actions, m_cmp);
		}

		public void Dequeue()
		{
			PopHeap(m_Actions, m_cmp);
			m_Actions.RemoveAt(m_Actions.Count - 1);
		}

		public T Peek()
		{
			if (m_Actions.Count <= 0) throw new IndexOutOfRangeException();
			return m_Actions[0];
		}

		public void Clear()
		{
			m_Actions.Clear();
		}

		public void TrimExcess()
		{
			m_Actions.TrimExcess();
		}

		public static void PushHeap(IList<T> list, IComparer<T> pred)
		{
			var count = list.Count;
			if (count < 0 || count >= int.MaxValue) throw new IndexOutOfRangeException();
			if (count >= 2)
			{
				var last = count - 1;
				var val = list[last];
				PushHeapByIndex(list, last, 0, val, pred);
			}
		}

		private static void PushHeapByIndex(IList<T> list, int hole, int top, T value, IComparer<T> pred)
		{
			for (var idx = (hole - 1) >> 1;
				top < hole && pred.Compare(value, list[idx]) < 0;
				idx = (hole - 1) >> 1)
			{
				list[hole] = list[idx];
				hole = idx;
			}

			list[hole] = value;
		}

		public static void PopHeap(IList<T> list, IComparer<T> pred)
		{
			var count = list.Count;
			if (count < 0 || count >= int.MaxValue) throw new IndexOutOfRangeException();
			if (count >= 2)
			{
				var last = count - 1;
				var value = list[last];
				list[last] = list[0];
				var hole = 0;
				var bottom = last;
				var top = hole;
				var idx = hole;
				var maxSequenceNonLeaf = (bottom - 1) >> 1;
				while (idx < maxSequenceNonLeaf)
				{
					idx = 2 * idx + 2;
					if (pred.Compare(list[idx - 1], list[idx]) < 0)
					{
						--idx;
					}

					list[hole] = list[idx];
					hole = idx;
				}

				if (idx == maxSequenceNonLeaf && bottom % 2 == 0)
				{
					list[hole] = list[bottom - 1];
					hole = bottom - 1;
				}

				PushHeapByIndex(list, hole, top, value, pred);
			}
		}
	}

}