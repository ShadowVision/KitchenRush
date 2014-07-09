using UnityEngine;
using System.Collections;

public class Btn_Retry : Btn_Base {

	override protected void OnMouseUp(){
		base.OnMouseUp ();
		GameLogic.instance.loadingScreen.showScreen();
		Application.LoadLevel ("Game");
	}
}
