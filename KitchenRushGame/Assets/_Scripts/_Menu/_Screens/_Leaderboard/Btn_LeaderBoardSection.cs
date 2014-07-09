using UnityEngine;
using System.Collections;

public class Btn_LeaderBoardSection : Btn_Base {
	public Leaderboard_Screen screen;
	public int i = 0;
	public SpriteRenderer selected;
	public SpriteRenderer deselected;

	override protected void OnMouseUp(){
		base.OnMouseUp ();
		screen.selectSection (i);
	}
	public void select(){
		selected.enabled = true;
		deselected.enabled = false;
	}
	public void deselect(){
		selected.enabled = false;
		deselected.enabled = true;
	}
}
