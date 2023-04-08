using Sirenix.OdinInspector;
#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
#endif
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RougeLike.Battle.Configs
{
	public class ConfigProcess : SerializedScriptableObject
	{
		[OnCollectionChanged("After")]
		public List<MonsterInfo> processs = new List<MonsterInfo>();
#if UNITY_EDITOR
		public void After(CollectionChangeInfo info, object value)
		{
			var config = info.Value as MonsterInfo;
			config.stage = (value as List<MonsterInfo>).Count - 1;
		}
#endif
	}
	[Serializable]
	public class MonsterInfo
	{
		[ReadOnly]
		public int stage;
		public float hpPercent = 1;
		public int totalCount;
		[Header("生成间隔")]
		public float interval;
		[Header("每次生成数量")]
		public int count;
		public List<ConfigCharacter> monsters;
		public bool haveBoss;
		[ShowIf("haveBoss")]
		public List<ConfigCharacter> boss;
		[Header("持续时间")]
		public float time;
	}

}
