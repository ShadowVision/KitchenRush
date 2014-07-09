using UnityEngine;
using System.Collections;

public class Reward : MonoBehaviour {
	public Rewards_Screen screen;
	public TextMesh txt;
	public SpriteRenderer normalSprite;
	public SpriteRenderer purchasedSprite;
	public int costInOrbs = 99;
	public string label = "Name Of Dish";
	public int id = 0;

	private bool redeemed = false;
	// Use this for initialization
	void Start () {
		txt.text = "x"+costInOrbs;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseUp(){
		if(!screen.isDragging){
			if(!redeemed){
				if(STATS.orbs >= costInOrbs){
					redeem();
				}
			}else{
				//already purchased
				screen.showRewardConfirmation((Reward)this);
			}
		}
	}
	private void redeem(){
		bool successfull = FacebookController.instance.spendOrbs(costInOrbs);
		if(successfull){
			FacebookController.instance.setReward(id, true);
			redeemed = true;
			screen.updateText();
			turnOff();
		}
	}
	public void use(){
		if(redeemed){
			redeemed = false;
			STATS.rewardsEarned[id] = false;
			FacebookController.instance.setReward(id, false,label);
		}
	}
	private void turnOff(){
		txt.renderer.enabled = false;
		normalSprite.enabled = false;
		purchasedSprite.enabled = true;
	}
	public void show(){
		collider2D.enabled =true;
		txt.renderer.enabled = true;
		normalSprite.enabled = true;
		purchasedSprite.enabled = false;
		if(STATS.rewardsEarned[id]){
			redeemed = true;
			turnOff();
		}
	}
	public void hide(){
		collider2D.enabled =false;
		txt.renderer.enabled = false;
		normalSprite.enabled = false;
		purchasedSprite.enabled = false;
	}
}
