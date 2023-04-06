using UnityEngine;

namespace Fundamental
{
	public struct UseCount {

		public int count;
		public ActionList<bool> onChange;

		public void Init()
		{
			onChange = new ActionList<bool>();
		}

		public void Add()
		{
			count++;
			if (count > 0)
			{
				onChange?.Invoke(true);
			}
		}

		public void Remove()
		{
			count--;
			Debug.Assert(count >= 0);
			if (count == 0)
			{
				onChange?.Invoke(false);
			}
		}
	}
}