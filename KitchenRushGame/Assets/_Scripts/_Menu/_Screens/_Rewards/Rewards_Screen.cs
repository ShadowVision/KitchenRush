using UnityEngine;
using System.Collections;

public class Rewards_Screen : Rolling_Screen {
	public TextMesh orbsTxt;
	public TextMesh otherTxt;
	public Reward[] rewards;
	public GameObject hostScreen;
	public SpriteRenderer hostRewardSprite;

	private bool showingConfirmation = false;

	private string passTxt = "";
	private Rect passRect;
	private GUIStyle style;
	private Reward savedReward;
	private string password = "Sage";

	// Use this for initialization
	void Start () {
		style = new GUIStyle();
		style.fontSize = 30;
		passRect = new Rect(Screen.width/2 + 10, Screen.height/2, 500, 50);
	}

	void OnGUI(){
		if(showingConfirmation){
			passTxt = GUI.TextField(passRect,passTxt,style);
			if(passTxt == password){
				passTxt = "";
				savedReward.use();
				hideConfirmation();
			}
		}
	}
	public override void showScreen ()
	{
		base.showScreen ();
		FacebookController.instance.getRewards();
		password = FacebookController.instance.loadRedeemPassword ();
		foreach(Reward reward in rewards){
			reward.show();
		}
		orbsTxt.renderer.enabled = true;
		otherTxt.renderer.enabled = true;
		updateText();
		hideConfirmation();
		STATS.orbs = FacebookController.instance.getOrbs();
		orbsTxt.text = "x"+STATS.orbs;
	}
	public override void hideScreen ()
	{
		base.hideScreen ();
		foreach(Reward reward in rewards){
			reward.hide();
		}
		orbsTxt.renderer.enabled = false;
		otherTxt.renderer.enabled = false;

	}
	public void updateText(){
		orbsTxt.text = "x"+STATS.orbs;
	}
	public void showRewardConfirmation(Reward reward){
		savedReward = reward;
		showConfirmation();
		hostRewardSprite.sprite = reward.normalSprite.sprite;
	}
	private void showConfirmation(){
		showingConfirmation = true;
		Renderer[] ren = hostScreen.GetComponentsInChildren<Renderer>();
		foreach(Renderer r in ren){
			r.enabled = true;
		}
		foreach(Reward reward in rewards){
			reward.hide();
		}
		hostRewardSprite.enabled = true;
	}
	private void hideConfirmation(){
		showingConfirmation = false;
		Renderer[] ren = hostScreen.GetComponentsInChildren<Renderer>();
		foreach(Renderer r in ren){
			r.enabled = false;
		}
		foreach(Reward reward in rewards){
			reward.show();
		}
		hostRewardSprite.enabled = false;
	}
	public void clickBack(){
		if(showingConfirmation){
			hideConfirmation();
		}else{
			MenuScreenController.instance.hideRewards();
		}
	}
}
