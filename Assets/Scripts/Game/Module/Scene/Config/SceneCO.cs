using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Game
{
    public class SceneCO
    {
        public string Name { get; private set;}
        public string ResPath { get; private set; }
        public string MapPath { get; private set; }
        public Vector3 BirthPoint { get; private set; }
        public SceneCO(string name, Vector3 birthPoint,string resPath,string mapPath)
        {
            this.Name = name;
            this.ResPath = resPath;
            this.MapPath = mapPath;
            this.BirthPoint = birthPoint;
        }
    }
}
