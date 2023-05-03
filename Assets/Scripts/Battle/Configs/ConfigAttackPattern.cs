using Battle.ECS;
using Cysharp.Threading.Tasks;
using RougeLike.Battle.Action;
using RougeLike.Tool;
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
	    // public abstract void SetPosition(EntityBehave owner,EntityBehave target, ConfigWeapon weapon, CompWeapon.runTimeInfo info);
    }
    public class ConfigSimpleBullet : ConfigAttackPattern
    {
	    public FindTarget findTarget;
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
	        var result = Fundamental.ListPool<EntityBehave>.Get();
			findTarget.FindTargets(result);
			var entityTarget = result[0];
	        var config = weapon.configs[info.level];
	        var bullets = new EntityBehave[totalCount];
	        for (int i = 0; i < totalCount; i++)
	        {
		        var pair = SpawnEntity.Instance.CreateBullet(bullet.effect,bullet.bulletName,bullet.useObjectPool);
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
				var trans = entityTarget.compTransform.transform;
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
                bulletGroup.transform.rotation = entityTarget.compTransform.transform.rotation;
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
	            if(t.compTransform == null)
		            continue;
	            t.compTransform.transform.gameObject.SetActive(true);
	            foreach (var action in t.compBullet.config.onLaunch)
                {
	                action.Do(new Memory(){caster = t});
                }
                MonoECS.instance.EnqueueBehave(t);
                if (intantiateDelay > 0)
					await UniTask.Delay((int)(intantiateDelay * 1000 * MonoECS.instance.systemTime.timeScale), delayTiming: PlayerLoopTiming.Update);
			}
        }
    }

    public partial class ConfigRandomAreaBullet : ConfigAttackPattern
    {
	    public FindTarget findTarget;
	    public float radius;
	    public int totalCount;
	    public Vector3 initSpeed;
	    public Vector3 offset;
	    public float intantiateDelay;
	    public ConfigBullet bullet;
	    public override async void SetPosition(EntityBehave owner, ConfigWeapon weapon, CompWeapon.runTimeInfo info)
	    {
		    var result = Fundamental.ListPool<EntityBehave>.Get();
		    findTarget.FindTargets(result);
		    var pos = Fundamental.ListPool<Vector3>.Get();
		    for (int i = 0; i < totalCount; i++)
		    {
			    var p = Random.insideUnitCircle;
			    var translater = new Vector3(p.x * radius,0,p.y * radius) + result[0].compTransform.position;
			    pos.Add(translater);
		    }
		    var config = weapon.configs[info.level];
		    var bullets = new EntityBehave[totalCount];
		    for (int i = 0; i < totalCount; i++)
		    {
			    var pair = SpawnEntity.Instance.CreateBullet(bullet.effect,bullet.bulletName,bullet.useObjectPool);
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
			    Vector3 actualVelocity = initSpeed;
			    behave.compTransform.transform.gameObject.SetActive(false);
			    behave.compTransform.transform.position = pos[i] + offset;
			    behave.compPhysic.Velocity = actualVelocity;
			    behave.compTransform.transform.rotation = Quaternion.FromToRotation(Vector3.forward, initSpeed);
				    //延迟渲染
			    
		    }
		    foreach(var t in bullets)
		    {
			    if(t.compTransform == null)
				    continue;
			    t.compTransform.transform.gameObject.SetActive(true);
			    foreach (var action in t.compBullet.config.onLaunch)
			    {
				    action.Do(new Memory(){caster = t});
			    }
			    MonoECS.instance.EnqueueBehave(t);
			    if (intantiateDelay > 0)
				    await UniTask.Delay((int)(intantiateDelay * 1000 * MonoECS.instance.systemTime.timeScale), delayTiming: PlayerLoopTiming.Update);
		    }
	    }
    }

    public class ConfigMulTargetBullet : ConfigAttackPattern
    {
	    public FindTarget findTarget;
	    public int totalCount;
	    public Vector3 initSpeed;
	    public Vector3 offset;
	    public float intantiateDelay;
	    public ConfigBullet bullet;
	    public override async void SetPosition(EntityBehave owner, ConfigWeapon weapon, CompWeapon.runTimeInfo info)
	    {
		    var result = Fundamental.ListPool<EntityBehave>.Get();
		    findTarget.FindTargets(result);
		    var config = weapon.configs[info.level];
		    totalCount = Mathf.Min(totalCount, result.Count);
		    var bullets = new EntityBehave[totalCount];
		    for (int i = 0; i < totalCount; i++)
		    {
			    var pair = SpawnEntity.Instance.CreateBullet(bullet.effect,bullet.bulletName,bullet.useObjectPool);
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
			    Vector3 actualVelocity = initSpeed;
			    behave.compTransform.transform.gameObject.SetActive(false);
			    behave.compTransform.transform.position = result[i].compTransform.position + offset;
			    behave.compPhysic.Velocity = actualVelocity;
			    behave.compTransform.transform.rotation = Quaternion.FromToRotation(Vector3.forward, initSpeed);
		    }
		    //延迟渲染
		    foreach(var t in bullets)
		    {
			    if(t.compTransform == null)
				    continue;
			    t.compTransform.transform.gameObject.SetActive(true);
			    foreach (var action in t.compBullet.config.onLaunch)
			    {
				    action.Do(new Memory(){caster = t});
			    }
			    MonoECS.instance.EnqueueBehave(t);
			    if (intantiateDelay > 0)
				    await UniTask.Delay((int)(intantiateDelay * 1000 * MonoECS.instance.systemTime.timeScale), delayTiming: PlayerLoopTiming.Update);
		    }
		    Fundamental.ListPool<EntityBehave>.Release(result);
	    }
    }
    // public class ConfigAttachEntityBullet : ConfigAttackPattern
    // {
	   //  public FindTarget findTarget;
	   //  [BoxGroup("生成子弹配置"), LabelText("子弹")]
	   //  public ConfigBullet bullet;
	   //  public int totalCount;
	   //  public float intantiateDelay;
	   //  public Vector3 offset;
	   //  public Vector3 initSpeed;
	   //  public override void SetPosition(EntityBehave owner, ConfigWeapon weapon, CompWeapon.runTimeInfo info)
	   //  {
		  //   throw new System.NotImplementedException();
	   //  }
	    

// 	    public override async void SetPosition(EntityBehave owner, EntityBehave target, ConfigWeapon weapon, CompWeapon.runTimeInfo info)
// 	    {
// 		    var config = weapon.configs[info.level];
// 		    var bullets = new EntityBehave[totalCount];
// 		    for (int i = 0; i < totalCount; i++)
// 		    {
// 			    var pair = SpawnEntity.Instance.CreateBullet(bullet.effect,bullet.bulletName,bullet.useObjectPool);
// 			    var go = pair.Item1;
// 			    var behave = pair.Item2;
// 			    bullets[i] = behave;
// 			    behave.compTransform.transform = go.transform;
// 			    behave.compBullet.owner = owner;
// 			    behave.compBullet.config = bullet;
// 			    behave.compBullet.lifeTime = config.lifeTime;
// 			    behave.compBullet.isForever = config.CD <= config.lifeTime && !config.isRepeat;
// 			    behave.compBullet.weaponInfo = info;
// 			    behave.compBullet.hitCounter = config.hitCount;
// 			    behave.compBullet.damage = config.damage;
// 			    info.isForever = behave.compBullet.isForever;
// 			    info.currentBullet.Add(behave);
// #if UNITY_EDITOR
// 			    var box = new GameObject("DebugBullet");
// 			    var boxComp = box.AddComponent<CapsuleCollider>();
// 			    boxComp.direction = 2;
// 			    boxComp.isTrigger = true;
// 			    box.layer = 6;
// 			    boxComp.radius = bullet.colliderConfig.radius;
// 			    boxComp.height = bullet.colliderConfig.height;
// 				box.transform.SetParent(go.transform);
// 				box.transform.localPosition = Vector3.zero;
// 			    box.transform.localRotation = Quaternion.identity;
// 			    behave.compBullet.OnRelease.AddListener(() => {GameObject.Destroy(box);});
// #endif
// 		    }
//
// 		    var trans = target.compTransform?.position ?? MonoECS.instance.mainEntity.transform.position;
// 		    foreach (var t in bullets)
// 		    {
// 			    t.compTransform.transform.gameObject.SetActive(false);
// 			    t.compTransform.transform.position = trans + offset;
// 			    t.compPhysic.Velocity = initSpeed;
// 		    }
// 		    //延迟渲染
// 		    foreach(var t in bullets)
// 		    {
// 			    if(t.compTransform == null)
// 				    continue;
// 			    t.compTransform.transform.gameObject.SetActive(true);
// 			    foreach (var action in t.compBullet.config.onLaunch)
// 			    {
// 				    action.Do(new Memory(){caster = t});
// 			    }
// 			    MonoECS.instance.EnqueueBehave(t);
// 			    if (intantiateDelay > 0)
// 				    await UniTask.Delay((int)(intantiateDelay * 1000), delayTiming: PlayerLoopTiming.Update);
// 		    }
// 	    }
    // }
    public class ConfigFlameThrower : ConfigAttackPattern
    {
	    public GameObject effect;
	    public ConfigAttackPattern attackPattern;
	    [BoxGroup("冷却时间")]
	    public float CD;

	    [BoxGroup("持续时间")]
	    public float time;
	    public override async void SetPosition(EntityBehave owner, ConfigWeapon weapon, CompWeapon.runTimeInfo info)
	    {
		    GameObject go = null;
		    if (effect != null)
		    {
			    go = EntityPool.Instance.GetBullet(effect, "FireGun");
			    var particles = go.GetComponentsInChildren<ParticleSystem>();
			    go.transform.parent = MonoECS.instance.mainEntity.transform;
			    go.transform.localPosition = Vector3.up * 1.75f;
			    go.transform.localRotation = Quaternion.identity;
			    foreach (var particle in particles)
			    {
				    particle.Stop();
				    var main = particle.main;
				    main.duration = time;
			    }
			    foreach (var particle in particles)
			    {
				    particle.Play();
			    }
		    }
		    
		    float timer = 0;
		    while (timer < time)
		    {
			    attackPattern.SetPosition(owner,weapon,info);
			    var delTime = CD * MonoECS.instance.systemTime.timeScale;
			    timer += delTime;
			    await UniTask.Delay((int)(delTime * 1000 * MonoECS.instance.systemTime.timeScale));
		    }

		    await UniTask.Delay(10000);
		    if(effect != null)
				EntityPool.Instance.ReleaseBullet(go, "FireGun");
	    }
    }
	// /// <summary>
	// /// 范围内寻找一定数量的敌人，对敌人施加特效
	// /// </summary>
 //    public class ConfigTrackEntityBullet : ConfigAttackPattern
	// {
	// 	public int enemyCount;
	// 	public float radius;
	// 	public float delayTime;
	// 	public ConfigAttachEntityBullet attackPattern;
	// 	public override async void SetPosition(EntityBehave owner, ConfigWeapon weapon, CompWeapon.runTimeInfo info)
	// 	{
	// 		FindTarget ownerFind = new FindTarget();
	// 		ownerFind.regex = FindTarget.Regex.Closest;
	// 		ownerFind.type = EntityBehave.EntityType.Enemy;
	// 		var result = Fundamental.ListPool<EntityBehave>.Get();
	// 		ownerFind.FindTargets(result);
	// 	    var monsters = MonoECS.instance.GetAllMonster();
	// 	    for (int i = result.Count - 1; i >= 0; i--)
	// 	    {
	// 		    if (result[i].compTransform == null)
	// 		    {
	// 			    result.Remove(result[i]);
	// 			    continue;
	// 		    }
	// 		    if((MonoECS.instance.mainEntity.transform.position - result[i].compTransform.position).magnitude > radius)
	// 			    result.Remove(result[i]);
	// 	    }
	// 	    int num = Mathf.Min(result.Count, enemyCount);
	// 	    int j = 0;
	// 	    foreach (var t in result)
	// 	    {
	// 		    if(j >= num)
	// 			    break;
	// 		    j++;
	// 		    attackPattern.SetPosition(owner,t,weapon,info);
	// 		    if(delayTime > 0)
	// 			    await UniTask.Delay((int)(1000 * delayTime));
	// 	    }
	// 	    Fundamental.ListPool<EntityBehave>.Release(result);
	//     }
	// }
}