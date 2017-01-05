using UnityEngine;
using System.Collections;
using Framework;
using System.Collections.Generic;

public class ResourceTest : MonoBehaviour {
	MultiResourceLoader multiLoader;

	// Use this for initialization
	void Start () {
		ResourceManager.CreateInstance (this.gameObject);
        //ResourceManager.Instance.GetResource("Test/ResourceTest.txt", OnSucc, OnFail, ResourceType.Text);
        ResourceManager.Instance.GetResource("Test/ResourceTest.txt", OnSucc, OnFail);
        //TextAsset asset = Resources.Load<TextAsset>("Test/ResourceTest");
        //Debug.Log("asset:"+asset.text);
        //      multiLoader = new MultiResourceLoader ();
        //multiLoader.LoadList (new List<string>{
        //	"ResourceTest.txt","ResourceTest1.txt"
        //      },OnComplete,OnProgress);
        //multiLoader.TryGetRes ("VPN", OnTryGetRes);

    }

	private void OnTryGetRes(Resource res)
	{
		Debug.Log ("OnTryGetRes:"+res.path);
	}

	private void OnComplete(MultiResourceLoader loader)
	{
		Debug.Log ("OnComplete");
	}

	private void OnProgress(Resource res)
	{
		Debug.Log ("[OnProgress] " + res.path);
	}

	private void OnSucc(Resource res)
	{
		
		Debug.Log ("[OnSucc] " + res.path + ",content:" + res.GetText());
	}

	private void OnFail(Resource res)
	{
		Debug.Log ("[OnFail] " + res.path + ",Error:"+res.errorTxt);
	}
}
