using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
	public delegate void EventDispatchHandler(int eventID,params object[] param);
	public class EventDispatcher
	{
		private Dictionary<int,List<EventDispatchHandler>> _map;
		private Queue<EventEntity> _delayEvents;
		private int _dispatchId;
        private bool _destroy;

		public EventDispatcher()
		{
			_dispatchId = -1;
            _destroy = false;
			_map = new Dictionary<int, List<EventDispatchHandler>> ();
			_delayEvents = new Queue<EventEntity> ();
			ObjectPool<EventEntity>.Instance.Init (20);
		}

		public void AddListener(int eventID,EventDispatchHandler handler)
		{
			if (IsDispatching ())
			{
				EventEntity entity = ObjectPool<EventEntity>.Instance.GetObject ();
				entity.Init (eventID,handler,EventOperate.AddListener);
				_delayEvents.Enqueue (entity);
			}
			else
			{
				List<EventDispatchHandler> list;
				_map.TryGetValue (eventID, out list);
				if (list == null)
				{
					list = new List<EventDispatchHandler> ();
					_map.Add (eventID, list);
				}
				if (!list.Contains (handler))
				{
					list.Add (handler);
				}
			}
		}

		public void RemoveListener(int eventID,EventDispatchHandler handler)
		{
			if (IsDispatching ())
			{
				EventEntity entity = ObjectPool<EventEntity>.Instance.GetObject ();
				entity.Init (eventID,handler,EventOperate.RemoveListener);
				_delayEvents.Enqueue (entity);
			}
			else
			{
				List<EventDispatchHandler> list;
				_map.TryGetValue (eventID, out list);
				if (list != null)
				{
					list.Remove (handler);
				}
			}
		}

		public bool HasListener(int eventID,EventDispatchHandler handler)
		{
			List<EventDispatchHandler> list;
			_map.TryGetValue (eventID, out list);
			if (list == null)
				return false;
			return list.Contains (handler);
		}

		public void DispatchEvent(int eventID,params object[] param)
		{
			if (IsDispatching ())
			{
				CLog.LogError ("can not dispatch when current is dispatching!curDispatchId=" + _dispatchId + ",willDispatchId="+eventID);
				return;
			}
			List<EventDispatchHandler> handlers;
			_map.TryGetValue (eventID, out handlers);
			_dispatchId = eventID;
			if (handlers != null)
			{
				for (int i = 0; i < handlers.Count; i++)
				{
                    if (!_destroy)
                    {
                        handlers[i].Invoke(eventID, param);
                    }
                    else
                    {
                        break;
                    }
				}
			}
			_dispatchId = -1;
			while (_delayEvents.Count > 0)
			{
				EventEntity entity = _delayEvents.Dequeue ();
				if (entity.operate == EventOperate.AddListener)
				{
					AddListener (entity.eventID, entity.handler);
				}
				else if(entity.operate == EventOperate.RemoveListener)
				{
					RemoveListener (entity.eventID, entity.handler);
				}
				ObjectPool<EventEntity>.Instance.SaveObject (entity);
			}
		}

        public void Dispose()
        {
            _destroy = true;
            if (_map != null)
            {
                _map.Clear();
                _map = null;
            }
            if(_delayEvents != null)
            {
                _delayEvents.Clear();
                _delayEvents = null;
            }
          
        }

		private bool IsDispatching()
		{
			return _dispatchId != -1;
		}
	}
}

