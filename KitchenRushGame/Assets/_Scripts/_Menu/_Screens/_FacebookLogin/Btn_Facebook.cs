using UnityEngine;
using System.Collections;

public class Btn_Facebook : Btn_Base {
	override protected void OnMouseUp(){
		base.OnMouseUp ();
		FacebookController.instance.login();
	}
}
