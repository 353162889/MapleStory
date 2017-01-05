using System;

namespace Framework
{
	public class EventEntity : IPoolable
	{
		public int eventID{get;private set;}
		public EventDispatchHandler handler{get;private set;}
		public EventOperate operate{ get; private set;}

		public void Init(int eventID,EventDispatchHandler handler,EventOperate operate)
		{
			this.eventID = eventID;
			this.handler = handler;
			this.operate = operate;
		}

		public void Reset()
		{
			this.eventID = 0;
			this.handler = null;
			this.operate = EventOperate.AddListener;
		}
	}

	public enum EventOperate
	{
		AddListener,
		RemoveListener
	}
}

