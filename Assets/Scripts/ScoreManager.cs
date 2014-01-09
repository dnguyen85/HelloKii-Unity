using KiiCorp.Cloud.Storage;
using UnityEngine;
using System;

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
			Debug.Log ("Query succeeded");
			return 0;
		} catch (CloudException e) {
			Debug.Log ("Failed to query " + e.ToString());
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
		if (breakIce){
			// award ice_breaker badge (1st score in current game)
			Achievement icebBreaker = new Achievement("ice_breaker");
			Debug.Log("Loading ice_breaker achievement");
			icebBreaker.Load();
			//if(!icebBreaker.IsUnlocked()){
				icebBreaker.Unlock();
				Debug.Log("Congrats, you have unlocked " + icebBreaker.AchievementData.Name + ". " + icebBreaker.AchievementData.Description);
				icebBreaker.Save();
			//}
			breakIce = false;
		}
		// increment half_scorer badge 
		Achievement halfScorer = new Achievement("half_scorer");
		Debug.Log("Loading half_scorer achievement");
		halfScorer.Load();

		//if(!halfScorer.IsUnlocked()){
			Debug.Log("Steps before increment: "+halfScorer.CurrentSteps.ToString());
			halfScorer.Increment();
			Debug.Log("Steps after increment: "+halfScorer.CurrentSteps.ToString());
			if(halfScorer.IsUnlocked()){
				Debug.Log("Congrats, you have unlocked " + halfScorer.AchievementData.Name + ". " + halfScorer.AchievementData.Description);
			} else {
				Debug.Log("Done "+ halfScorer.CurrentSteps +" step/s. You have " + halfScorer.PercentCompleted().ToString() + "% of " + halfScorer.AchievementData.Name);
			}
			halfScorer.Save();
		//}
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
