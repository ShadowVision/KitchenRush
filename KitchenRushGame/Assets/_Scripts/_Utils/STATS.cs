using UnityEngine;
using System.Collections;

public class STATS : MonoBehaviour {
	static public int currentAppVersion = 1;
	static public int pointsTileNonObjective = 5;
	static public int pointsTileObjective = 15;
	static public int pointsTileGold = 200;
	static public int pointsObjectiveCompleteBonusPerTile = 200;
	static public int multiplier = 1;
	static public bool showRewards = false;
	static public int orbs = 0;
	static public bool[] rewardsEarned = new bool[6]{false,true,false,false,false,false};
	static public bool canEarnRewards = false;
	static public bool soundOn{
		get{
			int i = PlayerPrefs.GetInt("soundOn",1);
			if(i == 0){
				return false;
			}
			return true;
		}
		set{
			int i = 0;
			if(value){
				i = 1;
			}
			PlayerPrefs.SetInt("soundOn",i);
			if(GameLogic.instance != null){
				GameLogic.instance.updateSound();
			}
			if(MenuScreenController.instance != null){
				MenuScreenController.instance.updateSound();
			}
		}
	}
	static public bool appDataLoaded = false;
	static public string currentUserName = "Anonymous";
}
