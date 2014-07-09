using UnityEngine;
using System.Collections;

public class Btn_Challenges : Btn_Base {

	override protected void OnMouseUp(){
		base.OnMouseUp ();
		GameLogic.instance.loadingScreen.showScreen();
		STATS.showRewards = true;
		Application.LoadLevel ("MainMenu");
	}
}
