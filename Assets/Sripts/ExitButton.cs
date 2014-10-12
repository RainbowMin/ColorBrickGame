using UnityEngine;
using System.Collections;

public class ExitButton : MonoBehaviour 
{

	private GameObject _UI_GameScene;
	public TweenPosition _Title_TweenPos;
	public TweenPosition  _GameScene_TweenPos;
	private bool IsPressExit = false;
	
	void Awake()
	{
		//_UI_GameScene = GameObject.FindGameObjectWithTag ("UI_GameScene");
		_Title_TweenPos = GameObject.FindGameObjectWithTag ("UI_TitleMenu").GetComponent<TweenPosition>();
		_GameScene_TweenPos = GameObject.FindGameObjectWithTag ("UI_GameScene").GetComponent<TweenPosition>();
		
		_Title_TweenPos.enabled = false;
		_GameScene_TweenPos.enabled = false;
	}
	
	// Use this for initialization
	void Start () 
	{
		//UI_GameScene.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void PressExit()
	{
		IsPressExit = true;
		_Title_TweenPos.gameObject.SetActive (true);
		_Title_TweenPos.PlayReverse ();
		_GameScene_TweenPos.PlayReverse ();
	}

	private void FinishMoveAnimation()
	{
		if(IsPressExit)
		{
			_GameScene_TweenPos.gameObject.SetActive (false);
			IsPressExit = false;
		}
	}
}
