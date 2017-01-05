using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    /// <summary>
    /// unit的所有属性的存储
    /// </summary>
    public class UnitPropertyContainer
    {
        private Dictionary<UnitProperty, object> _mapData;
        public UnitPropertyContainer()
        {
            _mapData = new Dictionary<UnitProperty, object>();
        }

        public Object GetProperty(UnitProperty property)
        {
            object obj;
            _mapData.TryGetValue(property, out obj);
            return obj;
        }

        public void AddProperty(UnitProperty property,object obj)
        {
            _mapData.Add(property, obj);
        }

        public void UpdateProperty(UnitProperty property,object obj)
        {
            _mapData[property] = obj;
        }
    }
}
