using UnityEngine;
using System.Collections;

public class Btn_Settings : Btn_Base {

	override protected void OnMouseUp(){
		base.OnMouseUp ();
		MenuScreenController.instance.showSettings();
	}
}
