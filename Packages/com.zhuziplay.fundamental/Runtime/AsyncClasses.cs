using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fundamental
{
	public class Instructions : CustomYieldInstruction
	{
		public bool status;
		public event Action onUpdate;
		public float percent { get; protected set; }
		public override bool keepWaiting => !status;

		List<LoadInstruction> m_List = new List<LoadInstruction>();
		List<float> m_Weight = new List<float>();
		public void AddInstruction(LoadInstruction load, float weight)
		{
			m_List.Add(load);
			m_Weight.Add(weight);
			load.onUpdate += OnOneUpdate;
			OnOneUpdate();
		}

		public void Clear()
		{
			for (int i = 0; i < m_List.Count; i++)
			{
				m_List[i].onUpdate -= OnOneUpdate;
			}

			m_List.Clear();
			m_Weight.Clear();

			OnOneUpdate();
		}

		private void OnOneUpdate()
		{
			percent = 0;
			for (int i = 0; i < m_List.Count; i++)
			{
				percent += m_List[i].percent * m_Weight[i];
			}
			onUpdate?.Invoke();
		}

		//先废置了，没啥用现在
		//public float GetInstructionsWeight() {
		//	float weightTemp = 0.0f;
		//	if (m_List.Count == 0)
		//	{
		//		weightTemp = 1.0f;
		//		return weightTemp;
		//	}
		//	else {
		//		return 1.0f / (float)m_List.Count;
		//	}
		//}
	}
	public class SwitchInstruction : CustomYieldInstruction
	{
		public bool status; //为false 是需要等待 为true 是不需要等待
		public SwitchInstruction()
		{
			status = false;
		}
		public override bool keepWaiting //为false的时候 不需要等待
		{
			get
			{
				return !status;
			}
		}
		public override void Reset()
		{
			status = false;
		}
	}
	public class LoadInstruction : CustomYieldInstruction
	{
		public bool status;
		public event Action onUpdate;
		public override bool keepWaiting
		{
			get
			{
				return !status;
			}
		}

		public float percent
		{
			get
			{
				if (m_Total == 0)
				{
					return 1;
				}
				return ((float)m_Current) / m_Total;
			}
		}

		int m_Total;
		int m_Current;
		
		public LoadInstruction()
		{
			status = true;
			m_Total = 1;
		}

		private void Release()
		{
			status = true;
			m_Total = 1;
			onUpdate = null;
		}

		public void AddCount()
		{
			Debug.Assert(m_Current < m_Total);
			m_Current++;

			if (m_Current >= m_Total)
			{
				status = true;
			}
			onUpdate?.Invoke();
		}

		public void Start(int totalMissionCount)
		{
			m_Current = 0;
			m_Total = totalMissionCount;
			if (totalMissionCount > 0)
			{
				status = false;
			}
			else
			{
				status = true;
			}
			onUpdate?.Invoke();
		}

		public static void Release(LoadInstruction l)
		{
			l.Release();
		}

		
	}
}