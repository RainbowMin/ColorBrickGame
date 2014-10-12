using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour 
{
	private UIWidget _widget;

	void Awake()
	{
		_widget = this.gameObject.GetComponent<UIWidget> ();
	}

	// Use this for initialization
	void Start () 
	{
		_widget.width =  ScreenHelper._instance.width;
		_widget.height = ScreenHelper._instance.height;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
