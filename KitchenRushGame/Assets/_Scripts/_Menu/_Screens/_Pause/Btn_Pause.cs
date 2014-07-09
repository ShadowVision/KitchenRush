using UnityEngine;
using System.Collections;

public class Btn_Pause : Btn_Base {

	override protected void OnMouseUp(){
		base.OnMouseUp ();
		if(GameLogic.instance.isPaused){
			GameLogic.instance.resumeGame();
		}else{
			GameLogic.instance.pauseGame();
		}
	}
}
