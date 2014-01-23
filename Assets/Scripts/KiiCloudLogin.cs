using UnityEngine;
using System.Collections;
using KiiCorp.Cloud.Storage;
using KiiCorp.Cloud.Gaming;
using System;

public class KiiCloudLogin : MonoBehaviour {

    private string username = "";
    private string password = "";
	private KiiUser user = null;
	private bool OnCallback = false;

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {
 
    }

    void OnGUI () {
		if (OnCallback)
			GUI.enabled = false;
		else
			GUI.enabled = true;

        GUILayout.BeginArea (new Rect (0, 0, Screen.width, Screen.height));
        GUILayout.FlexibleSpace ();
        GUILayout.BeginHorizontal ();
        GUILayout.FlexibleSpace ();
        GUILayout.BeginVertical ();

        GUILayout.Label ("Username");
        username = GUILayout.TextField (username, GUILayout.MinWidth (200));
        GUILayout.Space (10);
        GUILayout.Label ("Password");
        password = GUILayout.PasswordField (password, '*', GUILayout.MinWidth (100));
        GUILayout.Space (30);

        if (GUILayout.Button ("Login", GUILayout.MinHeight (50), GUILayout.MinWidth (100))) {
			if( username.Length == 0 || password.Length == 0 )
				Debug.Log ("Username/password can't be empty");
			else {
				ScoreManager.clearLocalScore();
            	login ();
			}
        }

        if (GUILayout.Button ("Register", GUILayout.MinHeight (50), GUILayout.MinWidth (100))) {
			if( username.Length == 0 || password.Length == 0 )
				Debug.Log ("Username/password can't be empty");
			else {
				ScoreManager.clearLocalScore();
            	register ();
			}
        }

		if (user != null) {
			OnCallback = false;
			ScoreManager.getHighScore ();
			Application.LoadLevel ("3_GameMain");
		}

        if (GUILayout.Button ("Cancel", GUILayout.MinHeight (50), GUILayout.MinWidth (100))) {
            Application.LoadLevel ("1_GameTitle");
        }
        GUILayout.EndVertical ();
        GUILayout.FlexibleSpace ();
        GUILayout.EndHorizontal ();
        GUILayout.FlexibleSpace ();
        GUILayout.EndArea ();
    }


	private void login () {
		user = null;
		OnCallback = true;
		KiiUser.LogIn(username, password, (KiiUser user2, Exception e) => {
			if (e == null) {
				Debug.Log ("Login completed");
				user = user2;
				SaveGamingMetadata ();
			} else {
				user = null;
				OnCallback = false;
				Debug.Log ("Login failed : " + e.ToString());
			}
		});
	}
	
	private void register () {
		user = null;
		OnCallback = true;
		KiiUser built_user = KiiUser.BuilderWithName (username).Build ();
		built_user.Register(password, (KiiUser user2, Exception e) => {
			if (e == null)
			{
				user = user2;
				Debug.Log ("Register completed");
			} else {
				user = null;
				OnCallback = false;
				Debug.Log ("Register failed : " + e.ToString());
			}

		});
	}

	private void SaveGamingMetadata() {
		KiiUtils.LogFunction = Debug.Log;
		Debug.Log("Building Achievement Metadata");
		AchievementMetadata badge1 = new AchievementMetadata("ice_breaker");
		try{
			badge1.LoadLatest();
		}
		catch(ObjectNotFoundException){
			badge1.Name = "Ice Breaker Achievement";
			badge1.Description = "Awarded when player scores for the first time";
			badge1.Save();
		}

		AchievementMetadata badge2 = new AchievementMetadata("half_scorer");
		try{
			badge2.LoadLatest();
		}
		catch(ObjectNotFoundException){
			badge2.Name = "Half Score Achievement";
			badge2.SetIncremental(20);
			badge2.Description = "Awarded when player reaches half the total score of the game";
			badge2.Save();
		}

		Debug.Log("Building Leaderboard Metadata");
		LeaderboardMetadata leaderboard1 = new LeaderboardMetadata("user_points");
		try{
			leaderboard1.LoadLatest();
		}
		catch(ObjectNotFoundException){
			leaderboard1.Name = "User Best Scores (not shared)";
			leaderboard1.Description = "Aggregates scores of single user";
			leaderboard1.Save();
		}

		LeaderboardMetadata leaderboard2 = new LeaderboardMetadata("user_time");
		try{
			leaderboard2.LoadLatest();
		}
		catch(ObjectNotFoundException){
			leaderboard2.Name = "User Best Times (not shared)";
			leaderboard2.Description = "Aggregates best completion times of single user";
			leaderboard2.Save();
		}

	}

}
