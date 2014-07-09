using UnityEngine;
using System.Collections;

public class Btn_ContinueWarning : Btn_Base {

	override protected void OnMouseUp(){
		base.OnMouseUp ();
		STATS.canEarnRewards = false;
		MenuScreenController.instance.showMenu();
	}
}
