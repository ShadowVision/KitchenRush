using UnityEngine;
using System.Collections;

public class MainMenu_Screen : Menu_Screen {
	public Btn_Rewards rewardsBtn;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void showScreen ()
	{
		base.showScreen ();
		rewardsBtn.updateGraphics();
	}
}
