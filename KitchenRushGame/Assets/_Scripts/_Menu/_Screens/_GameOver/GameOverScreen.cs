using UnityEngine;
using System.Collections;

public class GameOverScreen : MonoBehaviour {
	public TextMesh scoreTxt;
	public GameObject[] orbSprites;
	private int index =0;
	private string txtTxt = "";
	private int orbs = 0;
	private Animator anim;
	// Use this for initialization
	void Start () {
		scoreTxt.text = txtTxt;
		anim.Play ("GameOverAnimation");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void setScore(int amount, int orbs){
		txtTxt = amount+" Points";
		if(scoreTxt != null){
			scoreTxt.text = txtTxt;
		}
		this.orbs = orbs;
		anim = gameObject.GetComponent<Animator> ();
		animateOrbs();
	}
	private void animateOrbs(){
		float time = 1.75f;
		index = 0;
		for(int i=0; i<orbs; i++){
			Invoke("playOrb", time);
			time+= .3f;
		}
	}
	private void playOrb(){
		orbSprites[index].animation.Play();
		index++;
	}
	public void setScore(int amount){
		scoreTxt.text = amount + " Points";
	}
}
