using UnityEngine;
using System.Collections;

public class Btn_Play : Btn_Base {

	override protected void OnMouseUp(){
		base.OnMouseUp ();
		if(PlayerPrefs.GetInt("CompletedTutorial",0) == 1){
			MenuScreenController.instance.playGame();
		}else{
			MenuScreenController.instance.playTutorial();
		}
	}
}
