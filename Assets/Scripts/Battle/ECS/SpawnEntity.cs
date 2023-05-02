using Cinemachine;
using Cysharp.Threading.Tasks;
using RougeLike.Battle.Configs;
using RougeLike.Tool;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace RougeLike.Battle
{
	public class SpawnEntity
	{
		private static SpawnEntity _instance;

		public static SpawnEntity Instance
		{
			get
			{
				if (_instance == null)
					_instance = new SpawnEntity();
				return _instance;
			}
		}

		public MonoEntity SpawnCharacterEntity(ConfigCharacter config)
		{
			var main = EntityPool.Instance.GetGameObject(config.prefab, out bool back);
			main.GetComponentInChildren<ParticleSystem>().Play();
			var entity = main.GetComponent<MonoEntity>();
			var behave = back ? entity.entity : EntityBehave.GetCharacterEntityBehave();
			MonoECS.instance.EnqueueBehave(behave);
			main.transform.position = Vector3.zero;
			main.transform.rotation = Quaternion.identity;
			entity.SetEB(behave);
			behave.entityType = EntityBehave.EntityType.Player;
			behave.compPhysic.rigidbody = main.GetComponent<Rigidbody>();
			behave.compTransform.transform = main.transform;
			behave.compAnimator.animator = main.GetComponentInChildren<Animator>();
			behave.compCharacter.Init(config.m_HP,config.m_damageBonus,config.m_expBonus,config.m_criticalBonus,
				config.m_criticalDamageBonus,config.m_speedBonus,config.m_burialBonus,config.m_defenseBonus,config.m_recoverHP);
			behave.compCharacter.levelInfos = config.level.levelInfos;
			//暴击
			behave.compCharacter.criticalDamageBonus = 1;
			behave.compCharacter.criticalBonus = 0.5f;
			
			behave.compSkill.entity = behave;
			behave.compSkill.AddSkill(config.configWeapon1);
			var camobj = GameObject.Instantiate(config.vcam);
			var cam = camobj.GetComponent<CinemachineVirtualCamera>();
			cam.Follow = main.transform;
			cam.LookAt = main.transform;
			return entity;
		}

		public MonoEntity SpawnMonsterEntity(ConfigCharacter config, int stage,float hpPercent)
		{
			var main = EntityPool.Instance.GetGameObject(config.prefab, out bool back);
			var entity = main.GetComponent<MonoEntity>();
			var behave = back ? entity.entity : EntityBehave.GetMonsterEntityBehave();
			MonoECS.instance.EnqueueBehave(behave);
			behave.entityType = EntityBehave.EntityType.Enemy;
			behave.compPhysic.rigidbody = main.GetComponent<Rigidbody>();
			behave.compTransform.transform = main.transform;
			behave.compAnimator.animator = main.GetComponentInChildren<Animator>();
			behave.compMonster.entity = behave;
			behave.compMonster.HP = config.HP * hpPercent;
			behave.compMonster.info = config.monsterInfo;
			behave.compMonster.stage = stage;
			behave.compMonster.Reset();
			var randomPos = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)).normalized * 30;
			main.transform.position = MonoECS.instance.mainEntity.transform.position + randomPos;
			main.transform.rotation = Quaternion.identity;
			entity.SetEB(behave);
			return entity;
		}
		public (GameObject, EntityBehave) CreateBullet(GameObject prefab,string name = "",bool needPool = true)
		{
			var go = needPool ? Object.Instantiate(prefab) : EntityPool.Instance.GetBullet(prefab, name);
			var behave = EntityBehave.GetBulletEntityBehave();
			behave.entityType = EntityBehave.EntityType.Bullet;
			return (go, behave);
		}
		public async UniTask<(GameObject, EntityBehave)> AsyncCreateBullet(string nameReference)
		{
			GameObject go = await EntityPool.Instance.AsyncGetGameObject(nameReference);
			var behave = EntityBehave.GetBulletEntityBehave();
			behave.entityType = EntityBehave.EntityType.Bullet;
			return (go, behave);
		}
		public void SpawnBullet(EntityBehave owner, ConfigWeapon weapon, CompWeapon.runTimeInfo info)
		{
			//先检查子弹是否是全场唯一的
			var config = weapon.configs[info.level];
			//有主子弹，跟随人物移动
			config.attackPattern.SetPosition(owner, weapon ,info);
		}

		public void ReleaseEntityBehave(EntityBehave toRelease)
		{
			Object.Destroy(toRelease.compTransform.transform);

		}
	}
}

