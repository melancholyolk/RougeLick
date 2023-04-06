using System;
using UnityEngine;

namespace RougeLike.Battle
{
	public class CompPhysic : ECSComponent
	{
		public EntityBehave entity;

		public Rigidbody rigidbody;

		private float m_force;

		public float Force
		{
			get { return m_force; }
			set { m_force = value; }
		}

		protected float _speed;
		protected Vector3 _velocity;
		protected Vector3 _accelerate;
		public float Speed
		{
			get { return _speed; }
		}

		public Vector3 Velocity
		{
			set { _velocity = value;}
			get { return _velocity; }
		}

		public Vector3 Accelerate
		{
			set { _accelerate = value;}
			get { return _accelerate; }
		}
		public void Reset()
		{

		}

		public void Move()
		{
			Debug.Log("方向：" + entity.compInput.dir);
		}

	}

	public class CompInput : ECSComponent
	{
		[Flags]
		public enum EInputDir
		{
			None = 0,
			Left = 1 << 0,//1
			Right = 1 << 1,//2
			Up = 1 << 2,
			Down = 1 << 3,
		}

		private Vector2 m_Dir;
		private EInputDir m_InputDir;
		/// <summary>
		/// 万向方向
		/// </summary>
		public Vector2 dir
		{
			get { return m_Dir; }
			set
			{
				m_Dir = value.normalized;
				if (m_Dir != Vector2.zero)
				{
					var angle = Vector2.SignedAngle(Vector2.right, m_Dir);
					if (-67.5f < angle && angle <= 67.5f)
					{
						m_InputDir |= EInputDir.Right;
					}
					if (22.5f < angle && angle <= 157.5f)
					{
						m_InputDir |= EInputDir.Up;
					}
					if (-157.5f < angle && angle <= -22.5f)
					{
						m_InputDir |= EInputDir.Down;
					}
					if (112.5f < angle || angle <= -112.5f)
					{
						m_InputDir |= EInputDir.Left;
					}
				}
				dirX = m_InputDir;
				dirY = m_InputDir;
			}
		}


		private EInputDir m_DirX;
		/// <summary>
		/// 水平方向
		/// </summary>
		public EInputDir dirX
		{
			get { return m_DirX; }
			private set
			{
				if (m_DirX != value)
				{
					m_DirX = value;
				}
			}
		}
		private EInputDir m_DirY;
		/// <summary>
		/// 垂直方向
		/// </summary>
		public EInputDir dirY
		{
			get { return m_DirY; }
			private set
			{
				if (m_DirY != value)
				{
					m_DirY = value;
				}
			}
		}

		public void Reset()
		{

		}
	}
}

