using UnityEngine;
using System.Collections;

public class Grid_NGUI : MonoBehaviour 
{
	private GameObject _gameManager;
	private GameLogic _gameLogic;
	private bool IsLocked;
	private bool IsNeedAddForce;
	private DropInfo _dropInfo;
	
	public float RotateAngle = 270f;
	public float RotateTime = 2.0f;
	public int AddForceFrameTotalCount;

	private int _AddForceCurFrame;
	private UIWidget _UIWidget;	
	private Rigidbody _Rigidbody;
	
	public Rect _rect;
	public Vector2 _V2;
	public Vector3 _MousePos;
	public Bounds _Bounds;
	public Vector4 _Border;	
	
	// Use this for initialization
	void Start () 
	{
		_gameManager = GameObject.FindGameObjectWithTag("GameController");
		_gameLogic = _gameManager.GetComponent<GameLogic>();
		IsLocked = false;
		IsNeedAddForce = false;
		
		
		_AddForceCurFrame = 0;
		
		_UIWidget = GetComponent<UIWidget> ();
		_Rigidbody = (Rigidbody)GetComponent<Rigidbody>();
	}
	
	void MyOnClick()
	{
		if(IsLocked)
		{
			return;
		}
		
		IndexInfo info =  _gameLogic.GetGridIndex(this.gameObject);
		_gameManager.SendMessage("OnGridPressed", info);
	}
	
	//当鼠标在GUIElement(GUI元素)或Collider(碰撞体)上点击时调用OnMouseDown。
	void OnMouseDown()
	{	
		
		if(IsLocked)
		{
			return;
		}
		
		IndexInfo info =  _gameLogic.GetGridIndex(this.gameObject);
		Debug.Log("index x="+info.x+" y="+info.y);
		//Debug.Log("-----------------------------------");
		//Debug.Log ("OnMouseDown time=" + Time.time);
		_gameManager.SendMessage("OnGridPressed", info);
		
		
		//		_SpriteRenderer.sortingLayerName = "Sprite_Drop";
		//		rigidbody2D.AddForce(new Vector2(0,15));
		//		rigidbody2D.gravityScale = 1.0f;//启用重力
		//		rigidbody2D.isKinematic = false;
		//		//把碰撞体隐藏掉以防色块之间相互碰撞
		//		BoxCollider2D boxCollider = gameObject.GetComponent<BoxCollider2D>();
		//		boxCollider.size = new Vector2 (0, 0);
	}
	
	public IEnumerator DropToBottom(IndexInfo index)
	{
		//Debug.Log ("GriDropToBottom time=" + Time.time);
		
		//edit depth
		
		IsNeedAddForce = true;
		
		
		//把碰撞体隐藏掉以防色块之间相互碰撞
		BoxCollider2D boxCollider = gameObject.GetComponent<BoxCollider2D>();
		boxCollider.size = new Vector2 (0, 0);
		
		float forceDirectionX;
		if(index.x <= 1)
		{
			forceDirectionX = -1.0f;
		}
		else
		{
			forceDirectionX = 1.0f;
		}
		//_Rigidbody2D.gravityScale = 1.0f;
		//_Rigidbody2D.isKinematic = false;
		//_Rigidbody2D.AddForce(new Vector2(2 * forceDirectionX,15));
		
		iTween.RotateAdd(gameObject, iTween.Hash("z", RotateAngle, "time", RotateTime));
		
		//_dropInfo = new DropInfo (dropInfo.BottomY, dropInfo.Speed);
		
		yield return new WaitForSeconds (3.0f);		
		Destroy (this.gameObject);
	}
	
	void Lock()
	{
		//Debug.Log("lock");
		IsLocked = true;
	}
	
	void Unlock()
	{
		//Debug.Log("unlock");
		IsLocked = false;
	}
}
