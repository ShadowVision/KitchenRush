using UnityEngine;
using System.Collections;

public class Objective : MonoBehaviour {
	static public Objective instance;
	public TextMesh text;
	public Tile.TileType tileType;
	public int numberOfTilesNeeded;
	public GameObject spawnOnCompletion;

	private int tilesCaptured = 0;
	// Use this for initialization
	void Start () {
		instance = (Objective)this;
		updateText();
	}

	// Update is called once per frame
	void Update () {
	
	}

	public bool captureTile(Tile.TileType captureType){

		//add points for captured tile
		if(captureType == Tile.TileType.GOLD){
			Score.addPoints(STATS.pointsTileGold);
		}else if(captureType == tileType){
			animation.Play();
			Score.addPoints(STATS.pointsTileObjective);
		}else{
			Score.addPoints(STATS.pointsTileNonObjective);
		}

		//Add tile to objective
		if(tilesCaptured < numberOfTilesNeeded && captureType == tileType){
			tilesCaptured++;
			updateText();
			if(tilesCaptured >= numberOfTilesNeeded){
				finishObjective();
				return true;
			}
		}
		return false;
	}
	private void updateText(){
		if(text != null){
			text.text = " x" + (numberOfTilesNeeded-tilesCaptured);
		}
	}
	private void finishObjective(){
		if(spawnOnCompletion != null){
			Instantiate(spawnOnCompletion, transform.position,Quaternion.identity);
		}
		//Add points 
		Score.addPoints(numberOfTilesNeeded * STATS.pointsObjectiveCompleteBonusPerTile);

		//kill self after playing die animation
		AnimationController.instance.playAnimation(AnimationController.AnimationType.COMBO1);
		Board.instance.pauseInteraction(1.5f);
		Invoke("die",1.5f);
	}
	private void die(){
		GameLogic.instance.completeObjective((Objective)this);
		Destroy(gameObject);
	}
}
