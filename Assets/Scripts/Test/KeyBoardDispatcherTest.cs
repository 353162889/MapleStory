using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class KeyBoardDispatcherTest : MonoBehaviour
{
    private void Start()
    {
        KeyBoardDispatcher.CreateInstance(this.gameObject);
        KeyBoardDispatcher.Instance.OnKeyDown += OnKeyDown;
        KeyBoardDispatcher.Instance.OnKeyUp += OnKeyUp;
        KeyBoardDispatcher.Instance.RegisterRelationKey(KeyCode.A);
    }

    private void OnKeyUp(KeyCode obj)
    {
        Debug.Log("KeyUp:"+obj);
    }

    private void OnKeyDown(KeyCode obj)
    {
        Debug.Log("KeyDown:"+obj);
    }
}
