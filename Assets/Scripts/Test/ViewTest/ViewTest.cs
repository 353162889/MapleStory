using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ViewTest : MonoBehaviour
{
    private TestViewControlller viewController;
    private void Start()
    {
        ResourceManager.CreateInstance(this.gameObject);
        UpdateScheduler.CreateInstance(this.gameObject);
       
        //MultiResourceLoader _resLoader = new MultiResourceLoader();
        //_resLoader.LoadList(new List<string> { "Test/TestView.prefab" }, OnComplete, null, ResourceType.DirectObject);
    }

    private void OnComplete(MultiResourceLoader loader)
    {
        Debug.Log("complete");
    }

    private void ViewController_OnFinishDestroy(Framework.BaseViewController obj)
    {
        viewController.OnFinishDestroy -= ViewController_OnFinishDestroy;
        viewController = null;
    }

    private void OnGUI()
    {
        if(GUI.Button(new Rect(0,0,100,50), "打开"))
        {
            if(viewController == null)
            {
                viewController = new TestViewControlller(5);
                viewController.OnFinishDestroy += ViewController_OnFinishDestroy;
            }
            viewController.Open(1,1);
        }
        if (GUI.Button(new Rect(100, 0, 100, 50), "关闭"))
        {
            if (viewController != null)
            {
                viewController.Close();
            }
        }
    }
}
