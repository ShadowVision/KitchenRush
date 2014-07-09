using UnityEngine;
using System.Collections;

public class Btn_BackWarning : Btn_Base {

	override protected void OnMouseUp(){
		base.OnMouseUp ();
		MenuScreenController.instance.hideFacebookWarning();
	}
}
