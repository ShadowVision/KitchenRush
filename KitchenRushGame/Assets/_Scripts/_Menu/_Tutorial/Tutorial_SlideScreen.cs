using UnityEngine;
using System.Collections;

public class Tutorial_SlideScreen : Menu_Screen {
	public Tutorial tutorial;
	public Board board;
	public int numberOfSlidesRequired = 5;
	private int i = 0;

	public override void showScreen ()
	{
		base.showScreen ();
		board.buildBoard ();
		board.canClick = false;
		board.onSlideDelegate = onSlide;
	}
	public override void disableScreen ()
	{
		base.disableScreen ();
		board.onSlideDelegate = null;
	}
	public void onSlide (){
		i++;
		if(i>= numberOfSlidesRequired){
			tutorial.nextScreen();
		}
	}
}
