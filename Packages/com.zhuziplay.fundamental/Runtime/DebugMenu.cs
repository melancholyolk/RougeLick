using UnityEngine;

namespace Fundamental
{
	public static partial class DebugMenu
	{
		public static bool DamageEnabled;
		public static bool BehaviorTreeEnabled;
		public static bool AbilityEnabled;
		public static bool NetCodeEnabled;
		public static bool BluePrintEnabled;
		public static bool MotionEnabled;
		public static bool Motion_WaitLeftEnabled;
		public static bool WorldEnabled;
		public static bool World_WorldAssetEnabled;
		public static bool ObjectPoolEnabled;
		public static bool StateObjectEnabled;
		public static bool StateObject_MonsterPointEnabled;
		public static bool EventTrackEnabled;
		public static bool EventTrackDataEnabled;
		public static string DebugColor(Color color, string msg)
		{
			return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{msg}</color>";
		}
	}
}

//#if UNITY_EDITOR

//namespace ACT
//{
//	public static partial class DebugMenu
//	{
//		const string DamageLog = "DamageLog";
//		static DebugMenu()
//		{
//			DamageEnabled = EditorPrefs.GetBool(DamageLog, false);
//		}

//		[MenuItem("ACT/Log/Damage")]
//		private static void ToggleDamage()
//		{
//			DamageEnabled = !DamageEnabled;
//			EditorPrefs.SetBool(DamageLog, DamageEnabled);
//		}

//		[MenuItem("ACT/Log/Damage", true)]
//		private static bool ToggleActionValidate()
//		{
//			Menu.SetChecked("ACT/Log/Damage", DamageEnabled);
//			return true;
//		}
//	}
//}
//#endif