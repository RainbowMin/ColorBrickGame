using UnityEngine;
using System.Collections;

public class GlobalScale : MonoBehaviour 
{
	public static GlobalScale _instance;

	public float WidthScale;
	public float HeightScale;

	public float FixedWidth = 500;
	public float FixedHeight = 850;

	void Awake()
	{
		if(_instance == null)
		{
			_instance = new GlobalScale();
		}
	}

	void Start () 
	{
		_instance.SetScale ();
	}
	
	void Update () 
	{
	
	}

	void SetScale()
	{
		WidthScale = Camera.main.aspect * Camera.main.orthographicSize / (FixedWidth / 200.0f);
		HeightScale = Camera.main.orthographicSize / (FixedHeight / 200.0f);
	}
}
