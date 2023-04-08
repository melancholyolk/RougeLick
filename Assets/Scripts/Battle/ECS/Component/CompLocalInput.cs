using UnityEngine;

namespace RougeLike.Battle
{

	public enum Fire
	{
		down = 1,
		press,
		release,
		click
	}
	public struct ButtonState
	{
		public bool pressed;
		public bool currentFramePressed;

		public Fire fire;
		public float pressDuration;
	}

	public struct Vector2State
	{
		public Vector2 dir;
		public bool pressed;
	}


	public class CompLocalInput : ECSComponent
	{
		public Vector2State move;

		public ButtonState dodgeState;
		public bool disableWeapon;
		public ButtonState weaponState;
		public bool disableSwitchWeapon1, disableSwitchWeapon2, disableSwitchWeapon3, disableSwitchWeapon4;
		public ButtonState switchWeapon1State, switchWeapon2State, switchWeapon3State, switchWeapon4State;
		public ButtonState esc;

		public ButtonState[] skillsState = new ButtonState[20];

		public void Reset()
		{
			//throw new System.NotImplementedException();
		}
	}
}
