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
            Application.LoadLevel ("GameMain");
        }
    }

    void Awake () {
		//Broken code? Sign up on developer.kii.com and create a Unity app to get these parameters!
        Kii.Initialize ("__KII_APP_ID__", "__KII_APP_KEY__", __KII_APP_SITE__);
    }

    void OnGUI () {
        GUILayout.BeginArea (new Rect (0, 0, Screen.width, Screen.height));
        GUILayout.FlexibleSpace ();
        GUILayout.BeginHorizontal ();
        GUILayout.FlexibleSpace ();
        GUILayout.BeginVertical ();
        GUILayout.Label ("Breakout by KiiCloud", GUILayout.ExpandWidth (false));
        GUILayout.Space (20);
        GUILayout.Label ("Username : " + getCurrentUsername (), GUILayout.ExpandWidth (false));
        if (GUILayout.Button ("Login", GUILayout.MinHeight (50), GUILayout.MinWidth (100))) {
            Application.LoadLevel ("KiiCloudLogin");
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
