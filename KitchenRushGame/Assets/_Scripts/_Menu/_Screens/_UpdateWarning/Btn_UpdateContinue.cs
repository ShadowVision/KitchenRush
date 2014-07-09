using UnityEngine;
using System.Collections;

public class Btn_UpdateContinue : MonoBehaviour {
	public MenuScreenController screen;
	void OnMouseUp(){
		screen.hideUpdateWarning();
	}
}
