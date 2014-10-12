using UnityEngine;
using System.Collections;

public class ScreenHelper : MonoBehaviour 
{
	public static ScreenHelper _instance;
	public  int height, width;

	void Awake()
	{
		_instance = this;
		_instance.SetScreenWidthAndHeight ();
	}

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void  SetScreenWidthAndHeight()
	{
		UIRoot root = GameObject.FindObjectOfType<UIRoot>();
		if (root != null) 
		{
			float s = (float)root.activeHeight / Screen.height;
			height =  Mathf.CeilToInt(Screen.height * s);
			width = Mathf.CeilToInt(Screen.width * s);
//			Debug.Log("height = " + height);
//			Debug.Log("width = " + width);
		}
	}
}
