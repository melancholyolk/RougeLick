using System;
using System.Drawing.Text;
using UnityEngine;

namespace RougeLike.Battle
{
	public class CompTime : ECSComponent
	{
		public float fixedTime
		{
			get
			{
				return Time.fixedTime * m_timeScale;
			}
		}

		public float m_timeScale = 1f;

		public float Timescale
		{
			get { return m_timeScale; }
			set { m_timeScale = value; }
		}

		public void Reset()
		{

		}
	}
}

