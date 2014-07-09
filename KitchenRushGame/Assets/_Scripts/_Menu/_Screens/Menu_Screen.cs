using UnityEngine;
using System.Collections;

public class Menu_Screen : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	virtual public void disableScreen(){
		foreach(BoxCollider2D box in gameObject.GetComponentsInChildren<BoxCollider2D>()){
			box.enabled = false;
		}
		enabled = false;
	}
	virtual public void enableScreen(){
		foreach(BoxCollider2D box in gameObject.GetComponentsInChildren<BoxCollider2D>()){
			box.enabled = true;
		}
		enabled = true;
	}
	virtual public void hideScreen(){
		disableScreen();
		foreach(SpriteRenderer sprite in gameObject.GetComponentsInChildren<SpriteRenderer>()){
			sprite.enabled = false;
		}
		foreach(Renderer ren in gameObject.GetComponentsInChildren<Renderer>()){
			ren.enabled = false;
		}
	}
	virtual public void showScreen(){
		enableScreen();
		foreach(SpriteRenderer sprite in gameObject.GetComponentsInChildren<SpriteRenderer>()){
			sprite.enabled = true;
		}
		foreach(Renderer ren in gameObject.GetComponentsInChildren<Renderer>()){
			ren.enabled = true;
		}
	}
}
