using UnityEngine;
using System.Collections;

public class Btn_SkipFacebook : Btn_Base {

	override protected void OnMouseUp(){
		base.OnMouseUp ();
		MenuScreenController.instance.showFacebookWarning();
	}
}
