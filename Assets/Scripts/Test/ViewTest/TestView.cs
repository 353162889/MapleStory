using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;
using UnityEngine;
using UnityEngine.UI;

public class TestView : BaseView
{
    private Button _btn;
    public TestView(GameObject go, BaseViewController viewController) : base(go, viewController)
    {
    }

    public override List<BaseView> BuildSubView()
    {
        Debug.Log("[TestView]BuildSubView");
        return new List<BaseView> {
            new TestSubView(MainGO,this.viewController)
        };
    }

    public override void InitUI()
    {
        Debug.Log("[TestView]InitUI");

        _btn = MainGO.transform.FindChild("Button").GetComponent<Button>();
        base.InitUI();
    }

    public override void DestroyUI()
    {
        Debug.Log("[TestView]DestroyUI");
        _btn = null;
        base.DestroyUI();
    }

    public override void BindEvent()
    {
        Debug.Log("[TestView]BindEvent");
        _btn.onClick.AddListener(OnClick);
        base.BindEvent();
    }

    private void OnClick()
    {
        this.viewController.Close();
        Debug.Log("点击按钮");
    }

    public override void UnbindEvent()
    {
        Debug.Log("[TestView]UnbindEvent");
        base.UnbindEvent();
    }

    public override void OnEnter(params object[] param)
    {
        Debug.Log("[TestView]OnEnter:"+param.Length);
        base.OnEnter(param);
    }

    public override void OnEnterFinish()
    {
        Debug.Log("[TestView]OnEnterFinish");
        base.OnEnterFinish();
    }

    public override void OnExit()
    {
        Debug.Log("[TestView]OnExit");
        base.OnExit();
    }

    public override void OnExitFinish()
    {
        Debug.Log("[TestView]OnExitFinish");
        base.OnExitFinish();
    }
}
