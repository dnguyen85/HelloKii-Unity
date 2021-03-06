﻿using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {

    // Use this for initialization
    void Start () {
        ScoreManager.refreshHighScore();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetButton ("Jump")) {
            Application.LoadLevel ("1_GameTitle");
        }

        if (Input.touchCount == 1) {
            Touch touch = Input.GetTouch (0);
            if (touch.phase == TouchPhase.Began) {
                Application.LoadLevel ("1_GameTitle");
            }
        }
    }

    void OnGUI () {
        GUILayout.BeginArea (new Rect (0, 0, Screen.width, Screen.height));
        GUILayout.FlexibleSpace ();
        GUILayout.BeginHorizontal ();
        GUILayout.FlexibleSpace ();
        GUILayout.BeginVertical ();

        GUILayout.Label ("Game Over");
        GUILayout.Label ("Score : " + ScoreManager.getCurrentScore ());
        GUILayout.Label ("High Score : " + ScoreManager.getCachedHighScore());

        if (GUILayout.Button ("Replay!", GUILayout.MinHeight (50), GUILayout.MinWidth (100))) {
            Application.LoadLevel ("3_GameMain");
        }
        if (GUILayout.Button ("Send high score", GUILayout.MinHeight (50), GUILayout.MinWidth (100))) {
            ScoreManager.sendHighScore(ScoreManager.getCachedHighScore());
        }
        if (GUILayout.Button ("Return to title", GUILayout.MinHeight (50), GUILayout.MinWidth (100))) {
            Application.LoadLevel ("1_GameTitle");
        }
        GUILayout.EndVertical ();
        GUILayout.FlexibleSpace ();
        GUILayout.EndHorizontal ();
        GUILayout.FlexibleSpace ();
        GUILayout.EndArea ();
    }

}