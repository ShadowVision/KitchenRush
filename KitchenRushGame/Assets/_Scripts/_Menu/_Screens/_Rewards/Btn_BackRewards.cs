using UnityEngine;
using System.Collections;

public class Btn_BackRewards : Btn_Base {
	public Rewards_Screen screen;
	override protected void OnMouseUp(){
		base.OnMouseUp ();
		screen.clickBack();
	}
}
