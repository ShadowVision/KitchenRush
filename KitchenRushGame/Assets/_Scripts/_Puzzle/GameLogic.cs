using UnityEngine;
using System.Collections;

public class GameLogic : MonoBehaviour {
	static public GameLogic instance;
	private bool gameStarted = false;

	public bool startOnAwake = true;
	//TIMER
	public Timer timer;
	public float lengthOfTimerInSeconds = 60;
	//OBJECTIVE
	public Objective objectiveTemplate;
	public Transform objectiveSpawnPoint;
	public int minTilesNeeded = 20;
	public int maxTilesNeeded = 40;
	//SCORE
	public Score score;
	public int[] orbThresholds;

	private Objective currentObjective;
	private Board board;

	//GameOver
	public GameOverScreen gameOverScreenTemplate;
	public Menu_Screen loadingScreen;
	private bool gameEnded = false;

	//Pause Screen
	public Menu_Screen pauseScreen;
	public bool isPaused = false;

	public AudioSource music;

	// Use this for initialization
	void Start () {
		board = gameObject.GetComponent<Board>();
		loadingScreen.hideScreen();
		pauseScreen.hideScreen();
		updateSound ();

		if(startOnAwake){
			board.buildBoard();
			newObjective();
		}
	}
	void Awake(){	
		instance = (GameLogic)this;
	}
	public void startGame(){
		if(!gameStarted){
			gameStarted = true;
			timer.startTimer(lengthOfTimerInSeconds,onTimerCompletion);
			music.Play ();
		}
	}
	public void onTimerCompletion(){
		endGame ();
	}
	// Update is called once per frame
	void Update () {
	
	}
	public void completeObjective(Objective obj){
		newObjective();
	}
	public void newObjective(){
		Tile.TileType lastType = Tile.TileType.NONE;
		if(currentObjective != null){
			lastType = currentObjective.tileType;
			Destroy(currentObjective.gameObject);
		}
		currentObjective = (Objective)Instantiate(objectiveTemplate);
		currentObjective.numberOfTilesNeeded = Random.Range(minTilesNeeded,maxTilesNeeded);
		//Get a tile type until it is not the same as the last tile type
		Tile newTile = null;
		do{
			if(newTile != null){
				Destroy(newTile.gameObject);
			}
			newTile = (Tile)Instantiate(board.getTileTemplate());
		}
		while(newTile.type == lastType);

		newTile.transform.parent = currentObjective.transform;
		newTile.transform.localPosition = Vector3.zero;
		newTile.transform.localScale = new Vector3(.7f,.7f,.7f);
		currentObjective.tileType = newTile.type;
		currentObjective.transform.parent = objectiveSpawnPoint;
		currentObjective.transform.localPosition = Vector3.zero;
		Destroy(newTile);
	}
	public void endGame(){
		if (!gameEnded) {
			music.Stop();
			SoundFiles.playGameOver();
			gameEnded = true;
			//Turn off all colliders
			Collider2D[] colliders = FindObjectsOfType<Collider2D> ();
			foreach (Collider2D col in colliders) {
				col.enabled = false;
			}
			//figure out how many stars they get
			int orbs = 0;
			for (int i=0; i<orbThresholds.Length; i++) {
				if (score.points >= orbThresholds [i]) {
					orbs++;
				}
			}
			if (orbs > 1) {
				AnimationController.instance.playAnimation (AnimationController.AnimationType.WIN);
			} else {
				AnimationController.instance.playAnimation (AnimationController.AnimationType.LOSE);
			}
			GameOverScreen go = (GameOverScreen)Instantiate (gameOverScreenTemplate, Vector3.zero, Quaternion.identity);
			go.setScore (score.points, orbs);
			if (FacebookController.instance != null) {
				FacebookController.instance.addOrbs (orbs);
				FacebookController.instance.saveScore(score.points);
			}
		}
	}
	public void pauseGame(){
		isPaused = true;
		board.pauseInteraction(0);
		pauseScreen.showScreen();
		music.Pause ();
	}
	public void resumeGame(){
		isPaused = false;
		pauseScreen.hideScreen();
		board.resetInteraction();
		music.Play ();
	}

	public void updateSound(){
		if (STATS.soundOn) {
			music.volume = 1;
		}else{
			music.volume = 0;
		}
	}
}
