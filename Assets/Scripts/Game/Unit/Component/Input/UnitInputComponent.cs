using System;
using System.Collections.Generic;

namespace Game
{
	public class UnitInputComponent : UnitComponentBase
	{
		public override UnitComponentType ComponentType {
			get {
				return UnitComponentType.Input;
			}
		}

		private Dictionary<UnitInputType,List<UnitInputParam>> _mapCacheInput;

		public UnitInputComponent(UnitBase unitBase) : base(unitBase)
		{
		}

		public override void Init ()
		{
			_mapCacheInput = new Dictionary<UnitInputType, List<UnitInputParam>> ();
		}

		public void AcceptInput(UnitInputParam param)
		{
			if (_mapCacheInput != null)
			{
				List<UnitInputParam> list;
				_mapCacheInput.TryGetValue (param.InputType, out list);
				if (list == null)
				{
					list = new List<UnitInputParam> ();
					_mapCacheInput.Add (param.InputType, list);
				}
				for (int i = list.Count - 1; i >= 0; i--)
				{
					UnitInputParam p = list[i];
					if (param.Replaced (p))
					{
						list.RemoveAt (i);
						UnitInputParamPool.Instance.SaveObject (p);
						break;
					}
				}
				list.Add (param);
				this._unit.DispatchEvent (UnitInputEvent.OnAcceptInput, param);
			}
		}

		public List<UnitInputParam> GetCacheInput(UnitInputType type)
		{
			List<UnitInputParam> value;
			_mapCacheInput.TryGetValue (type, out value);
			return value;
		}

		public override void Dispose ()
		{
			if (_mapCacheInput != null)
			{
				foreach (var item in _mapCacheInput)
				{
					if (item.Value != null)
					{
						for (int i = 0; i < item.Value.Count; i++)
						{
							UnitInputParamPool.Instance.SaveObject (item.Value [i]);
						}
					}
				}
				_mapCacheInput.Clear ();
				_mapCacheInput = null;
			}
			base.Dispose ();
		}
	}
}

