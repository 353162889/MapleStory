using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Framework;

namespace Game
{
    [CreateAssetMenu(menuName = "创建动作资源")]
    public class ActionResScriptObj : ScriptableObject
    {
        public SerializableDictionaryActionResInfo actionInfos;
    }

    [Serializable]
    public class ActionResInfo
    {
        public FrameResInfo[] frameInfos;
    }

    [Serializable]
    public class FrameResInfo
    {
        public float delay;
        public SerializableDictionarySpriteResInfo frameResInfos;
    }

    [Serializable]
    public class SerializableDictionaryActionResInfo : AbstractSerializableDictionary<string, ActionResInfo>
    {
        public SerializableDictionaryActionResInfo()
            : base()
        {
        }

        public SerializableDictionaryActionResInfo(int capacity)
            : base(capacity)
        {
        }
    }

    [Serializable]
    public class SerializableDictionarySpriteResInfo : AbstractSerializableDictionary<string, SpriteResInfo>
    {
        public SerializableDictionarySpriteResInfo()
            : base()
        {
        }

        public SerializableDictionarySpriteResInfo(int capacity)
            : base(capacity)
        {
        }
    }

    [Serializable]
    public class SpriteResInfo
    {
        public Sprite sprite;
        public Vector2 origin;
		public Vector2 offset;
        public string z;
        public string group;
        public SerializableDictionarySpriteMap maps;
    }

    [Serializable]
    public class SerializableDictionarySpriteMap : AbstractSerializableDictionary<string, Vector2>
    {
        public SerializableDictionarySpriteMap() : base()
        {

        }

        public SerializableDictionarySpriteMap(int capacity) : base(capacity)
        {
        }
    }
}