using UnityEngine;
using System.Collections;
using KiiCorp.Cloud.Storage;
using System;

public class KiiCloudLogin : MonoBehaviour {

    private string username = "";
    private string password = "";

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {
 
    }

    void OnGUI () {
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
			else
            	login ();
        }

        if (GUILayout.Button ("Register", GUILayout.MinHeight (50), GUILayout.MinWidth (100))) {
			if( username.Length == 0 || password.Length == 0 )
				Debug.Log ("Username/password can't be empty");
			else
            	register ();
        }

        if (GUILayout.Button ("Cancel", GUILayout.MinHeight (50), GUILayout.MinWidth (100))) {
            Application.LoadLevel ("GameTitle");
        }
        GUILayout.EndVertical ();
        GUILayout.FlexibleSpace ();
        GUILayout.EndHorizontal ();
        GUILayout.FlexibleSpace ();
        GUILayout.EndArea ();
    }

	private void login () {
		Action<string> callback = delegate(string s) {
			loginCallback (s);};
		StartCoroutine (loginBlocking (callback));
	}
	
	private void loginCallback (string errorMessage) {
		if (errorMessage == null) {
			Debug.Log ("Login completed");
			ScoreManager.getHighScore();
			Application.LoadLevel ("GameMain");
		} else {
			Debug.Log ("Login failed : " + errorMessage);
		}
	}
	
	IEnumerator loginBlocking (Action<string> callback) {
		string errText = null;
		try {
			Debug.Log ("User : " + username + " / Pass : " + password);
			KiiUser.LogIn (username, password);
		} catch (CloudException e) {
			errText = e.Message;
		}
		yield return null;
		callback (errText);
		yield return null;
	}
	
	private void register () {
		Action<string> callback = delegate(string s) {
			registerCallback (s);};
		StartCoroutine (registerBlocking (callback));
	}
	
	private void registerCallback (string errorMessage) {
		if (errorMessage == null) {
			Debug.Log ("Register completed");
			Application.LoadLevel ("GameMain");
		} else {
			Debug.Log ("Register failed : " + errorMessage);
		}
	}
	
	IEnumerator registerBlocking (Action<string> callback) {
		string errText = null;
		KiiUser user = KiiUser.BuilderWithName (username).Build ();
		try {
			Debug.Log ("User : " + username + " / Pass : " + password);
			user.Register (password);
		} catch (CloudException e) {
			errText = e.Message;
		}
		yield return null;
		callback (errText);
		yield return null;
	}
}
