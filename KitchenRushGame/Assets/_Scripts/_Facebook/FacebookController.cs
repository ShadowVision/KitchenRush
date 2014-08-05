using UnityEngine;
using System.Collections;
using System.Threading.Tasks;
using Parse;
using System.Collections.Generic;
using Facebook.MiniJSON;
using System;

public class FacebookController : MonoBehaviour {
	static public FacebookController instance;
	public string listOfLoginPermissions = "email,publish_actions";
	public SpriteRenderer profilePic;
	public Texture2D defaultPic;
	public TextMesh debugText;

	private ParseObject appData;
	private bool authorizedToGainRewards = false;
	private ParseUser user;
	private bool parseLoaded = false;
	private bool gameStarted = false;
	private bool facebookLoaded = false;
	private int lastScore = 0;
	private int numberOfHighscores = 0;
	private HighscoreDelegate savedHighscoreFunction;
	private string highscoreName = "";
	private int highestScore = 0;
	
	// Use this for initialization
	void Start () {
	}
	public void Awake(){
		parseLoaded = false;
		gameStarted = false;
		facebookLoaded = false;
		instance = (FacebookController)this;
		defaultPic = profilePic.sprite.texture;
	}
	public void init(){
		if(!facebookLoaded){
			initFacebook ();
		}else{
			onInitComplete();
		}
		if(!parseLoaded){
			initParse();
		}


	}
	private void initFacebook(){
		//init facebook plugin
		FB.Init (onInitComplete, onHideUnity);
	}
	private void initParse(){
		//Load in app data from database
		ParseQuery<ParseObject> query = ParseObject.GetQuery("AppData");
		query.GetAsync("ALz0eVCO7C").ContinueWith(t => {
			appData = t.Result;
			parseLoaded = true;
		});

		//Run Parse test
		/*ParseObject testObject = new ParseObject("TestObject");
		testObject["whats"] = "up";
		testObject.SaveAsync();*/
	}
	private void loadAppData(){
		//check if this is the latest version of the app
		STATS.appDataLoaded = true;
		int appVersion = appData.Get<int>("appVersion");
		if(STATS.currentAppVersion < appVersion){
			//need to install the new version of the app
			STATS.canEarnRewards = false;
			MenuScreenController.instance.showUpdateWarning();
		}else{
			//we have the most up to date version
			STATS.canEarnRewards = true;
		}
	}
	private void loadMenu(){
		if(FB.IsLoggedIn){
			MenuScreenController.instance.showMenu();
		}else{
			MenuScreenController.instance.showLogin();
		}
	}
	private void Update(){
		if(!gameStarted && parseLoaded){
			if(STATS.appDataLoaded == false){
				loadAppData();
			}
			/*else{
				loadMenu();
			}*/
			gameStarted = true;
		}
	}

	private void onInitComplete(){
		Debug.Log ("finished initalizing facebook");
		facebookLoaded = true;
		if (FB.IsLoggedIn) {
			facebookUserLoggedIn();
		}
		loadMenu ();
	}
	private void onHideUnity(bool isGameShown){
		
	}
	public void logout(){
		if(FB.IsLoggedIn){
			FB.Logout();
			profilePic.sprite = Sprite.Create(defaultPic,new Rect(0,0,150,150),Vector2.zero);
		}
	}
	public void login(){
		if(facebookLoaded){
			if(STATS.canEarnRewards){
				authorizedToGainRewards = true;
			}
			STATS.canEarnRewards = false;

			if(FB.IsLoggedIn){
				onLogin(null);
			}else{
				FB.Login(listOfLoginPermissions,onLogin);
			}
		}
	}
	private void onLogin(FBResult result){
		if(FB.IsLoggedIn){
			facebookUserLoggedIn();
		}
		//loginParseUser();
	}
	private void facebookUserLoggedIn(){
		
		Debug.Log("Facebook user logged in");
		StartCoroutine("loginParseUser");
		loadUser ();
	}
	private void loadUser(){
		//debugText.text = "User: " + FB.UserId;
		requestUserPic();
		successfulLogin();
	}
	private IEnumerator loginParseUser(){
		//Task<ParseUser> logInTask = ParseFacebookUtils.LogInAsync(userId, accessToken, tokenExpiration);
		Task<ParseUser> loginTask = ParseFacebookUtils.LogInAsync(FB.UserId, FB.AccessToken, FB.AccessTokenExpiresAt);
		while(!loginTask.IsCompleted){
			yield return new WaitForSeconds(1);
		}
		Debug.Log("parse user result: " +  loginTask.Result.Username);
		
		user = ParseUser.CurrentUser;
		try{
			user.Add("orbs",15);
			user.Add ("rewards", new bool[6]{false,false,false,false,false,false});
			//user.Add ("name", loginTask.Result.Username);

			//Create their highscore entry
			FB.API("me?fields=name,id",Facebook.HttpMethod.GET,nameLoaded);
	
			user.SaveAsync();
			Debug.Log("setup orbs");
		}catch(ParseException e){
			Debug.Log("Already contains orbs: " + e.Message);
		}


		yield return null;
	}
	private void requestUserPic(){
		FB.API(Util.GetPictureURL("me", 150, 150), Facebook.HttpMethod.GET, loadUserPicture);
	}
	private void loadUserPicture(FBResult result){
		if (result.Error != null)                                                                                                  
		{                                                                                                                          
			FbDebug.Error(result.Error);                                                                                           
			// Let's just try again                                                                   
			Invoke("requestUserPic",5f);
			return;                                                                                                                
		} 
		Sprite newSprite = Sprite.Create(result.Texture,new Rect(0,0,150,150),Vector2.zero);
		if(profilePic!= null){
			profilePic.sprite = newSprite;
		}
	}
	private void successfulLogin(){
		//if(authorizedToGainRewards){
			STATS.canEarnRewards = true;
		//}
		MenuScreenController.instance.showMenu();
	}

	private void updateUser(){
		user = ParseUser.CurrentUser;

	}
	public void saveScore(int amount){
		Debug.Log ("trying to save score: " + amount);
		lastScore = amount;
		finishSavingHighscore ();
	}
	private string protectName(string fullName){
		string[] fullNameSplit = fullName.Split (' ');
		string finalName = "";
		for(int i=0; i<fullNameSplit.Length; i++){
			if(i != fullNameSplit.Length-1){
				finalName += fullNameSplit[i] + " ";
			}else{
				finalName += fullNameSplit[i][0] + ".";
			}
		}
		return finalName;
	}
	private void nameLoaded(FBResult result){
		Dictionary<string,object> r = Json.Deserialize (result.Text) as Dictionary<string,object>;
		saveFirstHighscore (protectName (r ["name"] as String));
	}
	private void saveFirstHighscore(string username){
		ParseObject highscore = new ParseObject("Highscore"); 
		highscore["score"] = 0;
		highscore["name"] = username;
		highscore["nameId"] = FB.UserId;
		highscore.SaveAsync();
	}
	private void finishSavingHighscore(){
		//having each score stored in a highscore object
		/*ParseObject obj = new ParseObject("Highscore");
		obj["score"] = lastScore;
		obj["name"] = protectName(r["name"] as String);
		obj ["nameId"] = r["id"];
		obj.SaveAsync ();*/

		//having highscore stored in User
		/*
		user["name"] = highscoreName;
		user["score"] = highestScore;
		user.SaveAsync ();
		Debug.Log("saving score: " + highestScore);*/

		//having only one highscore object stored
		ParseQuery<ParseObject> query = ParseObject.GetQuery("Highscore")
			.WhereMatches("nameId", FB.UserId,null);
		query.FirstAsync().ContinueWith(t => {
			ParseObject highscore = t.Result;

			highestScore = 0;
			int oldHighestScore = 0;
			highscore.TryGetValue("score", out oldHighestScore);
			
			if(lastScore > oldHighestScore){
				highestScore = lastScore;
			}else{
				highestScore = oldHighestScore;
			}

			highscore["score"] = highestScore;
			highscore.SaveAsync ();
		});
	}
	public void addOrbs(int amount){
		if (user != null) {
			int originalAmount = -1;
			user.TryGetValue ("orbs", out originalAmount);
			if (originalAmount != -1) {
				user ["orbs"] = originalAmount + amount;
				user.SaveAsync ();
			}
		}
	}
	public int getOrbs(){
		int orbs = 0;
		if (user != null) {
			updateUser ();
			user.TryGetValue ("orbs", out orbs);
		}
		return orbs;
	}
	public bool spendOrbs(int amount){
		bool canSpend = false;
		int orbs = 0;
		orbs = getOrbs();
		if(orbs-amount >=0){
			orbs-= amount;
			user["orbs"] = orbs;
			user.SaveAsync();
			canSpend = true;
		}
		STATS.orbs = orbs;
		return canSpend;
	}
	public bool[] getRewards(){
		updateUser();
		bool[] r = new bool[6];
		IList<bool> rewards;
		user.TryGetValue("rewards",out rewards);
		rewards.CopyTo(r,0);
		STATS.rewardsEarned = r;
		return r;
	}
	public void setReward(int i, bool purchased, string name){
		setReward (i, purchased);
		//Save reward stub
		ParseObject obj = new ParseObject("Receipts");
		obj["foodType"] = name;
		obj.SaveAsync ();
	}
	public void setReward(int i, bool purchased){
		updateUser();
		IList<bool> rewards;
		user.TryGetValue("rewards",out rewards);
		rewards[i] = purchased;
		user["rewards"] = rewards;
		user.SaveAsync();
		getRewards();
	}
	public string loadRedeemPassword(){
		string code = "Sage";
		appData.TryGetValue ("redeemCode", out code);
		return code;
	}

	public void share(int score){
		FB.Feed ("", "https://apps.facebook.com/departurekitchenrush", "Let's Play Kitchen Rush!", "Play video games for free food",
		         "I just scored " + score + " points! Redeem points for food at Departure Restaurant + Lounge!",
		         "http://libonatidesign.com/Storage/Departure/SushiCrush/KitchenRush_FacebookShareIcon.jpg", "", "", "", "", null,
		        shareCallback);
	}
	void shareCallback(FBResult response) {
		Debug.Log(response.Text);
	}

	public delegate void HighscoreDelegate(IEnumerable<ParseObject> results);
	public enum HighscoreType
		{
			TOPTEN,TOPTEN_RECENT,TOPTEN_FRIENDS
		}
	public void getHighscoreData(int numberOfHighscores, HighscoreType highscoreType, HighscoreDelegate resultsFunction){
		ParseQuery<ParseObject> query = null;
		this.numberOfHighscores = numberOfHighscores;
		switch (highscoreType) {
		case HighscoreType.TOPTEN:
			query = ParseObject.GetQuery ("Highscore")
				.Limit(numberOfHighscores)
					.OrderByDescending("score");
			
			query.FindAsync().ContinueWith( t => {
				resultsFunction(t.Result);
			});
			break;
		case HighscoreType.TOPTEN_RECENT:
			string date = DateTime.Now.Year+"-"
							+DateTime.Now.Month+"-"
								+"00T00:00:00Z";
			
			Debug.Log("Getting scores from after: " + date);

			DateTime now = DateTime.Now;
			DateTime time = new DateTime(now.Year,now.Month,1,12,0,0);

			query = ParseObject.GetQuery ("Highscore")
				.Limit(numberOfHighscores)
					.WhereGreaterThan("updatedAt", time)
						.OrderByDescending("score");

			
			query.FindAsync().ContinueWith( t => {
				resultsFunction(t.Result);
			});
			break;
		case HighscoreType.TOPTEN_FRIENDS:
			savedHighscoreFunction = resultsFunction;
			loadFriends();
			break;

		default:
			query = ParseObject.GetQuery ("Highscore")
				.Limit(numberOfHighscores)
					.OrderByDescending("score");
			
			query.FindAsync().ContinueWith( t => {
				resultsFunction(t.Result);
			});
			break;
		}

	}

	private void loadFriends(){
		FB.API("me/friends",Facebook.HttpMethod.GET,friendsLoaded);
	}
	private void friendsLoaded(FBResult result){
		//Compile all ffriend ID's into a string array
		Dictionary<string,object> json = Json.Deserialize (result.Text) as Dictionary<string,object>;
		List <object> names = json ["data"] as List<object>;
		string[] friendIds = new string[names.Count];
		int i = 0;
		foreach(object name in names){
			friendIds[i] = (name as Dictionary<string,object>)["id"] as String;
			i++;
		}
		//continue with the highscore query
		ParseQuery<ParseObject> query = ParseObject.GetQuery ("Highscore")
			.Limit(numberOfHighscores)
				.WhereContainedIn("nameId", friendIds)
				.OrderByDescending("score");
		
		query.FindAsync().ContinueWith( t => {
			savedHighscoreFunction(t.Result);
		});
	}
}
