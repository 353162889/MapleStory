using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;
using UnityEngine;

namespace Game
{
    public class UnitFSMCondRiseCheck : UnitFSMConditionBase
    {
        //人物检测高度数据时的偏移高度（人物中心点的高度加上当前偏移高度才是检测高度）
        private static float OffsetOnGround = 0.3f;
        private static float OffsetUnderGround = -0.09f;//跟地图的每个碰撞格子的高度有关
        public UnitFSMCondRiseCheck(UnitBase unit) : base(unit)
        {
        }
        //只要当前位置是可行走区域，并且当前位置与上一个位置的高度差
        public override bool IsInCondition(FiniteState curState, string toStateName)
        {
            Vector2 lastPos = _unit.PropComponent.LastPos;
            float lastHeight = lastPos.y + OffsetUnderGround;
            bool isLastWalkable = SceneModel.Instance.IsWalkable(lastPos.x, lastHeight);
            Vector3 curPos = _unit.transform.position;
            float curHeight = curPos.y + OffsetUnderGround;
            bool isCurWalkable = SceneModel.Instance.IsWalkable(curPos.x, curHeight);
            //当前位置是可行走区域，才算上升
            if (isLastWalkable && isCurWalkable) return true;
            //上一个位置与当前位置的连线和当前高度的一条线如果有交集，那么就是着陆了
            float lastGroundHeight = SceneModel.Instance.GetCurMapHeight(lastPos.x, lastPos.y + OffsetOnGround);
            float curGroundHeight = SceneModel.Instance.GetCurMapHeight(curPos.x, curPos.y + OffsetOnGround);
            if (!isLastWalkable && isCurWalkable && (lastGroundHeight - curGroundHeight) > SceneModel.Instance.SceneMapMO.MaxInterpolationHeight)
                return true;
            return false;
        }
    }
}
