using UnityEngine;
using System.Collections;

public class Tutorial_TapScreen : Menu_Screen {
	public Tutorial tutorial;
	public Board board;
	public int numberRequired = 5;
	private int i = 0;

	public override void showScreen ()
	{
		base.showScreen ();
		board.buildBoard ();
		board.canClick = true;
		board.onTapDelegate = onTap;
	}
	public override void disableScreen ()
	{
		base.disableScreen ();
		board.onTapDelegate = null;
	}
	public void onTap (){
		i++;
		if(i>= numberRequired){
			tutorial.nextScreen();
		}
	}
}
