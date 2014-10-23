using UnityEngine;
using System.Collections;

public class StartButton : MonoBehaviour 
{
	private GameObject _UI_GameScene;
	public TweenPosition _Title_TweenPos;
	public TweenPosition  _GameScene_TweenPos;
	private int _height;
	private bool _IsPressStart = false;

	void Awake()
	{
		_Title_TweenPos = GameObject.FindGameObjectWithTag ("UI_TitleMenu").GetComponent<TweenPosition>();
		//_GameScene_TweenPos = GameObject.FindGameObjectWithTag ("UI_GameScene").GetComponent<TweenPosition>();
	
		_Title_TweenPos.enabled = false;
		//_GameScene_TweenPos.enabled = false;
	}

	// Use this for initialization
	void Start () 
	{
		//UI_GameScene.SetActive (false);
		_height = NGUIScreenHelper._instance.height;

		_Title_TweenPos.to = new Vector3 (0, _height, 0);
		//_GameScene_TweenPos.from = new Vector3 (0, _height * -1, 0);
		//_GameScene_TweenPos.gameObject.transform.position = new Vector3(0, _height * -1, 0);

		//_GameScene_TweenPos.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PressStart()
	{
		//_IsPressStart = true;

		//_GameScene_TweenPos.gameObject.SetActive (true);

		//_Title_TweenPos.PlayForward ();
		//_GameScene_TweenPos.PlayForward ();

		Application.LoadLevel ("GamePlay");
	}

	private void FinishMoveAnimation()
	{
		if(_IsPressStart)
		{
			_Title_TweenPos.gameObject.SetActive (false);
			_IsPressStart = false;
		}
	}
}
