/// <summary>
/// 代码由 Assets\Scripts\Editor\CodeGen\DebugMenuGen.cs 生成；参阅<seealso cref="DebugMenuGen"/>
/// </summary>
#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
namespace Fundamental
{
	public class LogManager : OdinEditorWindow
	{
		const string DamageLog = "DamageLog";
		const string BehaviorTreeLog = "BehaviorTreeLog";
		const string AbilityLog = "AbilityLog";
		const string NetCodeLog = "NetCodeLog";
		const string BluePrintLog = "BluePrintLog";
		const string MotionLog = "MotionLog";
		const string Motion_WaitLeftLog = "Motion_WaitLeftLog";
		const string WorldLog = "WorldLog";
		const string World_WorldAssetLog = "World_WorldAssetLog";
		const string ObjectPoolLog = "ObjectPoolLog";
		const string StateObjectLog = "StateObjectLog";
		const string StateObject_MonsterPointLog = "StateObject_MonsterPointLog";
		[MenuItem("ACT/LogManager")]
		public static void Create()
		{
			DebugMenu.DamageEnabled = EditorPrefs.GetBool(DamageLog, true);
			DebugMenu.BehaviorTreeEnabled = EditorPrefs.GetBool(BehaviorTreeLog, true);
			DebugMenu.AbilityEnabled = EditorPrefs.GetBool(AbilityLog, true);
			DebugMenu.NetCodeEnabled = EditorPrefs.GetBool(NetCodeLog, true);
			DebugMenu.BluePrintEnabled = EditorPrefs.GetBool(BluePrintLog, true);
			DebugMenu.MotionEnabled = EditorPrefs.GetBool(MotionLog, true);
			DebugMenu.Motion_WaitLeftEnabled = EditorPrefs.GetBool(Motion_WaitLeftLog, true);
			DebugMenu.WorldEnabled = EditorPrefs.GetBool(WorldLog, true);
			DebugMenu.World_WorldAssetEnabled = EditorPrefs.GetBool(World_WorldAssetLog, true);
			DebugMenu.ObjectPoolEnabled = EditorPrefs.GetBool(ObjectPoolLog, true);
			DebugMenu.StateObjectEnabled = EditorPrefs.GetBool(StateObjectLog, true);
			DebugMenu.StateObject_MonsterPointEnabled = EditorPrefs.GetBool(StateObject_MonsterPointLog, true);
			CreateWindow<LogManager>().Show();
		}
		
		[ShowInInspector,ToggleLeft,LabelText("Damage")]
		public bool Damage
		{
			get => DebugMenu.DamageEnabled;
			set
			{
				DebugMenu.DamageEnabled = value;
				EditorPrefs.SetBool(DamageLog, DebugMenu.DamageEnabled);
			}
		}
		
		[ShowInInspector,ToggleLeft,LabelText("BehaviorTree")]
		public bool BehaviorTree
		{
			get => DebugMenu.BehaviorTreeEnabled;
			set
			{
				DebugMenu.BehaviorTreeEnabled = value;
				EditorPrefs.SetBool(BehaviorTreeLog, DebugMenu.BehaviorTreeEnabled);
			}
		}
		
		[ShowInInspector,ToggleLeft,LabelText("Ability")]
		public bool Ability
		{
			get => DebugMenu.AbilityEnabled;
			set
			{
				DebugMenu.AbilityEnabled = value;
				EditorPrefs.SetBool(AbilityLog, DebugMenu.AbilityEnabled);
			}
		}
		
		[ShowInInspector,ToggleLeft,LabelText("NetCode")]
		public bool NetCode
		{
			get => DebugMenu.NetCodeEnabled;
			set
			{
				DebugMenu.NetCodeEnabled = value;
				EditorPrefs.SetBool(NetCodeLog, DebugMenu.NetCodeEnabled);
			}
		}
		
		[ShowInInspector,ToggleLeft,LabelText("BluePrint")]
		public bool BluePrint
		{
			get => DebugMenu.BluePrintEnabled;
			set
			{
				DebugMenu.BluePrintEnabled = value;
				EditorPrefs.SetBool(BluePrintLog, DebugMenu.BluePrintEnabled);
			}
		}
		
		[ShowInInspector,ToggleLeft,LabelText("Motion")]
		public bool Motion
		{
			get => DebugMenu.MotionEnabled;
			set
			{
				DebugMenu.MotionEnabled = value;
				EditorPrefs.SetBool(MotionLog, DebugMenu.MotionEnabled);
			}
		}
		[BoxGroup("Motion",false)]
		[ShowInInspector,ToggleLeft,LabelText("WaitLeft")]
		public bool Motion_WaitLeft
		{
			get => DebugMenu.Motion_WaitLeftEnabled;
			set
			{
				DebugMenu.Motion_WaitLeftEnabled = value;
				EditorPrefs.SetBool(Motion_WaitLeftLog, DebugMenu.Motion_WaitLeftEnabled);
			}
		}
		
		[ShowInInspector,ToggleLeft,LabelText("World")]
		public bool World
		{
			get => DebugMenu.WorldEnabled;
			set
			{
				DebugMenu.WorldEnabled = value;
				EditorPrefs.SetBool(WorldLog, DebugMenu.WorldEnabled);
			}
		}
		[BoxGroup("World",false)]
		[ShowInInspector,ToggleLeft,LabelText("WorldAsset")]
		public bool World_WorldAsset
		{
			get => DebugMenu.World_WorldAssetEnabled;
			set
			{
				DebugMenu.World_WorldAssetEnabled = value;
				EditorPrefs.SetBool(World_WorldAssetLog, DebugMenu.World_WorldAssetEnabled);
			}
		}
		
		[ShowInInspector,ToggleLeft,LabelText("ObjectPool")]
		public bool ObjectPool
		{
			get => DebugMenu.ObjectPoolEnabled;
			set
			{
				DebugMenu.ObjectPoolEnabled = value;
				EditorPrefs.SetBool(ObjectPoolLog, DebugMenu.ObjectPoolEnabled);
			}
		}
		
		[ShowInInspector,ToggleLeft,LabelText("StateObject")]
		public bool StateObject
		{
			get => DebugMenu.StateObjectEnabled;
			set
			{
				DebugMenu.StateObjectEnabled = value;
				EditorPrefs.SetBool(StateObjectLog, DebugMenu.StateObjectEnabled);
			}
		}
		[BoxGroup("StateObject",false)]
		[ShowInInspector,ToggleLeft,LabelText("MonsterPoint")]
		public bool StateObject_MonsterPoint
		{
			get => DebugMenu.StateObject_MonsterPointEnabled;
			set
			{
				DebugMenu.StateObject_MonsterPointEnabled = value;
				EditorPrefs.SetBool(StateObject_MonsterPointLog, DebugMenu.StateObject_MonsterPointEnabled);
			}
		}
		Vector2 scrollPosition;
		protected override void OnGUI()
		{
			scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true, GUILayout.Height(position.height - 30));
			base.OnGUI();
			GUILayout.EndScrollView();
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Select all"))
			{
				Damage = true;
				BehaviorTree = true;
				Ability = true;
				NetCode = true;
				BluePrint = true;
				Motion = true;
				Motion_WaitLeft = true;
				World = true;
				World_WorldAsset = true;
				ObjectPool = true;
				StateObject = true;
				StateObject_MonsterPoint = true;
			}
			if (GUILayout.Button("Deselect all"))
			{
				Damage = false;
				BehaviorTree = false;
				Ability = false;
				NetCode = false;
				BluePrint = false;
				Motion = false;
				Motion_WaitLeft = false;
				World = false;
				World_WorldAsset = false;
				ObjectPool = false;
				StateObject = false;
				StateObject_MonsterPoint = false;
			}
			GUILayout.EndHorizontal();
		}
	}
	public static partial class DebugMenu
	{
		const string DamageLog = "DamageLog";
		const string BehaviorTreeLog = "BehaviorTreeLog";
		const string AbilityLog = "AbilityLog";
		const string NetCodeLog = "NetCodeLog";
		const string BluePrintLog = "BluePrintLog";
		const string MotionLog = "MotionLog";
		const string Motion_WaitLeftLog = "Motion_WaitLeftLog";
		const string WorldLog = "WorldLog";
		const string World_WorldAssetLog = "World_WorldAssetLog";
		const string ObjectPoolLog = "ObjectPoolLog";
		const string StateObjectLog = "StateObjectLog";
		const string StateObject_MonsterPointLog = "StateObject_MonsterPointLog";
		const string EventTrackLog = "EventTrackLog";
		const string EventTrackDataLog = "EventTrackDataLog";
		static DebugMenu()
		{
			DamageEnabled = EditorPrefs.GetBool(DamageLog, true);
			BehaviorTreeEnabled = EditorPrefs.GetBool(BehaviorTreeLog, true);
			AbilityEnabled = EditorPrefs.GetBool(AbilityLog, true);
			NetCodeEnabled = EditorPrefs.GetBool(NetCodeLog, true);
			BluePrintEnabled = EditorPrefs.GetBool(BluePrintLog, true);
			MotionEnabled = EditorPrefs.GetBool(MotionLog, true);
			Motion_WaitLeftEnabled = EditorPrefs.GetBool(Motion_WaitLeftLog, true);
			WorldEnabled = EditorPrefs.GetBool(WorldLog, true);
			World_WorldAssetEnabled = EditorPrefs.GetBool(World_WorldAssetLog, true);
			ObjectPoolEnabled = EditorPrefs.GetBool(ObjectPoolLog, true);
			StateObjectEnabled = EditorPrefs.GetBool(StateObjectLog, true);
			StateObject_MonsterPointEnabled = EditorPrefs.GetBool(StateObject_MonsterPointLog, true);
			EventTrackEnabled = EditorPrefs.GetBool(EventTrackLog, true);
			EventTrackDataEnabled = EditorPrefs.GetBool(EventTrackDataLog, true);
		}

		[MenuItem("ACT/Log/Damage")]
		private static void ToggleDamage()
		{
			DamageEnabled = !DamageEnabled;
			EditorPrefs.SetBool(DamageLog, DamageEnabled);
		}

		[MenuItem("ACT/Log/Damage", true)]
		private static bool ToggleDamageActionValidate()
		{
			Menu.SetChecked("ACT/Log/Damage", DamageEnabled);
			return true;
		}

		[MenuItem("ACT/Log/BehaviorTree")]
		private static void ToggleBehaviorTree()
		{
			BehaviorTreeEnabled = !BehaviorTreeEnabled;
			EditorPrefs.SetBool(BehaviorTreeLog, BehaviorTreeEnabled);
		}

		[MenuItem("ACT/Log/BehaviorTree", true)]
		private static bool ToggleBehaviorTreeActionValidate()
		{
			Menu.SetChecked("ACT/Log/BehaviorTree", BehaviorTreeEnabled);
			return true;
		}

		[MenuItem("ACT/Log/Ability")]
		private static void ToggleAbility()
		{
			AbilityEnabled = !AbilityEnabled;
			EditorPrefs.SetBool(AbilityLog, AbilityEnabled);
		}

		[MenuItem("ACT/Log/Ability", true)]
		private static bool ToggleAbilityActionValidate()
		{
			Menu.SetChecked("ACT/Log/Ability", AbilityEnabled);
			return true;
		}

		[MenuItem("ACT/Log/NetCode")]
		private static void ToggleNetCode()
		{
			NetCodeEnabled = !NetCodeEnabled;
			EditorPrefs.SetBool(NetCodeLog, NetCodeEnabled);
		}

		[MenuItem("ACT/Log/NetCode", true)]
		private static bool ToggleNetCodeActionValidate()
		{
			Menu.SetChecked("ACT/Log/NetCode", NetCodeEnabled);
			return true;
		}

		[MenuItem("ACT/Log/BluePrint")]
		private static void ToggleBluePrint()
		{
			BluePrintEnabled = !BluePrintEnabled;
			EditorPrefs.SetBool(BluePrintLog, BluePrintEnabled);
		}

		[MenuItem("ACT/Log/BluePrint", true)]
		private static bool ToggleBluePrintActionValidate()
		{
			Menu.SetChecked("ACT/Log/BluePrint", BluePrintEnabled);
			return true;
		}

		[MenuItem("ACT/Log/Motion")]
		private static void ToggleMotion()
		{
			MotionEnabled = !MotionEnabled;
			EditorPrefs.SetBool(MotionLog, MotionEnabled);
		}

		[MenuItem("ACT/Log/Motion", true)]
		private static bool ToggleMotionActionValidate()
		{
			Menu.SetChecked("ACT/Log/Motion", MotionEnabled);
			return true;
		}

		[MenuItem("ACT/Log/Motion_WaitLeft")]
		private static void ToggleMotion_WaitLeft()
		{
			Motion_WaitLeftEnabled = !Motion_WaitLeftEnabled;
			EditorPrefs.SetBool(Motion_WaitLeftLog, Motion_WaitLeftEnabled);
		}

		[MenuItem("ACT/Log/Motion_WaitLeft", true)]
		private static bool ToggleMotion_WaitLeftActionValidate()
		{
			Menu.SetChecked("ACT/Log/Motion_WaitLeft", Motion_WaitLeftEnabled);
			return true;
		}

		[MenuItem("ACT/Log/World")]
		private static void ToggleWorld()
		{
			WorldEnabled = !WorldEnabled;
			EditorPrefs.SetBool(WorldLog, WorldEnabled);
		}

		[MenuItem("ACT/Log/World", true)]
		private static bool ToggleWorldActionValidate()
		{
			Menu.SetChecked("ACT/Log/World", WorldEnabled);
			return true;
		}

		[MenuItem("ACT/Log/World_WorldAsset")]
		private static void ToggleWorld_WorldAsset()
		{
			World_WorldAssetEnabled = !World_WorldAssetEnabled;
			EditorPrefs.SetBool(World_WorldAssetLog, World_WorldAssetEnabled);
		}

		[MenuItem("ACT/Log/World_WorldAsset", true)]
		private static bool ToggleWorld_WorldAssetActionValidate()
		{
			Menu.SetChecked("ACT/Log/World_WorldAsset", World_WorldAssetEnabled);
			return true;
		}

		[MenuItem("ACT/Log/ObjectPool")]
		private static void ToggleObjectPool()
		{
			ObjectPoolEnabled = !ObjectPoolEnabled;
			EditorPrefs.SetBool(ObjectPoolLog, ObjectPoolEnabled);
		}

		[MenuItem("ACT/Log/ObjectPool", true)]
		private static bool ToggleObjectPoolActionValidate()
		{
			Menu.SetChecked("ACT/Log/ObjectPool", ObjectPoolEnabled);
			return true;
		}

		[MenuItem("ACT/Log/StateObject")]
		private static void ToggleStateObject()
		{
			StateObjectEnabled = !StateObjectEnabled;
			EditorPrefs.SetBool(StateObjectLog, StateObjectEnabled);
		}

		[MenuItem("ACT/Log/StateObject", true)]
		private static bool ToggleStateObjectActionValidate()
		{
			Menu.SetChecked("ACT/Log/StateObject", StateObjectEnabled);
			return true;
		}

		[MenuItem("ACT/Log/StateObject_MonsterPoint")]
		private static void ToggleStateObject_MonsterPoint()
		{
			StateObject_MonsterPointEnabled = !StateObject_MonsterPointEnabled;
			EditorPrefs.SetBool(StateObject_MonsterPointLog, StateObject_MonsterPointEnabled);
		}

		[MenuItem("ACT/Log/StateObject_MonsterPoint", true)]
		private static bool ToggleStateObject_MonsterPointActionValidate()
		{
			Menu.SetChecked("ACT/Log/StateObject_MonsterPoint", StateObject_MonsterPointEnabled);
			return true;
		}
		
		[MenuItem("ACT/Log/EventTrack")]
		private static void ToggleEventTrack()
		{
			EventTrackEnabled = !EventTrackEnabled;
			EditorPrefs.SetBool(EventTrackLog, EventTrackEnabled);
		}

		[MenuItem("ACT/Log/EventTrack", true)]
		private static bool ToggleEventTrackActionValidate()
		{
			Menu.SetChecked("ACT/Log/EventTrack", EventTrackEnabled);
			return true;
		}
		
		[MenuItem("ACT/Log/EventDataTrack")]
		private static void ToggleEventDataTrack()
		{
			EventTrackDataEnabled = !EventTrackDataEnabled;
			EditorPrefs.SetBool(EventTrackDataLog, EventTrackDataEnabled);
		}

		[MenuItem("ACT/Log/EventDataTrack", true)]
		private static bool ToggleEventDataTrackActionValidate()
		{
			Menu.SetChecked("ACT/Log/EventDataTrack", EventTrackDataEnabled);
			return true;
		}
	}
}
#endif