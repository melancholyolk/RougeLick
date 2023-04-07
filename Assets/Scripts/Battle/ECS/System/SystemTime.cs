
using UnityEngine;

namespace RougeLike.Battle
{
	public class SystemTime : ECSSystem
	{
		public float timeScale = 1f;

		public float MonsterTimeScale = 1f;
		private bool m_MonsterSlow = false;
		private float m_MonsterTime = 0;
		public override void Update()
		{
			if(m_MonsterSlow)
            {
				m_MonsterTime += Time.deltaTime;
				if (m_MonsterTime > 20)
				{
					MonsterTimeScale = 1f;
					m_MonsterSlow = false;
					m_MonsterTime = 0f;
				}
			}
		}

		public void SlowMonster()
        {
			m_MonsterSlow = true;
			MonsterTimeScale = 0.5f;
		}
	}
}