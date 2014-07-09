using UnityEngine;
using System.Collections;

public class AutoReposition : MonoBehaviour {
	
	Camera cam;
	float cameraHeight;
	float cameraWidth;
	float ratio = 0;
	float targetEdge = 0;

	public float sideMargin = 0;
	public bool onRightSide = false;
	public bool recuring = false;

	// Use this for initialization
	void Awake () {
		setVariables ();
		setPosition();
	}
	private void setVariables(){
		ratio = (float)Screen.width / (float)Screen.height;
		cam = Camera.main;
		cameraHeight = Camera.main.orthographicSize;
		cameraWidth = cameraHeight * ratio;
		targetEdge = 0;
		if (onRightSide) {
			//on right side
			targetEdge = cam.transform.position.x + cameraWidth;
		}else{
			//on left side
			targetEdge = cam.transform.position.x - cameraWidth;
		}
	}
	private void setPosition(){
		Vector3 newPos = transform.position;
		newPos.x = targetEdge + sideMargin;
		transform.position = newPos;
	}
	
	// Update is called once per frame
	void Update () {
		if(recuring){
			setVariables ();
			setPosition();
		}
	}
}
