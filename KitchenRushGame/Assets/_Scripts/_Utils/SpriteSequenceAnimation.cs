using UnityEngine;
using System.Collections;

public class SpriteSequenceAnimation : MonoBehaviour {
	public string spritePath;
	public float fps = 24f;
	public bool autoPlay = false;
	public bool loop = true;
	public SpriteRenderer sprite;
	private Sprite[] animationSequence;
	private float delayBetweenFramesInSeconds = .1f;
	private int frameIndex = 0;

	public delegate void OnAnimationEnd();
	public OnAnimationEnd onAnimationEnd;
	// Use this for initialization
	protected virtual void Start () {
		frameIndex = 0;
		animationSequence = Resources.LoadAll<Sprite>(spritePath);
		if(autoPlay){
			playAnimation();
		}
	}
	
	/*public float animationWidth{
		get{
			return animationSequence[0].width;
		}
	}
	public float animationHeight{
		get{
			return animationSequence[0].height;
		}
	}*/
	
	// Update is called once per frame
	void Update () {
	
	}

	public void playAnimation(){
		pauseAnimation();
		delayBetweenFramesInSeconds = 1/fps;
		InvokeRepeating("tickAnimation", delayBetweenFramesInSeconds, delayBetweenFramesInSeconds);
	}

	public void stopAnimation(){
		CancelInvoke("tickAnimation");
		frameIndex = 0;
	}
	public void pauseAnimation(){
		CancelInvoke("tickAnimation");
	}
	private void tickAnimation(){
		frameIndex++;
		if(frameIndex >= animationSequence.Length){
			frameIndex= animationSequence.Length-1;
			if(!loop){
				pauseAnimation();
				if(onAnimationEnd != null){
					onAnimationEnd();
				}
			}else{
				frameIndex = 0;
			}
		}
		sprite.sprite = animationSequence[frameIndex];
	}
}
