using UnityEngine;
using System.Collections;

public class ScreenSetting : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Awake()
	{
		Resolution r = Screen.currentResolution;
		//Screen.fullScreen = true;

		Screen.SetResolution (640, 480, true);
	}
}

