using UnityEngine;
using System.Collections;

public class CursorTest : MonoBehaviour {
    public Texture2D Up;
    public Texture2D Down;
	// Use this for initialization
	void Start () {
        Cursor.SetCursor(Up, Vector2.zero, CursorMode.Auto);
    }
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetMouseButtonDown(0))
        {
            Cursor.SetCursor(Down, Vector2.zero, CursorMode.Auto);
        }
        else if(Input.GetMouseButtonUp(0))
        {
            Cursor.SetCursor(Up, Vector2.zero, CursorMode.Auto);
        }
	}

   
}
