using RougeLike.Battle;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
	private EntityBehave entity;

	private Vector3 m_dir;
	private Transform m_transform;
	private Rigidbody m_rigidbody;
	private Animator m_animator;
	public void Start()
	{
		entity = GetComponent<MonoEntity>().entity;
		m_transform = entity.compTransform.transform;
		m_rigidbody = entity.compPhysic.rigidbody;
		m_animator = entity.compAnimator.animator;
	}
	public void Update()
	{
		if (!MonoECS.instance.running)
		{
			Pause();
			return;
		}
		Move();
		Turn();
	}
	public void Move()
	{
		m_animator.speed = 1;
		switch (entity.entityType)
		{
			case EntityBehave.EntityType.Player:
				{
					var dir = entity.compInput.dir;
					if (dir == null || dir == Vector2.zero)
					{
						m_animator.SetBool("Run", false);
						m_rigidbody.velocity = Vector3.zero;
						return;
					}
					m_dir = new Vector3(dir.x, 0, dir.y);
					m_rigidbody.velocity = m_dir * 10f * (1 + entity.compCharacter.speedBonus);
					m_animator.SetBool("Run", true);
					m_animator.speed = MonoECS.instance.TimeScale;
					break;
				}
			case EntityBehave.EntityType.Enemy:
				{
					if(!entity.compMonster.m_dead)
                    {
						m_animator.SetBool("Run", true);
						var dir = MonoECS.instance.mainEntity.transform.position - m_transform.position;
						m_dir = dir.normalized;
						m_rigidbody.velocity = m_dir * m_rigidbody.mass * 7 * entity.compMonster.info.speed * MonoECS.instance.systemTime.MonsterTimeScale;
                    }
                    else
                    {
						m_rigidbody.velocity  = Vector3.zero;
					}
					
					break;
				}
		}

	}
	public void Turn()
	{
		Vector3 turnForward = Vector3.RotateTowards(transform.forward, m_dir, 5 * Time.deltaTime * MonoECS.instance.TimeScale, 0f);
		transform.rotation = Quaternion.LookRotation(turnForward);
	}

	private void Pause()
	{
		m_animator.speed = 0;
		m_rigidbody.velocity = Vector3.zero;
	}
}
