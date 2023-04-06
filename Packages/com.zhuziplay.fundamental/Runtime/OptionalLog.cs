using System.Diagnostics;
using UnityEngine;
namespace Fundamental
{
	/// <summary>
	/// 警告和错误任何时候都应该打出来，想要Log警告和错误还是用Debug的API
	/// </summary>
	public class OptionalLog
	{
		[Conditional("UNITY_EDITOR")]
		public static void EditorLog(object message, Object context = null)
		{
			if (context == null)
				UnityEngine.Debug.Log(message);
			else
				UnityEngine.Debug.Log(message, context);
		}
		[Conditional("UNITY_EDITOR")]
		public static void EditorLog(bool condition, object message, Object context = null)
		{
			if (!condition)
				return;
			EditorLog(message, context);
		}
	}
}