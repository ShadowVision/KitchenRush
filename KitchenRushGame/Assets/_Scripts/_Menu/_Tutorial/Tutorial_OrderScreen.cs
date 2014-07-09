using UnityEngine;
using System.Collections;

public class Tutorial_OrderScreen : Menu_Screen {
	public Tutorial tutorial;
	public Board board;

	public override void showScreen ()
	{
		base.showScreen ();
		board.buildBoard ();
		board.onOrderDelegate = onOrderComplete;
		GameLogic.instance.newObjective ();
	}
	public override void disableScreen ()
	{
		base.disableScreen ();
		board.onOrderDelegate = null;
	}
	public void onOrderComplete (){
		tutorial.nextScreen ();
	}
}
