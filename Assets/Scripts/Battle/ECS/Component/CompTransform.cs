using RougeLike.Tool;
using UnityEngine;

namespace RougeLike.Battle
{
	public class CompTransform : ECSComponent
	{
		public Transform transform;
		public Vector3 size;
		public Vector3 position => transform.position;
		public Vector3 rotation => transform.eulerAngles;
		public void Reset()
		{
			size = Vector3.zero;
		}
	}
}