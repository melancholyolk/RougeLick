using Sirenix.OdinInspector;
using UnityEngine;

namespace RougeLike.Battle.Configs
{
	//绕圈走的子弹
	public class ConfigCircleBullet : ConfigBullet
	{
		[BoxGroup("角速度配置"), LabelText("角速度大小(角色为原点)")]
		public float angleSpeed;
		public ConfigCircleBullet()
		{
			detectType = DetectType.Overlap;
		}
		public override void DoMovement(EntityBehave bullet, float delta)
		{
		}

		public override void Release(EntityBehave bullet)
		{
			//销毁之后检查是否需要释放BulletGroup
			
			var group = bullet.compBullet.bulletGroup;
			group.children.Remove(bullet);
			
		}
	}
}