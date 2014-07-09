using UnityEngine;
using System.Collections;

public class Btn_Leaderboard : Btn_Base {

	override protected void OnMouseUp(){
		base.OnMouseUp ();
		MenuScreenController.instance.showLeaderboard();
	}
}
