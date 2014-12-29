using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {

	private SpriteRenderer _SpriteRenderer;
	public float UpdateInterval = 0.5f;
	private float timer;
	public bool IsUpdateBGSize = true;

	void Awake()
	{
		_SpriteRenderer = GetComponent<SpriteRenderer>();
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
		Rect OldRect = _SpriteRenderer.sprite.rect;
		float WidthScale = Camera.main.aspect * Camera.main.orthographicSize / (OldRect.width / 200.0f);
		float HeightScale = Camera.main.orthographicSize / (OldRect.height / 200.0f);
		_SpriteRenderer.gameObject.transform.localScale = new Vector3 (WidthScale, HeightScale, 1);


		Debug.Log ("Screen.width=" + Screen.width + " Screen.height=" + Screen.height + " SpriteWidth=" + OldRect.width + " SpriteHeight=" + OldRect.height);
		Debug.Log ("WidthScale =" + WidthScale + " HeightScale=" + HeightScale);
		Debug.Log ("Camera.main.aspect=" + Camera.main.aspect);
		//_SpriteRenderer.sprite.rect = new Rect (OldRect.x, OldRect.y, Screen.width, Screen.height);
	}

	void CheckBgSize()
	{
		if(Screen.width != _SpriteRenderer.sprite.rect.width || Screen.height != _SpriteRenderer.sprite.rect.height)
		{
			SetBgSize();
		}
	}
}
