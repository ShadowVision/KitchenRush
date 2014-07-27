using UnityEngine;
using System.Collections;

public class Btn_Share : Btn_Base {

	override protected void OnMouseUp(){
		base.OnMouseUp ();
		if(FB.IsLoggedIn){
			FacebookController.instance.share ();
		}else{
			//STATS.showFacebookLogin = true;
			/*GameLogic.instance.loadingScreen.showScreen();
			Application.LoadLevel ("MainMenu");*/

			FacebookController.instance.login();
		}
	}
}
