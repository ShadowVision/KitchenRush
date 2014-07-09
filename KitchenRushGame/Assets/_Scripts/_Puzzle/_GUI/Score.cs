using UnityEngine;
using System.Collections;

public class Score : UILabel {
	static private Score instance;
	// Use this for initialization
	new void Start () {
		Score.instance = (Score)this;
		base.Start();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	static public void addPoints(int points){
		instance.amount+= points*STATS.multiplier;
	}
	public int points{
		get{
			return amount;
		}
	}
}
