using System.Collections.Generic;

namespace Fundamental
{
	public class Trie
	{
		public Dictionary<char, Trie> childen;
		public string val;
		public Trie()
		{
			childen = new Dictionary<char, Trie>();
		}
		public void Insert(string word)
		{
			int len = word.Length;
			var cur = this.childen;
			for (int i = 0; i < len - 1; i++)
			{
				if (!cur.ContainsKey(word[i]))
				{
					cur.Add(word[i], new Trie());
				}
				cur = cur[word[i]].childen;
			}
			if (!cur.ContainsKey(word[len - 1]))
			{
				cur.Add(word[len - 1], new Trie());
			}
			cur[word[len - 1]].val = word;
		}
		public bool Search(string word)
		{
			int len = word.Length;
			var cur = this.childen;
			for (int i = 0; i < len - 1; i++)
			{
				if (!cur.ContainsKey(word[i]))
				{
					return false;
				}
				cur = cur[word[i]].childen;
			}
			if (!cur.ContainsKey(word[len - 1]))
			{
				return false;
			}
			return cur[word[len - 1]].val == word;
		}
		public bool StartsWith(string prefix)
		{
			int len = prefix.Length;
			var cur = this.childen;
			for (int i = 0; i < len - 1; i++)
			{
				if (!cur.ContainsKey(prefix[i]))
				{
					return false;
				}
				cur = cur[prefix[i]].childen;
			}
			if (!cur.ContainsKey(prefix[len - 1]))
			{
				return false;
			}
			return true;
		}
	}
	public class DisjointSet
	{
		public DisjointSet(int capacity)
		{
			Capacity = capacity;
			m_Parent = new int[capacity];
			for (int i = 0; i < capacity; i++)
				m_Parent[i] = i;
			m_Size = new int[capacity];
			for (int i = 0; i < capacity; i++)
				m_Size[i] = 1;
		}
		public int Capacity { get; private set; }
		int[] m_Parent;
		int[] m_Size;
		public int GetRoot(int index)
		{
			while (m_Parent[index] != index)
				index = m_Parent[index];
			return index;
		}
		public bool Unite(int x, int y)
		{
			x = GetRoot(x);
			y = GetRoot(y);
			if (x == y)
				return false;
			if (m_Size[x] < m_Size[y])
			{
				int tmp = x;
				x = y;
				y = tmp;
			}
			m_Parent[y] = x;
			m_Size[x] += m_Size[y];
			return true;
		}
		public bool Connected(int x, int y)
		{
			return GetRoot(x) == GetRoot(y);
		}
		public int GetSize(int index)
		{
			return m_Size[index];
		}
	}
	public class ObjectPoolStack<T>
	{
		public ObjectPoolStack(System.Func<T> onAdd, System.Action<T> onRemove = null)
		{
			this.onAdd = onAdd;
			this.onRemove = onRemove;
		}
		Stack<T> m_ObjectPool = new Stack<T>();
		public System.Func<T> onAdd;
		public System.Action<T> onRemove;
		public System.Action<T> onPop;
		public System.Action<T> onPush;
		public int Count => m_ObjectPool.Count;
		public T Pop()
		{
			if (Count == 0)
			{
				var add = onAdd.Invoke();
				onPop?.Invoke(add);
				return add;
			}
			onPop?.Invoke(m_ObjectPool.Peek());
			return m_ObjectPool.Pop();
		}
		public void Push(T t)
		{
			onPush?.Invoke(t);
			m_ObjectPool.Push(t);
		}
		public void Clear()
		{
			foreach (var o in m_ObjectPool)
			{
				onRemove(o);
			}
			m_ObjectPool.Clear();
		}
	}
	public class RelationGraph<TForwardKey, TBackwardKey>
	{
		Dictionary<TForwardKey, HashSet<TBackwardKey>> m_ForwardValues = new Dictionary<TForwardKey, HashSet<TBackwardKey>>();
		Dictionary<TBackwardKey, HashSet<TForwardKey>> m_BackwardValues = new Dictionary<TBackwardKey, HashSet<TForwardKey>>();
		public HashSet<TBackwardKey> getForwardValue(TForwardKey forwardKey) => m_ForwardValues.ContainsKey(forwardKey) ? m_ForwardValues[forwardKey] : null;
		public HashSet<TForwardKey> getBackwardValues(TBackwardKey backwardKey) => m_BackwardValues.ContainsKey(backwardKey) ? m_BackwardValues[backwardKey] : null;
		public void Add(TForwardKey forwardKey, TBackwardKey backwardKey)
		{
			if (!m_ForwardValues.ContainsKey(forwardKey))
				m_ForwardValues.Add(forwardKey, new HashSet<TBackwardKey>() { backwardKey });
			else
				m_ForwardValues[forwardKey].Add(backwardKey);
			if (!m_BackwardValues.ContainsKey(backwardKey))
				m_BackwardValues.Add(backwardKey, new HashSet<TForwardKey>() { forwardKey });
			else
				m_BackwardValues[backwardKey].Add(forwardKey);
		}
		public void RemoveForwardKey(TForwardKey forwardKey)
		{
			if (m_ForwardValues.ContainsKey(forwardKey))
			{
				var backwardKeys = m_ForwardValues[forwardKey];
				m_ForwardValues.Remove(forwardKey);
				foreach (var backwardKey in backwardKeys)
				{
					m_BackwardValues[backwardKey].Remove(forwardKey);
					if (m_BackwardValues[backwardKey].Count == 0)
						m_BackwardValues.Remove(backwardKey);
				}
			}
		}
		public void RemoveBackwardKey(TBackwardKey backwardKey)
		{
			if (m_BackwardValues.ContainsKey(backwardKey))
			{
				var forwardKeys = m_BackwardValues[backwardKey];
				m_BackwardValues.Remove(backwardKey);
				foreach (var forwardKey in forwardKeys)
				{
					m_ForwardValues[forwardKey].Remove(backwardKey);
					if (m_ForwardValues[forwardKey].Count == 0)
						m_ForwardValues.Remove(forwardKey);
				}
			}
		}
		public void Remove(TForwardKey forwardKey, TBackwardKey backwardKey)
		{
			if (!m_ForwardValues.ContainsKey(forwardKey))
				m_ForwardValues[forwardKey].Remove(backwardKey);
			if (!m_BackwardValues.ContainsKey(backwardKey))
				m_BackwardValues[backwardKey].Remove(forwardKey);
		}
	}
}

