using UnityEngine;
using System.Collections;

public class AnimationController : MonoBehaviour {
	public static AnimationController instance;
	public enum AnimationType{
		NONE,
		IDLE,
		LOSE,
		WIN,
		GOOD1,
		GOOD2,
		GOOD3,
		COMBO1,
		COMBO2,
		COMBO3
	}
	private AnimationType currentAnimationType = AnimationType.NONE;
	private SpriteSequenceAnimation currentAnimation;
	private SpriteSequenceAnimation[] animations;
	
	public float timeBetweenIdleAnimationInSeconds = 5f;

	// Use this for initialization
	void Start () {
		animations = GetComponents<SpriteSequenceAnimation>();
		foreach(SpriteSequenceAnimation a in animations){
			a.onAnimationEnd = animationEnd;
		}
		playAnimation(AnimationType.IDLE);
	}
	private void animationEnd(){
		currentAnimationType = AnimationType.NONE;
		startIdle();
	}
	void Awake(){
		instance = (AnimationController)this;
	}
	// Update is called once per frame
	void Update () {
	
	}
	private void playAnim(int i){
		if(animations != null){
			currentAnimation = animations[i];
			if(currentAnimation != null){
				currentAnimation.playAnimation();
			}
		}
	}

	public void playAnimation(AnimationType anim){
		if(anim != currentAnimationType){
			Debug.Log("playing animation: " + anim);
			if(currentAnimation!= null){
				currentAnimation.stopAnimation();
			}
			switch(anim){
			case AnimationType.IDLE:
				startIdle();
				break;
			case AnimationType.GOOD1:
			case AnimationType.GOOD2:
			case AnimationType.GOOD3	:
				playAnim (1);
				break;
			case AnimationType.COMBO1:
				playAnim(2);
				break;
			case AnimationType.WIN:
				CancelInvoke("playIdle");
				playAnim(3);
				break;
			case AnimationType.LOSE:
				CancelInvoke("playIdle");
				playAnim(4);
				break;
			
			}
			currentAnimationType = anim;
		}
	}
	private void startIdle(){
		currentAnimationType = AnimationType.IDLE;
		Invoke("playIdle",timeBetweenIdleAnimationInSeconds);
	}
	private void playIdle(){
		if(currentAnimationType == AnimationType.IDLE){
			playAnim(0);
		}
	}
}
