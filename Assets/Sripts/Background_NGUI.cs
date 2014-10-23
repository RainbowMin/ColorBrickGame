using UnityEngine;
using System.Collections;

public class Background_NGUI : MonoBehaviour 
{
	private UIWidget _widget;
	public bool IsUpdateBGSize = true;
	public float UpdateInterval = 0.5f;
	private float timer;

	void Awake()
	{
		_widget = this.gameObject.GetComponent<UIWidget> ();
		timer = UpdateInterval;
	}

	// Use this for initialization
	void Start () 
	{
		SetBgSize ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (IsUpdateBGSize)
		{
			timer -= Time.deltaTime;
			if (timer <= 0) 
			{
				CheckBgSize();
				timer = UpdateInterval;
			}
		}
	}

	void SetBgSize()
	{
		_widget.width = Screen.width;
		_widget.height = Screen.height;
	}

	void CheckBgSize()
	{
		if(Screen.width != _widget.width || Screen.height != _widget.height)
		{
			SetBgSize();
		}
	}
}
