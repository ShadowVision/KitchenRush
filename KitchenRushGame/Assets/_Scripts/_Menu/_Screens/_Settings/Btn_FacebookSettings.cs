using UnityEngine;
using System.Collections;

public class Btn_FacebookSettings : Btn_Base {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	override protected void OnMouseUp(){
		base.OnMouseUp ();
		FacebookController.instance.logout();
		MenuScreenController.instance.hideSettings();
		MenuScreenController.instance.showLogin();
	}
}
