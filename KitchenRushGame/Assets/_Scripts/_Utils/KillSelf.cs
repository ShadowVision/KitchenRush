using UnityEngine;
using System.Collections;

public class KillSelf : MonoBehaviour {
	public float timeTillDeathInSeconds = 1;
	// Use this for initialization
	void Start () {
		Invoke ("die",timeTillDeathInSeconds);
	}
	private void die(){
		Destroy(gameObject);
	}
}
