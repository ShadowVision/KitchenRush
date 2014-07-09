using UnityEngine;
using System.Collections;

public class Btn_Base : MonoBehaviour {

	protected virtual void OnMouseUp(){
		//play click sound
		SoundFiles.playClick ();
	}
}
