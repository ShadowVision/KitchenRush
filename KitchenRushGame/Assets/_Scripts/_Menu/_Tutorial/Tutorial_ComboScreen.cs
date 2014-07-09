using UnityEngine;
using System.Collections;

public class Tutorial_ComboScreen : Menu_Screen {
	public Tutorial tutorial;
	public Board board;
	public int numberRequired = 3;
	
	public override void showScreen ()
	{
		base.showScreen ();
		board.buildBoard ();
		board.onTapDelegate = onTap;
	}
	public override void disableScreen ()
	{
		base.disableScreen ();
		board.onTapDelegate = null;
	}
	public void onTap (){
		if(STATS.multiplier >= numberRequired){
			tutorial.nextScreen();
		}
	}
}