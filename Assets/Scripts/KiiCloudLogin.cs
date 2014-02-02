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

	public static Achievement iceBreaker;
	public static Achievement halfScorer;
	public static LeaderboardMetadata userPointsMeta;
	public static LeaderboardMetadata userTimeMeta;

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
				user = user2;
				SaveGamingData ();
				Debug.Log ("Login completed");
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
				SaveGamingData();
				Debug.Log ("Register completed");
			} else {
				user = null;
				OnCallback = false;
				Debug.Log ("Register failed : " + e.ToString());
			}

		});
	}

	private void SaveGamingData() {

		KiiUtils.LogFunction = Debug.Log;

		Debug.Log("Building Achievements");

		AchievementMetadata iceBreakerMeta = new AchievementMetadata("ice_breaker");
		iceBreakerMeta.LoadLatest((GamingObject<AchievementMetadata> md1, Exception e1) => {
			if(e1 != null){
				if(e1 is ObjectNotFoundException){
					iceBreakerMeta.Name = "Ice Breaker Achievement";
					iceBreakerMeta.Description = "Awarded when player scores for the first time";
					iceBreakerMeta.Save((GamingObject<AchievementMetadata> md2, Exception e2) => {
						if(e2 == null)
							Debug.Log("Saved achievement metadata: ice_breaker");
						else {
							Debug.Log("Error saving achievement metadata: ice_breaker");
							return;
						}
					});
				}
				else {
					Debug.Log("Unknown error loading achievement metadata: ice_breaker");
					return;
				}
			}
			else
				Debug.Log("Loaded latest achievement metadata: ice_breaker");
			iceBreaker = new Achievement(iceBreakerMeta);
			iceBreaker.LoadLatest((GamingObject<Achievement> d1, Exception e3) => {
				if(e3 != null){
					if(e3 is ObjectNotFoundException){
						Debug.Log("Not found. Attaching: ice_breaker");
						iceBreaker.Attach();
					}
					else
						Debug.Log("Unknown error loading achievement: ice_breaker");
				}
				else
					Debug.Log("Found achievement: ice_breaker");
			});
		});

		AchievementMetadata halfScorerMeta = new AchievementMetadata("half_scorer");
		halfScorerMeta.LoadLatest((GamingObject<AchievementMetadata> md1, Exception e1) => {
			if(e1 != null){
				if(e1 is ObjectNotFoundException){
					halfScorerMeta.Name = "Half Score Achievement";
					halfScorerMeta.SetIncremental(20);
					halfScorerMeta.Description = "Awarded when player reaches half the total score of the game";
					halfScorerMeta.Save((GamingObject<AchievementMetadata> md2, Exception e2) => {
						if(e2 == null){
							Debug.Log("Saved achievement metadata: half_scorer");
						}
						else {
							Debug.Log("Error saving achievement metadata: half_scorer");
							return;
						}
					});
				}
				else {
					Debug.Log("Unknown error loading achievement metadata: half_scorer");
					return;
				}
			}
			else
				Debug.Log("Loaded latest achievement metadata: half_scorer");
			halfScorer = new Achievement(halfScorerMeta);
			halfScorer.LoadLatest((GamingObject<Achievement> d1, Exception e3) => {
				if(e3 != null){
					if(e3 is ObjectNotFoundException){
						Debug.Log("Not found. Attaching: halfScorer");
						halfScorer.Attach();
					}
					else
						Debug.Log("Unknown error loading achievement: halfScorer");
				}
				else
					Debug.Log("Found achievement: halfScorer");
			});
		});


		Debug.Log("Building Leaderboards");

		userPointsMeta = new LeaderboardMetadata("user_points");
		userPointsMeta.LoadLatest((GamingObject<LeaderboardMetadata> md1, Exception e1) => {
			if(e1 != null){
				if(e1 is ObjectNotFoundException){
					userPointsMeta.Name = "User Best Scores (not shared)";
					userPointsMeta.Description = "Aggregates scores of single user";
					userPointsMeta.Save((GamingObject<LeaderboardMetadata> md2, Exception e2) => {
						if(e2 == null)
							Debug.Log("Saved leaderboard metadata: user_points");
						else
							Debug.Log("Error saving leaderboard metadata: user_points");
					});
				}
				else
					Debug.Log("Unknown error loading leaderboard metadata: user_points");
			}
			else
				Debug.Log("Loaded latest leaderboard metadata: user_points");
		});

		userTimeMeta = new LeaderboardMetadata("user_time");
		userTimeMeta.LoadLatest((GamingObject<LeaderboardMetadata> md1, Exception e1) => {
			if(e1 != null){
				if(e1 is ObjectNotFoundException){
					userTimeMeta.Name = "User Best Times (not shared)";
					userTimeMeta.Description = "Aggregates best completion times of single user";
					userTimeMeta.Save((GamingObject<LeaderboardMetadata> md2, Exception e2) => {
						if(e2 == null)
							Debug.Log("Saved leaderboard metadata: user_time");
						else
							Debug.Log("Error saving leaderboard metadata: user_time");
					});
				}
				else
					Debug.Log("Unknown error loading leaderboard metadata: user_time");
			}
			else
				Debug.Log("Loaded latest leaderboard metadata: user_time");
		});
	}

}
