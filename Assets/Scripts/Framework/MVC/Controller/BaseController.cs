using System;

namespace Framework
{
	public class BaseController<T> : Singleton<T> where T : BaseController<T>,new()
	{
		public EventDispatcher EventDispatcher{ get; private set;}

		public override void Init ()
		{
			this.EventDispatcher = new EventDispatcher ();
		}

		public virtual void InitController()
		{
		}

		public void AddListener(int eventID,EventDispatchHandler handler)
		{
			EventDispatcher.AddListener (eventID, handler);
		}

		public void RemoveListener(int eventID,EventDispatchHandler handler)
		{
			EventDispatcher.RemoveListener (eventID, handler);
		}

		public void DispatchEvent(int eventID,params object[] param)
		{
			EventDispatcher.DispatchEvent (eventID,param);
		}
	}
}

