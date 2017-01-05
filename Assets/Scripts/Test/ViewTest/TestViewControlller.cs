using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;
using UnityEngine;

public class TestViewControlller : BaseViewController
{
    public TestViewControlller(int autoDestroyTime) : base(autoDestroyTime)
    {
    }

    protected override GameObject Parent
    {
        get
        {
            GameObject container = GameObject.Find("UI");
            return container;
        }
    }

    public override List<string> DependResources()
    {
        return new List<string> {
            "Test/TestView.prefab"
        };
    }

    protected override List<BaseView> BuildViews()
    {
        return new List<BaseView> {
                new TestView(MainGO,this)
        };
    }

    public override List<BaseViewAnim> BuildOpenAnims()
    {
        return new List<BaseViewAnim> { new TestViewAnim() };
    }

    public override List<BaseViewAnim> BuildCloseAnims()
    {
        return new List<BaseViewAnim> { new TestViewAnim() };
    }
}
