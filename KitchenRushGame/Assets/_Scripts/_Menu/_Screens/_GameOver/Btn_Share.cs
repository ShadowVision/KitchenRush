using UnityEngine;
using System.Collections;

public class Btn_Share : Btn_Base {
	public int score = 0;
	override protected void OnMouseUp(){
		base.OnMouseUp ();
		if(FB.IsLoggedIn){
			FacebookController.instance.share (score);
		}else{
			//STATS.showFacebookLogin = true;
			/*GameLogic.instance.loadingScreen.showScreen();
			Application.LoadLevel ("MainMenu");*/

			FacebookController.instance.login();
		}
	}
}
