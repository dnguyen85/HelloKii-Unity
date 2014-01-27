using UnityEngine;
using System.Collections;
using KiiCorp.Cloud.Storage;
using System.Text;
using JsonOrg;
using System;

public class GameTitle : MonoBehaviour {

	private const string mAppId = "a8ce3753";
	private const string mAppKey = "7eac5fd9138f754adef2c84145c045dd";
	private const string mFacebookAppId = "532595163514897";
	private const string mFacebookToken = "CCAAHkZAHmHyBEBAHURnZCZAmluXHQfU2awfsGXC6w4uZCo6u3jlP9qCQ7seGhOqBagR4HZAbjuqQ1AlZAw8EH2OACKzQ8arKihA64Udn4NU3QYESR01RAGnxb8NntLIdzz0gRucBq1nJyD89RVqIJHZB3yPm2ZBrAA80IkOZCeT5ZCZCTfz8mD2UG5H1xzu9uA7oKZBsZD";
	
    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetButton ("Jump")) {
            Application.LoadLevel ("3_GameMain");
        }
    }

    void Awake () {
		KiiLogger.LogFunction = Debug.Log;
		KiiLogger.Log("Plugged in logger");
        //Sign up on developer.kii.com and create a Unity app to get these parameters!
		//See the Assets/Readme.txt file in this project for more info
		//Kii.Initialize ("__KII_APP_ID__", "__KII_APP_KEY__", __KII_APP_SITE__);
		Kii.Initialize (mAppId, mAppKey, Kii.Site.US);
		//Your backend location options: Kii.Site.US, Kii.Site.JP, Kii.Site.CN
		//IMPORTANT: backend location here must match backend location configured in your app at developer.kii.com

		//Interested in Game Analytics? Get our Analytics SDK http://developer.kii.com/#/sdks
		//More info: http://documentation.kii.com/en/guides/unity/managing-analytics

		// Initialize FB SDK              
		enabled = false;                  
		FB.Init(SetInit, OnHideUnity);
	}
	
    void OnGUI () {
		GUIStyle style = GUI.skin.GetStyle("Label");
		GUILayout.BeginArea (new Rect (0, 0, Screen.width, Screen.height));
        GUILayout.FlexibleSpace ();
        GUILayout.BeginHorizontal ();
        GUILayout.FlexibleSpace ();
        GUILayout.BeginVertical ();
		GUILayout.Label ("<size=35>Breakout by Kii</size>", style, GUILayout.ExpandWidth (false));
		// Don't replace the parameters below please!! Replace in the Awake() method
		if (Kii.AppId == null || Kii.AppKey == null || Kii.AppId.Equals ("__KII_APP_ID__") || Kii.AppKey.Equals ("__KII_APP_KEY__")) {
			GUILayout.Space (10);
			GUILayout.Label ("Invalid API keys. See Assets/Readme.txt", GUILayout.ExpandWidth (false));
			GUILayout.Space (20);
			if (GUILayout.Button ("Get API Keys", GUILayout.MinHeight (50), GUILayout.MinWidth (100))) {
				Application.OpenURL("http://developer.kii.com");
			}
		} else {
			GUILayout.Space (20);
			GUILayout.Label ("Username : " + getCurrentUsername (), GUILayout.ExpandWidth (false));
			if (GUILayout.Button ("Login", GUILayout.MinHeight (50), GUILayout.MinWidth (100))) {
				//Application.LoadLevel ("2_KiiCloudLogin");
				FB.Login("email,publish_actions", LoginCallback);
				//login ();
			}	
		}

        GUILayout.EndVertical ();
        GUILayout.FlexibleSpace ();
        GUILayout.EndHorizontal ();
        GUILayout.FlexibleSpace ();
        GUILayout.EndArea ();
    }

    private string getCurrentUsername () {
        KiiUser user = KiiUser.CurrentUser;
        if (user != null) {
            return user.Username;
        }
        return "No user";
    }

	void LoginCallback(FBResult result)                                                        
	{                                                                                          
		FbDebug.Log("On FB login callback"); 
		
		if (FB.IsLoggedIn)                                                                     
		{                                                                                      
			OnLoggedIn();                                                                      
		}                                                                                      
	} 

	void login(){
		Debug.Log ("Attempting Kii signin with Facebook token");
		try{
			KiiUser user = KiiUser.LoginWithFacebookToken(mFacebookToken);
			
			// Update the user attributes from Facebook data
			KiiUser.ChangeEmail("user_123456@example.com");
			KiiUser.ChangePhone("+919012345678");
			user.Displayname = "Your user display name";
			user.Country = "US";
			user.Update();
			Debug.Log ("Current user name is: " + KiiUser.CurrentUser.Username);
			Debug.Log ("Current user name is: " + KiiUser.CurrentUser.Displayname);
			Debug.Log ("Current user email is: " + KiiUser.CurrentUser.Email);
			Debug.Log ("Current user phone is: " + KiiUser.CurrentUser.Phone);
		} catch(Exception e){
			Debug.Log(e.GetType().ToString());
			Debug.Log (e.StackTrace);
		}
		Application.LoadLevel ("3_GameMain");
	}
	
	void OnLoggedIn()                                                                          
	{                
		Debug.Log("Logged in. ID: " + FB.UserId);
		Debug.Log ("Access token: " + FB.AccessToken);
		//FbDebug.Log("Logged in ID: " + FB.UserId);
		Debug.Log ("Attempting Kii signin with Facebook token");
		try{
			KiiUser user = KiiUser.LoginWithFacebookToken(FB.AccessToken);

			// Update the user attributes from Facebook data
			KiiUser.ChangeEmail("user_123456@example.com");
			KiiUser.ChangePhone("+919012345678");
			user.Displayname = "Your user display name";
			user.Country = "US";
			user.Update();
			Debug.Log ("Current user name is: " + KiiUser.CurrentUser.Username);
			Debug.Log ("Current user name is: " + KiiUser.CurrentUser.Displayname);
			Debug.Log ("Current user email is: " + KiiUser.CurrentUser.Email);
			Debug.Log ("Current user phone is: " + KiiUser.CurrentUser.Phone);
		} catch(Exception e){
			Debug.Log(e.GetType().ToString());
			Debug.Log (e.StackTrace);
		}
		Application.LoadLevel ("3_GameMain");
	}               

	private void SetInit()                                                                       
	{                                                                                            
		FbDebug.Log("SetInit");                                                                  
		enabled = true; // "enabled" is a property inherited from MonoBehaviour                  
		if (FB.IsLoggedIn)                                                                       
		{                                                                                        
			FbDebug.Log("Already logged in");                                                    
			OnLoggedIn();                                                                        
		}                                                                                        
	}                                                                                            
	
	private void OnHideUnity(bool isGameShown)                                                   
	{                                                                                            
		FbDebug.Log("OnHideUnity");                                                              
		if (!isGameShown)                                                                        
		{                                                                                        
			// pause the game - we will need to hide                                             
			Time.timeScale = 0;                                                                  
		}                                                                                        
		else                                                                                     
		{                                                                                        
			// start the game back up - we're getting focus again                                
			Time.timeScale = 1;                                                                  
		}                                                                                        
	}

}
