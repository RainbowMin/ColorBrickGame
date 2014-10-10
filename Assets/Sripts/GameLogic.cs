using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct IndexInfo
{
	public int x;
	public int y;

	public IndexInfo(int X, int Y)
	{
		x = X;
		y = Y;
	}
}

public struct IndexAndColor
{
	public int x;
	public int y;
	public Color color;

	IndexAndColor(int X, int Y, Color color1)
	{
		x = X;
		y = Y;
		color = color1;
	}
}

public class GameLogic : MonoBehaviour 
{
	public Transform BottomPoint;
	//public Transform BottomPreparePoint;
	//public Transform TopGameOverPoint;
	public Transform BottomDestinationPoint;
//	public int BottomX = -10;
//	public int BottomY = -10;
	public List<GameObject> GridPrefabList; 

	public float fCleanGridInterval = 0.3f;
	public float fWaitForDropByGravity = 0.15f;
	public float fDropByGravityTime = 0.15f;
	public float fAddOnLevelTime = 0.2f;
	public float DropByGravitySpeed = 3.3f;

	public GameObject AudioManager;
	public GameObject CameraObject;


	public float GridHeight;
	public float GridWidth;

	private float curLockTime;
	private float WaitForDropByGravity = 0.0f;

	private static int Width = 4;
	private static int Height = 13;
	private static int InitWidth = 4;
	private static int InitHeight = 4;
	//private Grid[] GridList  = new Grid[Width*Height];
	private Grid[,] GridList = new Grid[Width, Height];

	private bool IsGameOver = false;

	public GameObject m_UI_GameOver;

	public struct Grid
	{
		public GameObject _grid;
		public bool IsEmpty;
		public bool IsNeedToBeDeleted;
		public float fHeightChanged;
	};

	// Use this for initialization
	void Start () 
	{

		SpriteRenderer spriteRenderer = (SpriteRenderer)GridPrefabList[0].GetComponent<SpriteRenderer>();
		GridHeight = spriteRenderer.sprite.rect.height / 100;
		GridWidth = spriteRenderer.sprite.rect.width / 100;

		for(int i = 0; i < Width; i++)
		{
			for(int j = 0; j < Height; j++)
			{
				GridList[i,j].IsEmpty = true;
				GridList[i,j]._grid = null;
				GridList[i,j].IsNeedToBeDeleted = false;
			}
		}
		InitGrid();

		HideUI_GameOver ();
	}

	void InitGrid()
	{
		for(int i = 0; i < InitWidth; i++)
		{
			for(int j = 0; j < InitHeight; j++)
			{
				AddRandomGrid(i,j);
			}
		}
	}

	void AddRandomGrid(int X, int Y)
	{
		int PrefabIndex = Random.Range(0, GridPrefabList.Count);

		GridList[X, Y]._grid = (GameObject)GameObject.Instantiate(GridPrefabList[PrefabIndex]);
		GridList[X, Y]._grid.transform.position = new Vector3(BottomPoint.position.x + X * GridWidth, BottomPoint.position.y + Y * GridHeight - GridHeight, 0.0f);
		GridList[X, Y]._grid.transform.parent = GameObject.Find("Grids").transform;
		GridList[X, Y]._grid.name += "["+X+","+Y+"]";
		GridList[X, Y].IsEmpty = false;

		iTween.MoveTo(GridList[X,Y]._grid, iTween.Hash("position", GridList[X, Y]._grid.transform.position + new Vector3(0, GridHeight, 0), "time", fAddOnLevelTime));
	}

//	void AddGrid(int X, int Y, int PrefabIndex)
//	{
//		GridList[X, Y]._grid = (GameObject)GameObject.Instantiate(GridPrefabList[PrefabIndex]);
//		GridList[X, Y]._grid.transform.position = new Vector3(BottomX + X * GridWidth, BottomY + Y * GridHeight, 0.0f);
//		GridList[X, Y]._grid.transform.parent = GameObject.Find("Grids").transform;
//		GridList[X, Y]._grid.name += "["+X+","+Y+"]";
//		GridList[X, Y].IsEmpty = false;	
//	}

	void PrintGridNum()
	{
		//foreach(Grid grid in GridList)
		{
			//Debug.Log("grid index x="+grid._indexX+" y= "+grid._indexY + " PosX="+grid._grid.transform.position.x+" PosY="+grid._grid.transform.position.y);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public IndexInfo GetGridIndex(GameObject o)
	{
		IndexInfo info;
		info.x = info.y = -1;

		for(int i = 0; i < Width; i++)
		{
			for(int j = 0; j < Height; j++)
			{
				if(GridList[i,j].IsEmpty == false)
				{
					if(o == GridList[i,j]._grid)
					{
						
						info.x = i;
						info.y = j;
						return info;
					}
				}
			}
		}

		return info;
	}

	IEnumerator OnGridPressed(IndexInfo indexInfo)
	{
		if (IsGameOver) 
		{
			yield return null;				
		} 

		//Debug.Log ("OnGridPressed1 time=" + Time.time);

		PlaySound (AUDIO_TYPE.CLEAN);

		//Debug.Log("OnGridPressed index x="+indexInfo.x+" y="+indexInfo.y);

		//notify all the grids to lock
		GameObject[] grids = GameObject.FindGameObjectsWithTag("Grid");
		foreach (GameObject grid in grids) 
		{
			grid.SendMessage("Lock");
		}
		
		if(GridList[indexInfo.x, indexInfo.y]._grid == null || GridList[indexInfo.x, indexInfo.y].IsEmpty)
		{
			Debug.LogError("OnGridPressed error");
		}
		
		SpriteRenderer spritePressed = GridList[indexInfo.x, indexInfo.y]._grid.GetComponent<SpriteRenderer>();
		
		//1.点击的方块掉落
		//CleanOneSameColorGrid (indexInfo.x, indexInfo.y, spritePressed.color);
		CleanOneSameColorGrid (indexInfo.x, indexInfo.y, spritePressed.color);

		CleanAllSameColorGrid ();

		//Debug.Log ("DropByGravity");
		//2.点击方块上面的一起往下掉落
		DropByGravity();
		yield return new WaitForSeconds(WaitForDropByGravity+0.05f);
		
		//3.检查是否能消除
		yield return StartCoroutine(CheckCleanUp());
	
		//Debug.Log("Before AddOneLevel time=" + Time.time);
		AddOneLevel ();		

		yield return new WaitForSeconds(fAddOnLevelTime);

		GameObject[] grids2 = GameObject.FindGameObjectsWithTag("Grid");
		foreach (GameObject grid in grids2) 
		{
			grid.SendMessage("Unlock");
		}	

		//notify all the grids to unlock


		
		//Debug.Log("Finish!!!!!!!!!!!! time=" + Time.time);
	} 

	bool CleanAllSameColorGrid()
	{
		bool HasAnythingCleaned = false;

		for (int i = 0; i < Width; i++) 
		{
			for(int j = 0; j < Height; j++)
			{
				if(GridList[i,j].IsNeedToBeDeleted && GridList[i,j].IsEmpty == false)
				{
					DropClickedGrid(new IndexInfo(i,j));
					HasAnythingCleaned = true;

				}
			}
		}

		if(HasAnythingCleaned)
		{
			PlaySound (AUDIO_TYPE.CLEAN);
		}

		return HasAnythingCleaned;
	}

	void DropClickedGrid(IndexInfo index)
	{
		Grid grid = GridList[index.x, index.y];
		if(grid.IsEmpty || grid._grid == null)
		{
			Debug.LogError("ClickedGrid is empty or null");
		}
		Debug.Log ("GameLogic::DropClickedGrid time=" + Time.time);
		GridList[index.x, index.y]._grid.SendMessage("DropToBottom", index);//, new DropInfo(BottomDestinationPoint.transform.position.y, DropToBotSpeed));

		GridList[index.x, index.y].IsEmpty = true;
		GridList[index.x, index.y]._grid = null;
		GridList [index.x, index.y].IsNeedToBeDeleted = false;

	}

	void DropByGravity()
	{			
		//init
		for (int i = 0; i < Width; i++) 
		{
			for (int j = 0; j < Height; j++) 
			{	
				GridList[i, j].fHeightChanged = 0;
			}
		}
				
		for(int i = 0; i < Width; i++)
		{
			for(int j = 0; j < Height; j++)
			{				
				if(GridList[i,j].IsEmpty == true)
				{
					int EmptyHeight = 1;
					for(int k = 1; k < Height - j; k++)
					{
						if(GridList[i, j + k].IsEmpty == false)
						{
							//GridList[i, j + k]._grid.transform.position -= new Vector3(0, GridHeight * EmptyHeight, 0);
							GridList[i, j+k - EmptyHeight].fHeightChanged = -1 * GridHeight * EmptyHeight;

							GridList[i, j + k - EmptyHeight].IsEmpty = false;
							GridList[i, j + k - EmptyHeight]._grid = GridList[i, j + k]._grid;
							GridList[i, j + k].IsEmpty = true;
							GridList[i, j + k]._grid = null;
						}
						else
						{
							EmptyHeight++;
						}
					}		
					
					continue;
				}
			}
		}	

		float MaxDropTime = 0;
		//drop down animation
		for (int i = 0; i < Width; i++) 
		{
			for (int j = 0; j < Height; j++) 
			{	
				if(GridList[i,j].fHeightChanged != 0)
				{
					float time = Mathf.Abs(GridList[i,j].fHeightChanged / DropByGravitySpeed);
					if(time > MaxDropTime)
					{
						MaxDropTime = time;
					}
					iTween.MoveTo(GridList[i,j]._grid, iTween.Hash("position", GridList[i,j]._grid.transform.position + new Vector3(0, GridList[i,j].fHeightChanged, 0), "time", time));
				}
			}
		}

		WaitForDropByGravity = MaxDropTime;
	}

	IEnumerator CheckCleanUp()
	{
		//检查横排是否有3个以上连一起的，有的话把旁边一样颜色的也消除
		bool IsVerticalCleanGrid =  CleanByVertical();

		bool IsHorizontalCleanGrid = CleanByHorizontal();

		if(IsVerticalCleanGrid || IsHorizontalCleanGrid)
		{
			CleanAllSameColorGrid();
			CameraShake();
			yield return new WaitForSeconds(fCleanGridInterval);
			DropByGravity();
			yield return new WaitForSeconds(WaitForDropByGravity+0.3f);
			yield return StartCoroutine(CheckCleanUp());
		}
	}

	//检测竖排消除
	bool CleanByVertical()
	{		
		for(int i = 0; i < Width; i++)
		{
			for(int j = 0; j < Height-2; j++)
			{
				if(GridList[i,j].IsEmpty == false)
				{
					if(GridList[i,j+1].IsEmpty == false && GridList[i, j+2].IsEmpty == false)
					{
						SpriteRenderer sprite = GridList[i,j]._grid.GetComponent<SpriteRenderer>();
						Color color = sprite.color;
						if(IsColorSame(GridList[i, j+1]._grid, GridList[i, j]._grid) && 
							IsColorSame(GridList[i, j+1]._grid, GridList[i, j+2]._grid))
						{
							CleanOneSameColorGrid(i, j, color);
							return true;
						}
					}
				}
			}
		}

		return false;
	}

	//检测横排消除
	bool CleanByHorizontal()
	{
		for(int i = 0; i < Width-2; i++)
		{
			for(int j = 0; j < Height; j++)
			{
				if(GridList[i,j].IsEmpty == false)
				{
					if(GridList[i+1,j].IsEmpty == false && GridList[i+2, j].IsEmpty == false)
					{
						SpriteRenderer sprite = GridList[i,j]._grid.GetComponent<SpriteRenderer>();
						Color color = sprite.color;
						if(IsColorSame(GridList[i+1, j]._grid, GridList[i, j]._grid) && 
						   IsColorSame(GridList[i+1, j]._grid, GridList[i+2, j]._grid))
						{
							CleanOneSameColorGrid(i, j, color);
							return true;
						}
					}
				}
				
				
			}
		}

		return false;
	}

	void CleanOneSameColorGrid(int x, int y, Color color)
	{
		if(GridList[x,y].IsEmpty || GridList[x,y].IsNeedToBeDeleted == true)
		{
			return;
		}

		SpriteRenderer sprite1 = GridList[x,y]._grid.GetComponent<SpriteRenderer>();
		if (sprite1.color == color) 
		{
			//∫Debug.Log ("消除相同颜色， x=" + x + " y=" + y + " color.r=" + color.r + " color.g=" + color.g + " color.b=" + color.b);
			//DropClickedGrid (new IndexInfo(x,y));
			GridList[x,y].IsNeedToBeDeleted = true;
		} 
		else 
		{
			return;
		}

		//向其他方向继续搜索相同颜色的色块并消除
		if(x+1 < Width)
		{
			CleanOneSameColorGrid(x+1,y, color);
		}

		if(y+1 < Height)
		{
			CleanOneSameColorGrid(x,y + 1, color);
		}

		if(x-1 >= 0)
		{
			CleanOneSameColorGrid(x-1,y, color);
		}

		if(y-1 >= 0)
		{
			CleanOneSameColorGrid(x,y-1, color);
		}


	}

	bool IsColorSame(GameObject object1, GameObject object2)
	{
		SpriteRenderer sprite1 = object1.GetComponent<SpriteRenderer>();
		SpriteRenderer sprite2 = object2.GetComponent<SpriteRenderer>();
		return (sprite1.color == sprite2.color);
	}

	void AddOneLevel()
	{
		//层数整体加1
		for(int i = 0; i < Width; i++)
		{
			for(int j = Height - 2; j >= 0; j--)
			{
				if(GridList[i, j].IsEmpty == false)
				{
					//往上移动一格
					GridList[i, j+1].IsEmpty = false;
					GridList[i, j+1]._grid = GridList[i,j]._grid;
					GridList[i, j]._grid = null;
					GridList[i,j].IsEmpty = true;

					//animation
					iTween.MoveTo(GridList[i,j+1]._grid, iTween.Hash("position", GridList[i,j+1]._grid.transform.position + new Vector3(0, GridHeight, 0), "time", fAddOnLevelTime));
				}
			}
		}
		
		//新加一层
		for(int i = 0; i < Width; i++)
		{
			AddRandomGrid(i,0);
		}

		CheckGameOver ();
	}

	void CheckGameOver()
	{
		for (int i = 0; i < Width; i++) 
		{
			if(GridList[i, Height - 1].IsEmpty == false)
			{
				Debug.Log("游戏结束");
				IsGameOver = true;
				ShowUI_GameOver();
			}
		}
	}

	void PlaySound(AUDIO_TYPE type)
	{
		AudioManager.SendMessage ("PlaySound", type);
	}

	void CameraShake()
	{
		iTweenEvent.GetEvent(CameraObject, "CameraShake").Stop();
		iTweenEvent.GetEvent(CameraObject, "CameraShake").Play();
	}

	public IEnumerator Restart()
	{
		HideUI_GameOver ();

		Debug.Log("Restart begin--------");

		for (int i = 0; i < Width; i++) 
		{
			for(int j = 0; j < Height; j++)
			{
				if(GridList[i,j].IsEmpty == false)
				{
					DropClickedGrid(new IndexInfo(i,j));
				}
			}
		}
		PlaySound (AUDIO_TYPE.CLEAN);
		yield return new WaitForSeconds (0.5f);
		InitGrid ();
		yield return new WaitForSeconds (fAddOnLevelTime);

		IsGameOver = false;
		Debug.Log("Restart end--------");
	}

	public void Exit()
	{
		Application.LoadLevel("Start");
	}

	void ShowUI_GameOver()
	{
		m_UI_GameOver.SetActive (true);
	}

	void HideUI_GameOver()
	{
		m_UI_GameOver.SetActive (false);
	}
}
