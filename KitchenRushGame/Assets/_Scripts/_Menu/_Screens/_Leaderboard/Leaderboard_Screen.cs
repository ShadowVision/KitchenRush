using UnityEngine;
using System;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using Parse;
public class Leaderboard_Screen : Rolling_Screen {
	public Btn_LeaderBoardSection[] sectionBtns;
	public Highscore highscoreTemplate;
	public int numberOfScores = 10;

	private Highscore[] highscores;
	private IEnumerable<ParseObject> scoreResults;
	private Vector3 savedProfilPicPosition = Vector3.zero;
	private FacebookController.HighscoreType highscoreType = FacebookController.HighscoreType.TOPTEN;

	// Use this for initialization
	void Start () {
	
	}
	void FixedUpdate(){
		if(scoreResults != null){
			finishLoadingHighscores(scoreResults);
			scoreResults = null;
		}
	}

	public override void showScreen ()
	{
		base.showScreen ();
		savedProfilPicPosition = FacebookController.instance.profilePic.transform.localPosition;
		FacebookController.instance.profilePic.transform.parent = draggableElement.transform;
		selectSection (0);
	}
	public override void hideScreen ()
	{
		if(savedProfilPicPosition != Vector3.zero){
			FacebookController.instance.profilePic.transform.parent = FacebookController.instance.transform;
			FacebookController.instance.profilePic.transform.localPosition = savedProfilPicPosition;
		}
		base.hideScreen ();
	}

	public void selectSection(int i){
		foreach(Btn_LeaderBoardSection btn in sectionBtns){
			btn.deselect();
		}
		sectionBtns [i].select ();
		switch(i){
		case 0:
			selectTopTen();
			break;
		case 1:
			selectMonthlyTopTen();
			break;
		case 2: 
			selectFriendsTopTen();
			break;
		}
		createHighscores ();
	}
	
	private void selectTopTen(){
		highscoreType = FacebookController.HighscoreType.TOPTEN;
	}
	private void selectMonthlyTopTen(){
		highscoreType = FacebookController.HighscoreType.TOPTEN_RECENT;
	}
	private void selectFriendsTopTen(){
		highscoreType = FacebookController.HighscoreType.TOPTEN_FRIENDS;
	}

	private void createHighscores(){
		deleteExistingHighscores ();
		FacebookController.instance.getHighscoreData (numberOfScores, highscoreType,  OnRecievedHighscores);
	}
	private void OnRecievedHighscores(IEnumerable<ParseObject> results){
		scoreResults = results;
	}
	private void finishLoadingHighscores(IEnumerable<ParseObject> results){
		highscores = new Highscore[numberOfScores];
		Vector3 nextPosition = new Vector3(0,.8f,0);
		int i=0;
		foreach(ParseObject obj in results){
			string name = obj.Get<string>("name");
			if(name == ""){
				name = "No Name";
			}
			int score = obj.Get<int>("score");
			string date = obj.CreatedAt.Value.ToString("d", CultureInfo.InvariantCulture);
			highscores[i] = (Highscore)Instantiate(highscoreTemplate);
			highscores[i].transform.parent = draggableElement.transform;
			highscores[i].transform.localPosition = nextPosition;
			highscores[i].setLabel(i+1,name,score,date);
			nextPosition.y -=  1.3f;
			i++;
		}
	}
	private void deleteExistingHighscores(){
		if(highscores != null){
			foreach(Highscore score in highscores){
				if(score != null){
					Destroy(score.gameObject);
				}
			}
		}
	}
}
