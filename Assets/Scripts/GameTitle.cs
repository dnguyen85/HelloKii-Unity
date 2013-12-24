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
        Kii.Initialize ("__KII_APP_ID__", "__KII_APP_KEY__", __KII_APP_SITE__);
    }

    void OnGUI () {
        GUILayout.BeginArea (new Rect (0, 0, Screen.width, Screen.height));
        GUILayout.FlexibleSpace ();
        GUILayout.BeginHorizontal ();
        GUILayout.FlexibleSpace ();
        GUILayout.BeginVertical ();
        GUILayout.Label ("Breakout powered by KiiCloud", GUILayout.ExpandWidth (false));
        GUILayout.Space (10);
        if (GUILayout.Button ("Start!", GUILayout.MinHeight (50), GUILayout.MinWidth (100))) {
            Application.LoadLevel ("GameMain");
        }
        ;
        GUILayout.Space (50);
        GUILayout.Label ("Username : " + getCurrentUsername (), GUILayout.ExpandWidth (false));
        if (GUILayout.Button ("Login KiiCloud", GUILayout.MinHeight (50), GUILayout.MinWidth (100))) {
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
        return "Not logged in";
    }
}
