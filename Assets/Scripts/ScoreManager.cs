using KiiCorp.Cloud.Storage;
using UnityEngine;

public class ScoreManager {

    private static string BUCKET_NAME = "BreakoutHighScore";
    private static string SCORE_KEY = "score";
    private static int cachedHighScore = 0;
    private static int currentScore = 0;

    public static void sendHighScore (int score) {
        if (KiiUser.CurrentUser == null || cachedHighScore > score) {
            return;
        }

        KiiUser user = KiiUser.CurrentUser;
        KiiBucket bucket = user.Bucket (BUCKET_NAME);
        KiiObject kiiObj = bucket.NewKiiObject ();
        kiiObj [SCORE_KEY] = score;
        try {
            kiiObj.Save ();
			Debug.Log("High score sent");
        } catch (CloudException e) {
			Debug.LogError(e.ToString());
        }
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
            return 0;
        } catch (CloudException e) {
			Debug.LogError(e.ToString());
            return 0;
        }
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
    }

    public static int getCurrentScore () {
        return currentScore;
    }

    public static void initCurrentScore () {
        currentScore = 0;
    }
}
