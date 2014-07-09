using UnityEngine;
using System.Collections;

public class Highscore : MonoBehaviour {
	public TextMesh nameTxt;
	public TextMesh dateTxt;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setLabel(int place, string name, int score, string date){
		nameTxt.text = place + ". " + name + " - " + score;
		dateTxt.text = date;
	}
}
