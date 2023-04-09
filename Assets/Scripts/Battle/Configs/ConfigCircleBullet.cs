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
			var g = bullet.compBullet.bulletGroup;
			var rotate = Quaternion.AngleAxis(g.angleSpeed * delta, Vector3.up);
			var w = g.transform.rotation.eulerAngles;
			var r = bullet.compTransform.position - g.transform.position;
			bullet.compPhysic.Velocity = Vector3.Cross(w, r);
		}
	}
}