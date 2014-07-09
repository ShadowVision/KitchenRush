using UnityEngine;
using System.Collections;

public class Btn_BackLeaderboard : Btn_Base {

	override protected void OnMouseUp(){
		base.OnMouseUp ();
		MenuScreenController.instance.hideLeaderboard();
		MenuScreenController.instance.showMenu();
	}
}
