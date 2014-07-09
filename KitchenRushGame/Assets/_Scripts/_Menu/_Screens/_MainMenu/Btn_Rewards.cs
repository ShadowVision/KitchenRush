using UnityEngine;
using System.Collections;

public class Btn_Rewards : Btn_Base {
	public SpriteRenderer lockedSprite;
	void Start(){

	}
	public void updateGraphics(){
		if(STATS.canEarnRewards){
			lockedSprite.enabled = false;
			collider2D.enabled = true;
		}else{
			lockedSprite.enabled = true;
			collider2D.enabled = false;
		}
	}
	override protected void OnMouseUp(){
		base.OnMouseUp ();
		if(STATS.canEarnRewards){
			MenuScreenController.instance.showRewards();
		}
	}
}
