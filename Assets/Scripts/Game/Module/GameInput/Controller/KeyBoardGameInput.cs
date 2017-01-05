using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;
using UnityEngine;

namespace Game
{
    public class KeyBoardGameInput : GameInputBase
    {
        private Dictionary<KeyCode, VirtualKey> mapKey;
        private Dictionary<KeyCode, Vector2> mapMove;
        private List<KeyCode> ListMoveKey;
        public override void Init()
        {
            base.Init();
            mapMove = new Dictionary<KeyCode, Vector2>();
            ListMoveKey = new List<KeyCode>();
            mapMove.Add(KeyCode.A, new Vector2(-1f,0));
            mapMove.Add(KeyCode.S, new Vector2(0, -1f));
            mapMove.Add(KeyCode.D, new Vector2(1f, 0));
            mapMove.Add(KeyCode.W, new Vector2(0, 1f));

            mapKey = new Dictionary<KeyCode, VirtualKey>();
            mapKey.Add(KeyCode.K, VirtualKey.KeyJump);

            KeyBoardDispatcher.Instance.OnKeyDown += OnKeyDown;
            KeyBoardDispatcher.Instance.OnKeyUp += OnKeyUp;

            foreach (var item in mapMove)
            {
                KeyBoardDispatcher.Instance.RegisterRelationKey(item.Key);
            }
            foreach (var item in mapKey)
            {
                KeyBoardDispatcher.Instance.RegisterRelationKey(item.Key);
            }
        }

        private void OnDirectionChange()
        {
            Vector2 v = Vector2.zero;
            for (int i = 0; i < ListMoveKey.Count; i++)
            {
                Vector2 vec = mapMove[ListMoveKey[i]];
                v.x = v.x + vec.x;
                v.y = v.y + vec.y;
            }
            v.Normalize();
            this.OnMove(v);
        }

        private void OnKeyDown(KeyCode key)
        {
            if(mapMove.ContainsKey(key))
            {
                if(!ListMoveKey.Contains(key))
                {
                    ListMoveKey.Add(key);
                    OnDirectionChange();
                }
            }
            else
            {
                VirtualKey virtualKey = mapKey[key];
                this.OnClick(virtualKey);
            }
        }

        private void OnKeyUp(KeyCode key)
        {
            if (mapMove.ContainsKey(key))
            {
                if (ListMoveKey.Contains(key))
                {
                    ListMoveKey.Remove(key);
                    OnDirectionChange();
                }
            }
         
        }
    }

    

}
