using UnityEngine;
using UnityEngine.Events;

namespace Fundamental
{
	public class Timer
	{
		float m_Interval, m_Duration;
		bool m_Started;
		float m_Time, m_IntervalTime;

		public UnityEvent OnInterval = new UnityEvent();
		public UnityEvent OnEnd = new UnityEvent();

		public Timer(float duration)
		{
			m_Interval = float.MaxValue;
			m_Duration = duration;
		}

		public Timer(float duration, float interval)
		{
			m_Interval = interval;
			m_Duration = duration;
		}

		public bool Started
		{
			get { return m_Started; }
		}

		public void Reset(bool start)
		{
			m_Time = m_IntervalTime = 0;
			m_Started = start;
		}

		public void Toggle(bool start)
		{
			m_Started = start;
		}

		public void Update()
		{
			if (m_Started)
			{
				float delta = Time.deltaTime;

				m_Time += delta;
				if (m_Time > m_Duration)
				{
					OnEnd.Invoke();
					m_Started = false;
				}

				m_IntervalTime += delta;
				if (m_IntervalTime > m_Interval)
				{
					OnInterval.Invoke();
					m_IntervalTime -= m_Interval;
				}
			}
		}
	}
}