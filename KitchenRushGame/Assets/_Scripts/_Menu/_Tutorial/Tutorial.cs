using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour {
	public Menu_Screen[] screens;

	private int i;
	// Use this for initialization
	void Start () {
		hideAllScreens ();
		i = 0;
		screens [i].showScreen ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	private void hideAllScreens(){
		foreach(Menu_Screen screen in screens){
			screen.hideScreen();
		}
	}

	public void nextScreen(){
		screens [i].hideScreen ();
		Board.instance.destroyBoard ();
		i++;
		if(i < screens.Length){
			screens [i].showScreen ();
		}else{
			endTutorial();
		}
	}
	private void endTutorial(){
		PlayerPrefs.SetInt ("CompletedTutorial", 1);
		Application.LoadLevel ("Game");
	}
}
