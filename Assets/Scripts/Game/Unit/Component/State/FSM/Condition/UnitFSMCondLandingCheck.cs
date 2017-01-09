using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;
using UnityEngine;

namespace Game
{
    public class UnitFSMCondLandingCheck : UnitFSMConditionBase
    {
        //人物检测高度数据时的偏移高度（人物中心点的高度加上当前偏移高度才是检测高度）
        private static float OffsetOnGround = 0.3f;
        private static float OffsetUnderGround = -0.09f;//跟地图的每个碰撞格子的高度有关
        public UnitFSMCondLandingCheck(UnitBase unit) : base(unit)
        {
        }

        public override bool IsInCondition(FiniteState curState, string toStateName)
        {
            Vector2 lastPos = _unit.PropComponent.LastPos;
            float lastHeight = lastPos.y + OffsetUnderGround;
            bool isLastWalkable = SceneModel.Instance.IsWalkable(lastPos.x, lastHeight);
            //上一个位置是可行走区域，才算着落
            if(!isLastWalkable)return false;
            Vector3 curPos = _unit.transform.position;
            float curHeight = curPos.y + OffsetUnderGround;
            bool isCurWalkable = SceneModel.Instance.IsWalkable(curPos.x, curHeight);
            //当前位置是不可行走区域，才算着落
            if(isCurWalkable)return false;
            //上一个位置与当前位置的连线和当前高度的一条线如果有交集，那么就是着陆了
            float height = SceneModel.Instance.GetCurMapHeight(curPos.x, curPos.y + OffsetOnGround);
            if (lastHeight > height && curHeight <= height) return true;
            return false;
        }
    }
}
