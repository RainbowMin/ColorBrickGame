using UnityEngine;
using System.Collections;

public class BottomPoint : MonoBehaviour 
{
	public SpriteRenderer _BgSprite;

	void Start()
	{
		SetPos ();

	}

	void SetPos()
	{
		transform.position = new Vector3 (OldPos.x * WidthScale, OldPos.y, OldPos.z);
	}
}
