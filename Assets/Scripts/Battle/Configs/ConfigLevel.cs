#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
#endif
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

namespace RougeLike.Battle.Configs
{
	public class ConfigLevel : SerializedScriptableObject
	{
#if UNITY_EDITOR
		[OnCollectionChanged("After")]
#endif
		public List<LevelInfo> levelInfos = new List<LevelInfo>();
#if UNITY_EDITOR
		public void After(CollectionChangeInfo info, object value)
		{
			var config = info.Value as LevelInfo;
			config.level = (value as List<LevelInfo>).Count;
			config.needExp = (value as List<LevelInfo>).Count * 50 +100;
		}
#endif
	}

	[Serializable]
	public class LevelInfo
	{
		[ReadOnly]
		public int level;
		public int needExp;
	}

}
