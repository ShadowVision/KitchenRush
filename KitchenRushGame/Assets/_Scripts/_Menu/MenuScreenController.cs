using UnityEngine;
using System.Collections;

public class MenuScreenController : MonoBehaviour {
	static public MenuScreenController instance;
	public Menu_Screen loginScreen;
	public Menu_Screen facebookWarning;
	public Menu_Screen mainMenu;
	public Menu_Screen settingsScreen;
	public Menu_Screen rewardsScreen;
	public Menu_Screen loadingScreen;
	public Menu_Screen updateWarning;
	public Menu_Screen leaderboardScreen;
	public AudioSource music;

	[HideInInspector]
	public bool loaded = false;
	// Use this for initialization
	void Start () {
		resetMenuPositions ();
		instance = (MenuScreenController)this;
		updateWarning.hideScreen();
		loginScreen.hideScreen();
		facebookWarning.hideScreen();
		mainMenu.hideScreen();
		rewardsScreen.hideScreen();
		settingsScreen.hideScreen();
		leaderboardScreen.hideScreen ();
		if(STATS.showRewards){
			if(STATS.canEarnRewards){
				loadingScreen.hideScreen();
				showRewards();
			}else{
				showMenu();
			}
		}else{

		}
		STATS.showRewards = false;
		loaded = true;
		updateSound ();
	}
	private void resetMenuPositions(){
		resetMenuPos (leaderboardScreen.gameObject);
		resetMenuPos (updateWarning.gameObject);
		resetMenuPos (loadingScreen.gameObject);
		resetMenuPos (rewardsScreen.gameObject);
		resetMenuPos (settingsScreen.gameObject);
		resetMenuPos (mainMenu.gameObject);
		resetMenuPos (facebookWarning.gameObject);
		resetMenuPos (loginScreen.gameObject);
	}
	private void resetMenuPos(GameObject obj){
		obj.transform.position = Vector3.zero;
	}
	// Update is called once per frame
	void Update () {
	
	}
	public void hideAll(){
		updateWarning.hideScreen();
		loginScreen.hideScreen();
		facebookWarning.hideScreen();
		mainMenu.hideScreen();
		rewardsScreen.hideScreen();
		settingsScreen.hideScreen();
		leaderboardScreen.hideScreen ();
		loadingScreen.hideScreen ();
	}
	public void showLogin(){
		loginScreen.showScreen();

		loadingScreen.hideScreen();
	}
	public void showFacebookWarning(){
		loginScreen.disableScreen();
		facebookWarning.showScreen();
	}
	public void hideFacebookWarning(){
		loginScreen.showScreen();
		facebookWarning.hideScreen();
	}
	public void showMenu(){
		hideAll ();
		mainMenu.showScreen();
	}
	public void showRewards(){
		rewardsScreen.showScreen();
		mainMenu.hideScreen();
		loadingScreen.hideScreen ();
		loginScreen.hideScreen ();
	}
	public void hideRewards(){
		rewardsScreen.hideScreen();
		mainMenu.showScreen();
	}
	public void showLeaderboard(){
		leaderboardScreen.showScreen();
		mainMenu.hideScreen();
		loadingScreen.hideScreen ();
		loginScreen.hideScreen ();
	}
	public void hideLeaderboard(){
		leaderboardScreen.hideScreen();
		mainMenu.showScreen();
	}
	public void showSettings(){
		settingsScreen.showScreen();
		mainMenu.hideScreen();
	}
	public void hideSettings(){
		facebookWarning.hideScreen();
		settingsScreen.hideScreen();
	}
	public void showUpdateWarning(){
		updateWarning.showScreen();
		loadingScreen.hideScreen();
	}
	public void hideUpdateWarning(){
		updateWarning.hideScreen();
		if(FB.IsLoggedIn){
			showMenu();
		}else{
			showLogin();
		}
	}
	public void playGame(){
		loadingScreen.showScreen();
		Application.LoadLevel("Game");
	}
	public void playTutorial(){
		loadingScreen.showScreen();
		Application.LoadLevel("Tutorial");
	}
	
	public void updateSound(){
		if (STATS.soundOn) {
			music.Play ();
		}else{
			music.Pause();
		}
	}
}
