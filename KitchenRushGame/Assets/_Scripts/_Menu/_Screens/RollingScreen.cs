using UnityEngine;
using System.Collections;

public class Rolling_Screen : Menu_Screen {
	
	public GameObject draggableElement;
	public float maxTop = 6;
	public float maxBottom = 0;

	[HideInInspector]
	public bool isDragging = false;
	private Vector2 startingMousePos;
	private Vector3 startingPos;
	private float minMouseMoveRequiredForDrag = 20;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	protected void Update () {
		if(Input.GetMouseButtonDown(0)){
			//start drag
			startingMousePos = Input.mousePosition;
			startingPos = draggableElement.transform.position;
		}
		if(Input.GetMouseButton(0)){
			//check drag
			float mouseDist = Input.mousePosition.y-startingMousePos.y;
			
			if(!isDragging){
				//if the mouse has moved enough while clicking
				if(Mathf.Abs(mouseDist) > minMouseMoveRequiredForDrag){
					isDragging = true;
				}
			}else{
				//drag
				Vector3 newPosition = startingPos;
				newPosition.y += mouseDist*.01f;
				newPosition.y = Mathf.Min(newPosition.y, maxTop);
				newPosition.y = Mathf.Max(newPosition.y, maxBottom);
				draggableElement.transform.position = newPosition;
			}
		}
		if(Input.GetMouseButtonUp(0)){
			//stop drag
			isDragging = false;
		}
	}
}
