using UnityEngine;
using System.Collections;

public class SoundFiles : MonoBehaviour {
	public static SoundFiles instance;
	public AudioSource soundChannel;
	public AudioClip clickSound;
	public AudioClip slideSound;
	public AudioClip completeSound;
	public AudioClip complete2Sound;
	public AudioClip gameOverSound;
	void Start(){
		//_clickSound = Resources.Load<AudioSource> ("_Sounds/Clicks");
	}
	// Use this for initialization
	void Awake () {
		instance = (SoundFiles)this;
	}
	public void playSound(AudioClip sound){
		if(STATS.soundOn){
			soundChannel.clip = sound;
			soundChannel.Play ();
		}
	}
	public static void playClick(){
		instance.playSound(instance.clickSound);
	}
	public static void playSlide(){
		instance.playSound(instance.slideSound);
	}
	public static void playComplete(){
		instance.playSound(instance.completeSound);
	}
	public static void playComplete2(){
		instance.playSound(instance.complete2Sound);
	}
	public static void playGameOver(){
		instance.playSound(instance.gameOverSound);
	}
}
