using Sirenix.OdinInspector;
using UnityEngine;

namespace RougeLike.Battle.Configs
{
	/// <summary>
	/// 直线走的子弹
	/// </summary>
	public class ConfigLineBullet : ConfigBullet
	{
		[BoxGroup("线速度配置"), LabelText("是否使用自身坐标")]
		public bool useLocal;
		[BoxGroup("线速度配置"), LabelText("加速度"), Tooltip("z轴为正方向")]
		public Vector3 acceleration;
		public ConfigLineBullet() 
		{
			detectType = DetectType.Collision;
		}
		public override void DoMovement(EntityBehave bullet,float delta)
		{
			//base.DoMovement(bullet);
			var trans = bullet.compTransform.transform;
			bullet.compPhysic.Accelerate = useLocal ? trans.TransformVector(acceleration) : acceleration;
			bullet.compPhysic.Velocity += bullet.compPhysic.Accelerate * delta;
			trans.position += bullet.compPhysic.Velocity * delta;
		}
	}
}