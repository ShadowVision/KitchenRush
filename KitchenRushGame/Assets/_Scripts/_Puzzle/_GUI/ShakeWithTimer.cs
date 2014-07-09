using UnityEngine;
using System.Collections;

public class ShakeWithTimer : MonoBehaviour {
	public Timer timer;
	public Shake[] shakeObjects;
	public float maxShakeAmount = .2f;
	public float timeOffset = .5f;

	// Update is called once per frame
	void FixedUpdate () {
		if(timer.timerOn){
			float delta = Mathf.Max(0,timer.timeDelta - timeOffset) / (1-timeOffset);
			foreach(Shake shakeObject in shakeObjects){
				shakeObject.shakeAmount = Mathf.Lerp (0, maxShakeAmount, delta);
			}
		}else{
			foreach(Shake shakeObject in shakeObjects){
				shakeObject.shakeAmount = 0;
			}
		}
	}
}
