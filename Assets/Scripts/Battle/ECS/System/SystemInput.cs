using UnityEngine;
using static RougeLike.Battle.CompInput;

namespace RougeLike.Battle
{
	public class SystemInput : ECSSystem
	{
		EntityLocal m_input;

		const float PRESS_THRESHOLD = 0.2f;

		public BattleInputState battleInput;

		InputState m_InputState;
		bool m_couldInput;
		public InputState InputState
		{
			get => m_InputState;
			set
			{
				m_InputState?.OnExit();
				value?.OnEnter();
				m_InputState = value;
			}
		}
		public bool CouldInput
		{
			get => m_couldInput;
			set
			{
				m_couldInput = value;
			}
		}

		public override void FixedUpdate()
		{
			if (m_InputState == null)
				return;

			m_InputState.OnMove();

			HandleButton(ref m_input.compLocalInput.dodgeState);
			m_InputState.OnDodge();

			HandleButton(ref m_input.compLocalInput.weaponState);
			m_InputState.OnWeapon();

			HandleButton(ref m_input.compLocalInput.switchWeapon1State);
			m_InputState.OnSwitchWeapon1();

			HandleButton(ref m_input.compLocalInput.switchWeapon2State);
			m_InputState.OnSwitchWeapon2();

			HandleButton(ref m_input.compLocalInput.switchWeapon3State);
			m_InputState.OnSwitchWeapon3();

			HandleButton(ref m_input.compLocalInput.switchWeapon4State);
			m_InputState.OnSwitchWeapon4();
		}

		public void Init(EntityLocal el)
		{
			m_input = el;
			battleInput = new BattleInputState();
			m_couldInput = true;
		}

		void HandleButton(ref ButtonState state)
		{
			state.fire = 0;
			if (state.currentFramePressed)
			{
				if (!state.pressed)
				{
					state.pressed = true;
					state.pressDuration = 0;
					//state.fire = 1;
					state.fire = Fire.down;
				}
				else
				{
					float before = state.pressDuration;
					state.pressDuration += Time.fixedDeltaTime;
					if (before < PRESS_THRESHOLD && state.pressDuration >= PRESS_THRESHOLD)
					{
						//state.fire = 2;
						state.fire = Fire.press;
					}
				}
			}
			else
			{
				if (state.pressed)
				{
					state.pressed = false;
					if (state.pressDuration >= PRESS_THRESHOLD)
					{
						//state.fire = 3;
						state.fire = Fire.release;
					}
					else
					{
						//state.fire = 4;
						state.fire = Fire.click;
					}
				}
			}
		}

	}
	public abstract class InputState
	{
		protected EntityLocal m_input;
		protected EntityBehave controled;

		public virtual void OnEnter()
		{
			m_input = MonoECS.instance.entityLocal;
			controled = MonoECS.instance.mainEntity.entity;
		}

		public virtual void OnExit()
		{

		}

		public virtual void OnMove()
		{

		}

		public virtual void OnDodge()
		{

		}


		public virtual void OnWeapon()
		{

		}


		public virtual void OnSwitchWeapon1()
		{

		}

		public virtual void OnSwitchWeapon2()
		{

		}

		public virtual void OnSwitchWeapon3()
		{

		}

		public virtual void OnSwitchWeapon4()
		{

		}

	}
	public class BattleInputState : InputState
	{
		public override void OnMove()
		{
			if (m_input.compLocalInput.move.pressed)
			{
				var dir = m_input.compLocalInput.move.dir;
				controled.compInput.dir = dir;
			}
			else
			{
				controled.compInput.dir = Vector2.zero;
			}
		}

		public override void OnDodge()
		{
			if (m_input.compLocalInput.dodgeState.fire > 0)
			{
				switch (m_input.compLocalInput.dodgeState.fire)
				{
					case Fire.down:
						{
							Debug.Log("OnDodge");
							break;
						}
				}
			}

		}


		public override void OnWeapon()
		{
		}


		public override void OnSwitchWeapon1()
		{
			if (m_input.compLocalInput.switchWeapon1State.fire > 0)
			{
				switch (m_input.compLocalInput.switchWeapon1State.fire)
				{
					case Fire.down:
						{
							if (MonoECS.instance.skillPanel.gameObject.activeInHierarchy)
								MonoECS.instance.skillPanel.ConfirmSkill(0);
							break;
						}
				}
			}

		}

		public override void OnSwitchWeapon2()
		{
			if (m_input.compLocalInput.switchWeapon2State.fire > 0)
			{
				switch (m_input.compLocalInput.switchWeapon2State.fire)
				{
					case Fire.down:
						{
							if (MonoECS.instance.skillPanel.gameObject.activeInHierarchy)
								MonoECS.instance.skillPanel.ConfirmSkill(1);
							break;
						}
				}
			}

		}

		public override void OnSwitchWeapon3()
		{
			if (m_input.compLocalInput.switchWeapon3State.fire > 0)
			{
				switch (m_input.compLocalInput.switchWeapon3State.fire)
				{
					case Fire.down:
						{
							if(MonoECS.instance.skillPanel.gameObject.activeInHierarchy)
							MonoECS.instance.skillPanel.ConfirmSkill(2);
							break;
						}
				}
			}

		}

		public override void OnSwitchWeapon4()
		{
			if (m_input.compLocalInput.switchWeapon4State.fire > 0)
			{
				switch (m_input.compLocalInput.switchWeapon4State.fire)
				{
					case Fire.down:
						{
							MonoECS.instance.OpenAttribute();
							break;
						}
				}
			}

		}
	}
}
