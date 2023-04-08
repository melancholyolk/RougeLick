using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;
using static RougeLike.Battle.SystemBullet;

namespace RougeLike.Battle.Configs
{
    /// <summary>
    /// 武器攻击模式
    /// </summary>
    public abstract class ConfigAttackPattern
    {
	    public abstract void SetPosition(EntityBehave owner, ConfigWeapon weapon, CompWeapon.runTimeInfo info);
	    public abstract void SetPosition(EntityBehave owner,EntityBehave target, ConfigWeapon weapon, CompWeapon.runTimeInfo info);
    }
    public class ConfigSimpleBullet : ConfigAttackPattern
    {
	    [BoxGroup("生成子弹配置"), LabelText("子弹")]
	    public ConfigBullet bullet;
	    public int totalCount;
	    public bool useEntityFaceDir;
	    public bool useRandomDir;
	    [Range(0,360)]
        public float angleRange;
        public float angleOffset;
		public Vector3 offset;
        public Vector3 initSpeed;
        public float intantiateDelay;
        public override async void SetPosition(EntityBehave owner, ConfigWeapon weapon, CompWeapon.runTimeInfo info)
        {
	        var config = weapon.configs[info.level];
	        var bullets = new EntityBehave[totalCount];
	        for (int i = 0; i < totalCount; i++)
	        {
		        var pair = SpawnEntity.Instance.CreateBullet(bullet.effect);
		        var go = pair.Item1;
		        var behave = pair.Item2;
		        bullets[i] = behave;
		        behave.compTransform.transform = go.transform;
		        behave.compBullet.owner = owner;
		        behave.compBullet.config = bullet;
		        behave.compBullet.lifeTime = config.lifeTime;
		        behave.compBullet.isForever = info.CD <= config.lifeTime && !config.isRepeat;
		        behave.compBullet.weaponInfo = info;
		        behave.compBullet.hitCounter = config.hitCount;
		        behave.compBullet.damage = config.damage;
		        info.isForever = behave.compBullet.isForever;
		        info.currentBullet.Add(behave);
#if UNITY_EDITOR
				var box = new GameObject("DebugBullet");
				var boxComp = box.AddComponent<CapsuleCollider>();
				boxComp.direction = 2;
				boxComp.isTrigger = true;
				box.layer = 6;
				boxComp.radius = bullet.colliderConfig.radius;
				boxComp.height = bullet.colliderConfig.height;
				box.transform.SetParent(go.transform);
				box.transform.localPosition = Vector3.zero;
				box.transform.localRotation = Quaternion.identity;
				behave.compBullet.OnRelease.AddListener(() => {GameObject.Destroy(box);});
#endif
	        }
	        var hasOwner = bullet is ConfigCircleBullet;
            if(!hasOwner) {
				var trans = MonoECS.instance.mainEntity.entity.compTransform.transform;
				Vector3 actualVelocity = useEntityFaceDir ? trans.TransformVector(initSpeed) : initSpeed;
				Vector3 catchedPosition = trans.position;
				Vector3 catchedLocalOffSet = trans.TransformVector(offset);
				for (int i = 0; i < bullets.Length; i++)
				{
                    bullets[i].compTransform.transform.gameObject.SetActive(false);
                    var rotate =
	                    (useRandomDir
		                    ? Quaternion.AngleAxis(Random.Range(0, angleRange), Vector3.up)
		                    : Quaternion.AngleAxis(i * angleRange, Vector3.up)) *
	                    Quaternion.AngleAxis(angleOffset, Vector3.up);
                    bullets[i].compTransform.transform.position = useEntityFaceDir ? catchedPosition + rotate * catchedLocalOffSet : catchedPosition + rotate * offset;
					bullets[i].compPhysic.Velocity = rotate * actualVelocity;
					bullets[i].compTransform.transform.rotation = Quaternion.FromToRotation(Vector3.forward, initSpeed);                
				}
			}
            else
            {
                var bulletGroup = MonoECS.instance.systemBullet.bulletGroup.Find((l) => l.children.Count == 0);
                if(bulletGroup == null )
                {
					var newGo = new GameObject("NewGroup" + MonoECS.instance.systemBullet.bulletGroup.Count);
					bulletGroup = new BulletGroup();
					MonoECS.instance.systemBullet.bulletGroup.Add(bulletGroup);
					bulletGroup.transform = newGo.transform;
				}
                var circleConfig = bullets[0].compBullet.config as ConfigCircleBullet;
                bulletGroup.angleSpeed = circleConfig.angleSpeed;
                bulletGroup.transform.rotation = MonoECS.instance.mainEntity.entity.compTransform.transform.rotation;
				for (int i = 0; i < bullets.Length; i++)
				{
					bullets[i].compTransform.transform.gameObject.SetActive(false);
					var rotate = Quaternion.AngleAxis(i * angleRange, Vector3.up) * Quaternion.AngleAxis(angleOffset, Vector3.up);
                    bullets[i].compTransform.transform.SetParent(bulletGroup.transform);
                    bullets[i].compTransform.transform.position = useEntityFaceDir
	                    ? bulletGroup.transform.position + rotate * bulletGroup.transform.TransformVector(offset) : bulletGroup.transform.position + rotate * offset;
                    bullets[i].compBullet.bulletGroup = bulletGroup;
				}
			}
            //延迟渲染
            foreach(var t in bullets)
            {
	            t.compTransform.transform.gameObject.SetActive(true);
                t.compTransform.transform.localScale = t.compBullet.config.startSize * Vector3.one;
                MonoECS.instance.EnqueueBehave(t);
                if (intantiateDelay > 0)
					await UniTask.Delay((int)(intantiateDelay * 1000), delayTiming: PlayerLoopTiming.Update);
			}
        }

        public override void SetPosition(EntityBehave owner, EntityBehave target, ConfigWeapon weapon, CompWeapon.runTimeInfo info)
        {
	        throw new System.NotImplementedException();
        }
    }

    public class ConfigAttachEntityBullet : ConfigAttackPattern
    {
	    [BoxGroup("生成子弹配置"), LabelText("子弹")]
	    public ConfigBullet bullet;
	    public int totalCount;
	    public float intantiateDelay;
	    public Vector3 offset;
	    public Vector3 initSpeed;
	    public override void SetPosition(EntityBehave owner, ConfigWeapon weapon, CompWeapon.runTimeInfo info)
	    {
		    throw new System.NotImplementedException();
	    }
	    

	    public override async void SetPosition(EntityBehave owner, EntityBehave target, ConfigWeapon weapon, CompWeapon.runTimeInfo info)
	    {
		    var config = weapon.configs[info.level];
		    var bullets = new EntityBehave[totalCount];
		    for (int i = 0; i < totalCount; i++)
		    {
			    var pair = SpawnEntity.Instance.CreateBullet(bullet.effect);
			    var go = pair.Item1;
			    var behave = pair.Item2;
			    bullets[i] = behave;
			    behave.compTransform.transform = go.transform;
			    behave.compBullet.owner = owner;
			    behave.compBullet.config = bullet;
			    behave.compBullet.lifeTime = config.lifeTime;
			    behave.compBullet.isForever = config.CD <= config.lifeTime && !config.isRepeat;
			    behave.compBullet.weaponInfo = info;
			    behave.compBullet.hitCounter = config.hitCount;
			    behave.compBullet.damage = config.damage;
			    info.isForever = behave.compBullet.isForever;
			    info.currentBullet.Add(behave);
#if UNITY_EDITOR
			    var box = new GameObject("DebugBullet");
			    var boxComp = box.AddComponent<CapsuleCollider>();
			    boxComp.direction = 2;
			    boxComp.isTrigger = true;
			    box.layer = 6;
			    boxComp.radius = bullet.colliderConfig.radius;
			    boxComp.height = bullet.colliderConfig.height;
				box.transform.SetParent(go.transform);
				box.transform.localPosition = Vector3.zero;
			    box.transform.localRotation = Quaternion.identity;
			    behave.compBullet.OnRelease.AddListener(() => {GameObject.Destroy(box);});
#endif
		    }

		    var trans = target == null
			    ? MonoECS.instance.mainEntity.transform.position
			    : target.compTransform.position;
		    foreach (var t in bullets)
		    {
			    t.compTransform.transform.gameObject.SetActive(false);
			    t.compTransform.transform.position = trans + offset;
			    t.compPhysic.Velocity = initSpeed;
		    }
		    //延迟渲染
		    foreach(var t in bullets)
		    {
			    t.compTransform.transform.gameObject.SetActive(true);
			    t.compTransform.transform.localScale = t.compBullet.config.startSize * Vector3.one;
			    MonoECS.instance.EnqueueBehave(t);
			    if (intantiateDelay > 0)
				    await UniTask.Delay((int)(intantiateDelay * 1000), delayTiming: PlayerLoopTiming.Update);
		    }
	    }
    }
    public class ConfigFlameThrower : ConfigAttackPattern
    {
	    public ConfigAttackPattern attackPattern;
	    [BoxGroup("冷却时间")]
	    public float CD;

	    [BoxGroup("持续时间")]
	    public float time;
	    public override async void SetPosition(EntityBehave owner, ConfigWeapon weapon, CompWeapon.runTimeInfo info)
	    {
		    float timer = 0;
		    while (timer < time)
		    {
			    attackPattern.SetPosition(owner,weapon,info);
			    var delTime = CD * MonoECS.instance.systemTime.timeScale;
			    timer += delTime;
			    await UniTask.Delay((int)(delTime * 1000));
		    }
	    }

	    public override void SetPosition(EntityBehave owner, EntityBehave target, ConfigWeapon weapon, CompWeapon.runTimeInfo info)
	    {
		    throw new System.NotImplementedException();
	    }
    }
	/// <summary>
	/// 范围内寻找一定数量的敌人，对敌人施加特效
	/// </summary>
    public class ConfigTrackEntityBullet : ConfigAttackPattern
	{
		public int enemyCount;
		public float radius;
		public float delayTime;
		public ConfigAttachEntityBullet attackPattern;
		public override async void SetPosition(EntityBehave owner, ConfigWeapon weapon, CompWeapon.runTimeInfo info)
	    {
		    var monsters = MonoECS.instance.GetAllMonster();
		    for (int i = monsters.Count - 1; i >= 0; i--)
		    {
			    if (monsters[i].compTransform == null)
			    {
				    monsters.Remove(monsters[i]);
			    }
		    }

		    SortedList<float,EntityBehave> temp = new SortedList<float, EntityBehave>();
		    foreach (var m in monsters)
		    {
			    var dis = Vector3.Distance(m.compTransform.position, MonoECS.instance.mainEntity.transform.position);
			    if (dis <= radius)
			    {
				    temp.TryAdd(dis, m);
				    if(temp.Count > enemyCount)
					    break;
			    }
		    }
		    int num = Mathf.Min(temp.Count, enemyCount);
		    foreach (var t in temp)
		    {
			    attackPattern.SetPosition(owner,t.Value,weapon,info);
			    if(delayTime > 0)
				    await UniTask.Delay((int)(1000 * delayTime));
		    }
	    }

		public override void SetPosition(EntityBehave owner, EntityBehave target, ConfigWeapon weapon, CompWeapon.runTimeInfo info)
		{
			throw new System.NotImplementedException();
		}
	}
}