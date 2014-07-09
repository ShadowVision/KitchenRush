using UnityEngine;
using System.Collections;

public class Combo : UILabel {
	static private Combo instance;
	// Use this for initialization
	new void Start () {
		Combo.instance = (Combo)this;
		base.Start();
		amount = 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	static public void addCombo(int addAmount){
		instance.amount+= addAmount;
		if(instance.amount < 1){
			instance.amount = 1;
		}
		STATS.multiplier = instance.amount;
	}
	static public void resetCombo(){
		instance.amount = 1;
		STATS.multiplier = instance.amount;
	}
}
