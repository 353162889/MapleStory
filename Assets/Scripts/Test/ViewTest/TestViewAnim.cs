using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TestViewAnim : BaseViewAnim
{
    protected override void OnPlay()
    {
        Debug.Log("PlayAnim");
        UpdateScheduler.Instance.AddScheduler(OnDelay, 5f, 1);
    }

    private void OnDelay(float delay)
    {
        Debug.Log("FinishPlayAnim");
        this.FinishPlay();
    }

    protected override void OnStop()
    {
        UpdateScheduler.Instance.RemoveScheduler(OnDelay);
    }
}
