using System.Collections.Generic;

namespace Fundamental
{
	public class NotifyManager : MonoSingleton<NotifyManager>
	{
		private const int NotifyTiers = 5;
		List<List<INotifyHandler>> _invokeList, _listenerList;

		protected override void Init()
		{
			_invokeList = new List<List<INotifyHandler>>(NotifyTiers);
			for (int i = 0; i < NotifyTiers; i++)
			{
				_invokeList[i] = new List<INotifyHandler>();
			}

			_listenerList = new List<List<INotifyHandler>>(NotifyTiers);
			for (int i = 0; i < NotifyTiers; i++)
			{
				_listenerList[i] = new List<INotifyHandler>();
			}
		}

		protected override void UnInit()
		{

		}

		public void InvokeNotify(Notify type, object body)
		{
		}
	}

	public interface INotifyHandler
	{
		void OnNotify(Notify ntf);
	}

	public class Notify
	{
		public int type;
		public object body;

		public Notify(int type, object body = null)
		{
			this.type = type;
			this.body = body;
		}
	}
}