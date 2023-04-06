
using RougeLike.Battle.Configs;
using RougeLike.Tool;
using System.Collections.Generic;
using UnityEngine;

namespace RougeLike.Battle
{
	public class SystemProcess : ECSSystem
	{
		public List<MonsterInfo> processs;
		public float CurTime => m_curTime;
		public float CurStageTime => m_curStageTime;
		public int CurCount => m_curCount;
		public int CurStage => m_stage;

		private float m_curTime;
		private float m_curStageTime;
		private int m_curCount;
		private int m_stage;
		private float m_stageTime;
		private int m_maxCount;
		private int m_monsterTypeCount;
		private float m_intervalTime;
		private float m_curIntervalTime;

		public void Init(List<MonsterInfo> pro)
		{
			processs = pro;
			m_stage = 0;
			m_curTime = 0f;
			m_curCount = 0;
			m_curStageTime = 0f;
			m_curIntervalTime = 0f;
			m_intervalTime = processs[m_stage].interval;
			m_stageTime = processs[m_stage].time;
			m_maxCount = processs[m_stage].totalCount;
			m_monsterTypeCount = processs[m_stage].monsters.Count;
		}

		public override void Update()
		{
			var deltaTime = Time.deltaTime * MonoECS.instance.TimeScale;
			m_curStageTime += deltaTime;
			m_curTime += deltaTime;
			m_curIntervalTime += deltaTime;
			if (m_curStageTime >= m_stageTime)
			{
				GetNextStage();
			}
			else if (m_curIntervalTime > m_intervalTime)
			{
				SpawnMonster();
				m_curIntervalTime = 0f;
			}
		}

		private void GetNextStage()
		{
			m_curStageTime = 0;
			if (m_stage >= processs.Count - 1)
			{
				m_stageTime = 99999;
				return;
			}
			m_stage++;
			m_maxCount = processs[m_stage].totalCount;
			m_stageTime = processs[m_stage].time;
			m_monsterTypeCount = processs[m_stage].monsters.Count;
			EntityPool.Instance.ReleaseAll();
		}

		private void SpawnMonster()
		{
			if (m_maxCount > m_curCount)
			{
				var config = processs[m_stage].monsters;
				var surplus = m_maxCount - m_curCount;
				var count = processs[m_stage].count;
				var spawnCount = surplus > count ? count : surplus;
				for (int i = 0; i < spawnCount; i++)
				{
					var type = Random.Range(0, m_monsterTypeCount);
					SpawnEntity.Instance.SpawnMonsterEntity(config[type], m_stage);
				}
				m_curCount += spawnCount;
			}
		}

		public void MonsterRelease()
		{
			m_curCount--;
		}
	}
}