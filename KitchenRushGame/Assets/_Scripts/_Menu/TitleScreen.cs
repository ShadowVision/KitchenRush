using UnityEngine;
using System.Collections;

public class TitleScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Invoke("moveOn",7f);
	}
	private void Update(){
		if(Input.GetMouseButtonDown(0)){
			moveOn();
		}
	}
	private void moveOn(){
		Application.LoadLevel("MainMenu");
	}
}
