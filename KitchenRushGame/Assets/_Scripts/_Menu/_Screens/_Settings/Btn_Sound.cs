using UnityEngine;
using System.Collections;

public class Btn_Sound : Btn_Base {
	public SpriteRenderer onSprite;
	public SpriteRenderer offSprite;
	void Awake(){
		updateGraphics ();
	}
	public void updateGraphics(){
		if(STATS.soundOn){
			onSprite.enabled = true;
			offSprite.enabled = false;
		}else{
			onSprite.enabled = false;
			offSprite.enabled = true;
		}
	}
	override protected void OnMouseUp(){
		if(STATS.soundOn){
			STATS.soundOn = false;
		}else{
			STATS.soundOn = true;
		}
		updateGraphics();
		base.OnMouseUp ();
	}
}
