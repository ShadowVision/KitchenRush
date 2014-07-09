using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {
	public GameObject scaleObject;
	public Vector3 scaleRatio = new Vector3(0,1,0);
	private float timerLengthInSeconds = 1;
	private OnTimerCompletion onTimerComplete = null;
	private float startTime;
	private bool timerGoing = false;
	private float pauseTime; 
	private bool paused = false;

	public delegate void OnTimerCompletion();


	public void startTimer(float lengthInSeconds, OnTimerCompletion func){
		if(!timerGoing){
			timerGoing = true;
			timerLengthInSeconds = lengthInSeconds;
			onTimerComplete = func;
			startTime = Time.time;
		}
	}
	private void FixedUpdate(){
		if(timerGoing){
			if(!paused){
				Vector3 scale = new Vector3(1,1,1);
				float scaleDelta = 1-timeDelta;
				Vector3 newScale = scaleRatio * scaleDelta;


				if(scaleDelta <= 0){
					endTimer();
					scale = Vector3.zero;
				}else{
					if(newScale.x != 0){scale.x = newScale.x;}
					if(newScale.y != 0){scale.y = newScale.y;}
					if(newScale.z != 0){scale.z = newScale.z;}
				}
				scaleObject.transform.localScale = scale;
			}else{
				startTime += Time.deltaTime;
			}
		}
	}
	private void endTimer(){
		onTimerComplete();
		timerGoing = false;
	}
	public void pauseTimer(){
		paused = true;
	}
	public void resumeTimer(){
		paused = false;
	}
	public float timeDelta{
		get{
			return ((Time.time - startTime) / timerLengthInSeconds);
		}
	}
	public bool timerOn{
		get{
			return timerGoing;
		}
	}
}
