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
		Debug.Log ("animation ended");
		currentAnimationType = AnimationType.NONE;
		startIdle();
		Board.instance.resetInteraction ();
	}
	void Awake(){
		instance = (AnimationController)this;
	}
	// Update is called once per frame
	void Update () {
	
	}
	private void stopAllAnimations(){
		foreach(SpriteSequenceAnimation a in animations){
			a.stopAnimation();
		}
	}
	private void playAnim(int i){
		CancelInvoke ("playIdle");
		stopAllAnimations ();
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
			case AnimationType.GOOD3:
				playAnim (Random.Range(1,4));
				break;
			case AnimationType.COMBO1:
			case AnimationType.COMBO2:
			case AnimationType.COMBO3:
				int r = Random.Range(4,6);
				playAnim(r);
				if(r == 5){
					Invoke("finishPose",5f);
				}
				break;
			case AnimationType.WIN:
				playAnim(0);
				break;
			case AnimationType.LOSE:
				playAnim(0);
				break;
			
			}
			currentAnimationType = anim;
		}
	}
	private void finishPose(){
		playAnim (6);
	}
	private void startIdle(){
		animations [0].goToFrame (0);
		currentAnimationType = AnimationType.IDLE;
		Invoke("playIdle",timeBetweenIdleAnimationInSeconds);
	}
	private void playIdle(){
		stopAllAnimations ();
		if(currentAnimationType == AnimationType.IDLE){
			playAnim(0);
		}
	}
}
