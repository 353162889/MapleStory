using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Game
{
    public abstract class GameInputBase
    {
        public event Action<Vector2> OnInputMove;
        public event Action<VirtualKey> OnKeyDown;
		public event Action<VirtualKey> OnKeyUp;
        private List<GameInputForbidKey> _listForbid;

        public virtual void Init()
        {
            _listForbid = new List<GameInputForbidKey>();
        }

        protected void OnMove(Vector2 direction)
        {
            if(OnInputMove != null && _listForbid.Count == 0)
            {
                OnInputMove.Invoke(direction);
            }
        }

        protected void OnClickKeyDown(VirtualKey key)
        {
            if(OnKeyDown != null && _listForbid.Count == 0)
            {
                OnKeyDown.Invoke(key);
            }
        }

		protected void OnClickKeyUp(VirtualKey key)
		{
			if(OnKeyUp != null && _listForbid.Count == 0)
			{
				OnKeyUp.Invoke(key);
			}
		}

        public void ForbidInput(GameInputForbidKey key)
        {
            if(!_listForbid.Contains(key))
            {
                _listForbid.Add(key);
            }
        }

        public void ResumeInput(GameInputForbidKey key)
        {
            _listForbid.Remove(key);
        }

        public void ClearForbid()
        {
            _listForbid.Clear();
        }
    }

    public enum GameInputForbidKey
    {
        NotInGame = 1,
        SceneLoad = 2
    }
}
