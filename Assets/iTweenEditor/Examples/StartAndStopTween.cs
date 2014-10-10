using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class StartAndStopTween : MonoBehaviour {
	
	public GameObject target;

	public GameObject cameraObject;

	public GameObject grid;
	private Transform gridTransform;

	void Start()
	{
		Debug.Log("start");
		gridTransform = grid.transform;
	}
	
	void OnGUI() {
		if(GUILayout.Button("Start Bounce")) {
			iTweenEvent.GetEvent(target, "Bounce").Play();
		}
		
		if(GUILayout.Button("Stop Bounce")) {
			iTweenEvent.GetEvent(target, "Bounce").Stop();
		}
		
		if(GUILayout.Button("Start Color Fade")) {
			iTweenEvent.GetEvent(target, "Color Fade").Play();
		}
		
		if(GUILayout.Button("Stop Color Fade")) {
			iTweenEvent.GetEvent(target, "Color Fade").Stop();
		}

		//mj add
		if(GUILayout.Button("Start Camera Shake")) {
			iTweenEvent.GetEvent(cameraObject, "CameraShake").Play();
		}
		
		if(GUILayout.Button("Stop Camera Shake")) {
			iTweenEvent.GetEvent(cameraObject, "CameraShake").Stop();
		}

		if(GUILayout.Button("Start Grid Drop")) {
			iTweenEvent.GetEvent(grid, "GridRotate").Play();

			Rigidbody2D gridBody =  grid.GetComponent<Rigidbody2D>();
			gridBody.AddForce(new Vector2(0,200.0f));
			gridBody.gravityScale = 1.0f;

			//grid.GetComponent<Rigidbody2D>().AddForce(new Vector3(0, 2.0f, 0));;
			//grid.GetComponent<Rigidbody2D>().gravityScale = 1.0f;

		}
		
		if(GUILayout.Button("Stop Grid Drop")) {
			iTweenEvent.GetEvent(grid, "GridRotate").Stop();
			grid.transform.position = gridTransform.position;
			Rigidbody2D gridBody =  grid.GetComponent<Rigidbody2D>();
			gridBody.transform.position = gridTransform.position;
			gridBody.gravityScale = 0;
			gridBody.velocity = new Vector2(0,0);
		}
	}
}