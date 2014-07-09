using UnityEngine;
using System.Collections;

public class Menu_SettingsScreen : Menu_Screen {
	public Btn_Sound soundBtn;
	// Use this for initialization
	void Start () {
		hideScreen ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public override void showScreen ()
	{
		base.showScreen ();
		soundBtn.updateGraphics();
	}
	public override void hideScreen ()
	{
		base.hideScreen ();

	}
}
