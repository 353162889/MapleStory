using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 管理unit的属性数据的逻辑
    /// </summary>
    public class UnitPropertyComponent : UnitComponentBase
    {
        public string ActionName { get { return GetProperty<string>(UnitProperty.ActionName);} }
        public string FaceName { get { return GetProperty<string>(UnitProperty.FaceName); } }
        public Vector2 Speed { get { return GetProperty<Vector2>(UnitProperty.Speed); } }
        public int PropState { get { return GetProperty<int>(UnitProperty.State); } }
		public UnitFace UnitFace{get{return GetProperty<UnitFace> (UnitProperty.UnitFace);}}

        public override UnitComponentType ComponentType
        {
            get
            {
                return UnitComponentType.Property;
            }
        }

        private UnitPropertyContainer _unitPropertyContainer;

        public UnitPropertyComponent(UnitBase unitBase) : base(unitBase)
        {
            _unitPropertyContainer = new UnitPropertyContainer();
        }

        public void InitProperty(UnitProperty property,object obj)
        {
            _unitPropertyContainer.AddProperty(property, obj);
        }

        public void UpdateProperty(UnitProperty property,object obj)
        {
            _unitPropertyContainer.UpdateProperty(property, obj);
        }

        public object GetProperty(UnitProperty property)
        {
            return _unitPropertyContainer.GetProperty(property);
        }

        public T GetProperty<T>(UnitProperty property)
        {
            object obj = GetProperty(property);
            if(obj == null)
            {
                return default(T);
            }
            return (T)obj;
        }
    }
}
