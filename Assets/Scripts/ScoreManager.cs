using KiiCorp.Cloud.Storage;
using UnityEngine;
using System;
using KiiCorp.Cloud.Gaming;
using System.Collections.Generic;

public class ScoreManager {

    private static string BUCKET_NAME = "BreakoutHighScore";
    private static string SCORE_KEY = "score";
    private static int cachedHighScore = 0;
    private static int currentScore = 0;
	private static bool breakIce = true;

    public static void sendHighScore (int score) {
        if (KiiUser.CurrentUser == null || cachedHighScore > score) {
            return;
        }

        KiiUser user = KiiUser.CurrentUser;
        KiiBucket bucket = user.Bucket (BUCKET_NAME);
        KiiObject kiiObj = bucket.NewKiiObject ();
        kiiObj [SCORE_KEY] = score;

		kiiObj.Save((KiiObject obj, Exception e) => {
			if (e != null)
			{
				Debug.LogError(e.ToString());
			} else {
				Debug.Log("High score sent");
			}
		});
    }

    public static int getHighScore () {
        if (KiiUser.CurrentUser == null) {
            return 0;
        }
        if (cachedHighScore > 0) {
            return cachedHighScore;
        }

        KiiUser user = KiiUser.CurrentUser;
        KiiBucket bucket = user.Bucket (BUCKET_NAME);
        KiiQuery query = new KiiQuery ();
        query.SortByDesc (SCORE_KEY);
        query.Limit = 10;

		try {
			KiiQueryResult<KiiObject> result = bucket.Query (query);
			foreach (KiiObject obj in result) {
				int score = obj.GetInt (SCORE_KEY, 0);
				cachedHighScore = score;
				return score;
			}
			Debug.Log ("High score fetched");
			return 0;
		} catch (CloudException e) {
			Debug.Log ("Failed to fetch high score: " + e.ToString());
			return 0;
		}

		/* Async version
		bucket.Query(query, (KiiQueryResult<KiiObject> list, Exception e) =>{
			if (e != null)
			{
				Debug.Log ("Failed to query " + e.ToString());
			} else {
				Debug.Log ("Query succeeded");
				foreach (KiiObject obj in list) {
					int score = obj.GetInt (SCORE_KEY, 0);
					cachedHighScore = score;
					getHighScore = score;
					return;
				}
			}
		});
		*/
    }

    public static void refreshHighScore () {
        if (cachedHighScore < getCurrentScore ()) {
            cachedHighScore = getCurrentScore ();
        }
    }

    public static int getCachedHighScore () {
        return cachedHighScore;
    }

    public static void addCurrentScore (int add) {
        currentScore += add;

		if (breakIce) {
			// award ice_breaker badge (1st score in current game)
			Debug.Log("Processing achievement ice_breaker");
			Achievement iceBreaker = KiiCloudLogin.iceBreaker;
			Debug.Log("Unlocking achievement ice_breaker");
			iceBreaker.Unlock();
			if(iceBreaker.IsUnlocked()){
				Debug.Log("Achievement is unlocked ice_breaker");
				Debug.Log("Congrats, you have unlocked " 
			          + iceBreaker.AchievementMetadata.Name + ": " 
			         + iceBreaker.AchievementMetadata.Description
				          );
			}
			Debug.Log("Resetting ice_breaker");
			iceBreaker.Reset();
			Debug.Log("Saving ice_breaker");
			iceBreaker.Save((GamingObject<Achievement> ac, Exception e) => {
				if(e != null)
					Debug.Log("Failed to save ice_breaker");
			});
			breakIce = false;
		}

		// Increment half_scorer badge
		Achievement halfScorer = KiiCloudLogin.halfScorer;

		if(!halfScorer.IsUnlocked()){
			halfScorer.Increment();
			if(halfScorer.IsUnlocked()){
				Debug.Log("Congrats, you have unlocked " 
			          + halfScorer.AchievementMetadata.Name + ". " 
			          + halfScorer.AchievementMetadata.Description);
			} else {
				Debug.Log("Done "+ halfScorer.CurrentSteps +" step/s. You have " 
				          + halfScorer.PercentCompleted().ToString("0.00") 
			          + "% of " + halfScorer.AchievementMetadata.Name);
			}
		} else {
			Debug.Log("Resetting half_scorer");
			halfScorer.Reset();
		}
		Debug.Log("Saving half_scorer");
		halfScorer.Save((GamingObject<Achievement> ac, Exception e) => {
			if(e != null)
				Debug.Log("Failed to save half_scorer");
		});
    }

    public static int getCurrentScore () {
        return currentScore;
    }

    public static void initCurrentScore () {
        currentScore = 0;
    }

	public static void clearLocalScore(){
		cachedHighScore = 0;
		currentScore = 0;
	}
}
