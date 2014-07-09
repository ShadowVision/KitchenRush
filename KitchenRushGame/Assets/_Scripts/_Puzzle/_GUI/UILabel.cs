using UnityEngine;
using System.Collections;

public class UILabel : MonoBehaviour {
	public TextMesh text;
	public string label = "Thing: ";
	protected int amount = 0;

	protected void Start(){
	}
	void FixedUpdate(){
		if(text!=null){
			text.text = label + amount;
		}
	}
}
