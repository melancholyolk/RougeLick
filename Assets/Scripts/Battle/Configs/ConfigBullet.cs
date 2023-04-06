using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using RougeLike.Battle.Action;
using UnityEngine;
namespace RougeLike.Battle.Configs
{
	public abstract class ConfigBullet : SerializedScriptableObject
	{
		/// <summary>
		/// 胶囊体配置
		/// </summary>
		[Serializable]
		public struct CapsuleConfig
		{
			public float height;
			public float radius;
		}
		public enum DetectType
		{
			Overlap,
			Collision,
		}
		[BoxGroup("碰撞体配置"), LabelText("碰撞体配置")]
		public CapsuleConfig colliderConfig;
		[BoxGroup("碰撞体配置"), LabelText("碰撞检测类型")]
		public DetectType detectType;
		[BoxGroup("碰撞体配置"), LabelText("生成大小系数"),DefaultValue(1)]
		public float startSize;
		[BoxGroup("碰撞体配置"), LabelText("结束大小系数"),DefaultValue(1)]
		public float endSize;
		[BoxGroup("基础配置设置"), LabelText("特效预制")]
		public GameObject effect;
		[BoxGroup("基础配置设置"), LabelText("伤害")]
		public int damage;
		// [BoxGroup("基础配置设置"), LabelText("持续时间")]
		// public float duration;
		[BoxGroup("速度配置"), LabelText("是否使用速度方向")]
		public bool useVelocityDir;
		[BoxGroup("速度配置"),LabelText("与速度方向夹角")]
		public Vector3 velocityAngle;
		[BoxGroup("回调"), LabelText("当击中敌人")] 
		public ActionConfig[] onHitEnemy;
		[BoxGroup("回调"), LabelText("当子弹销毁")] 
		public ActionConfig[] onDestroySelf;
		
		public abstract void DoMovement(EntityBehave bullet,float delta);
		public abstract void Release(EntityBehave bullet);
	}
	
	
}

