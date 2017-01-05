using System.Collections.Generic;
using Framework;
using UnityEngine;

public class TestSubView : BaseView
{
    public TestSubView(GameObject go, BaseViewController viewController) : base(go, viewController)
    {
    }

    public override List<BaseView> BuildSubView()
    {
        Debug.Log("[TestSubView]BuildSubView");
        return null;
    }

    public override void InitUI()
    {
        Debug.Log("[TestSubView]InitUI");
    }

    public override void DestroyUI()
    {
        Debug.Log("[TestSubView]DestroyUI");
    }

    public override void BindEvent()
    {
        Debug.Log("[TestSubView]BindEvent");
    }

    public override void UnbindEvent()
    {
        Debug.Log("[TestSubView]UnbindEvent");
    }

    public override void OnEnter(params object[] param)
    {
        Debug.Log("[TestSubView]OnEnter:"+param.Length);
    }

    public override void OnEnterFinish()
    {
        Debug.Log("[TestSubView]OnEnterFinish");
    }

    public override void OnExit()
    {
        Debug.Log("[TestSubView]OnExit");
    }

    public override void OnExitFinish()
    {
        Debug.Log("[TestSubView]OnExitFinish");
    }
}
