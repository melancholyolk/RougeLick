
using UnityEngine;
namespace RougeLike.UI
{
	public class UIManage : MonoBehaviour
	{
		private static UIManage _instance;
		public static UIManage Instance
		{
			get
			{
				if (_instance == null)
					_instance = new UIManage();
				return _instance;
			}
		}

		public BaseUIView SkillView;


		public void ToSkillView()
		{
			SkillView.gameObject.SetActive(true);
			SkillView.OpenView();
		}
	}
}
