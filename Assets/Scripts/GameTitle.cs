using UnityEngine;
using System.Collections;
using KiiCorp.Cloud.Storage;

public class GameTitle : MonoBehaviour {

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
        //Sign up on developer.kii.com and create a Unity app to get these parameters!
		//See the Assets/Readme.txt file in this project for more info
		Kii.Initialize ("__KII_APP_ID__", "__KII_APP_KEY__", __KII_APP_SITE__);; 
		//Your backend location options: Kii.Site.US, Kii.Site.JP, Kii.Site.CN
		//IMPORTANT: backend location here must match backend location configured in your app at developer.kii.com

		//Interested in Game Analytics? Get our Analytics SDK http://developer.kii.com/#/sdks
		//More info: http://documentation.kii.com/en/guides/unity/managing-analytics
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
				Application.LoadLevel ("2_KiiCloudLogin");
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
}
