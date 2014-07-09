using UnityEngine;
using System.Collections;

public class Tutorial_ContinueBtn : Btn_Base {

	public Tutorial tutorial;

	protected override void OnMouseUp ()
	{
		base.OnMouseUp ();
		tutorial.nextScreen ();
	}
}
