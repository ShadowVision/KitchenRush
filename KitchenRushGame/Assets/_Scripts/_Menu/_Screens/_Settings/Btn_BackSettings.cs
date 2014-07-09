using UnityEngine;
using System.Collections;

public class Btn_BackSettings : Btn_Base {

	override protected void OnMouseUp(){
		base.OnMouseUp ();
		MenuScreenController.instance.hideSettings();
		MenuScreenController.instance.showMenu();
	}
}
