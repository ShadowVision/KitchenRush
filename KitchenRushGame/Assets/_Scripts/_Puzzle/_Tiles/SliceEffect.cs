using UnityEngine;
using System.Collections;

public class SliceEffect : MonoBehaviour {
	Animator anim;
	int id = 0;
	// Use this for initialization
	void Awake () {
		anim = gameObject.GetComponent<Animator> ();
		//id = Random.Range (1, 1);
		id = 1;
		Invoke ("playAnimation", Random.Range (.0f, .3f));
	}
	private void playAnimation(){
		anim.SetInteger ("SliceID", id);
	}
}
