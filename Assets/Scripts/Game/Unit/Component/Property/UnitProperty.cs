using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Game
{
    public class UnitProperty : IEqualityComparer<UnitProperty>
    {
        private static Dictionary<string, UnitProperty> _mapStringProperty = new Dictionary<string, UnitProperty>();
        private static Dictionary<int, UnitProperty> _mapIntProperty = new Dictionary<int, UnitProperty>();

        public static UnitProperty ActionName = new UnitProperty(1, "actionname");
        public static UnitProperty FaceName = new UnitProperty(2, "facename");
        public static UnitProperty Speed = new UnitProperty(3, "speed");
        public static UnitProperty State = new UnitProperty(4, "state");
        public static UnitProperty UnitFace = new UnitProperty(5, "unitface");

        public static UnitProperty GetUnitProperty(string name)
        {
            UnitProperty property;
            _mapStringProperty.TryGetValue(name, out property);
            return property;
        }

        public static UnitProperty GetUnitPorperty(int index)
        {
            UnitProperty property;
            _mapIntProperty.TryGetValue(index, out property);
            return property;
        }

        public static Dictionary<int, UnitProperty> GetAllProperty()
        {
            return _mapIntProperty;
        }

        public int Index { get; private set; }
        public string[] Names { get; private set; }
        public UnitProperty(int index,params string[] name)
        {
            this.Index = index;
            this.Names = name;
            foreach (var item in name)
            {
                _mapStringProperty.Add(item, this);
            }
            _mapIntProperty.Add(Index, this);
        }

        public bool Equals(UnitProperty x, UnitProperty y)
        {
            return x.Index == y.Index;
        }

        public int GetHashCode(UnitProperty obj)
        {
            return obj.Index;
        }
    }
}
