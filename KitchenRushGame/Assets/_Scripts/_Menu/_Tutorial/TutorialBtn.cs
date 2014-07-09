using UnityEngine;
using System.Collections;

public class TutorialBtn : Btn_Base {

	protected override void OnMouseUp ()
	{
		base.OnMouseUp ();
		Application.LoadLevel ("Tutorial");
	}
}
