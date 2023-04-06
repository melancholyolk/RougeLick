using RougeLike.Battle;
using UnityEngine;
namespace RougeLike.UI
{
	public class BaseUIView : MonoBehaviour
	{
		public virtual void OpenView()
		{
			MonoECS.instance.PushPause();
		}

		public virtual void CloseView()
		{
			MonoECS.instance.PopPause();
		}
	}
}
