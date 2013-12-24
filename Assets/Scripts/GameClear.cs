using UnityEngine;
using System.Collections;

public class GameClear : MonoBehaviour {

    // Use this for initialization
    void Start () {
        ScoreManager.refreshHighScore();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetButton ("Jump")) {
            Application.LoadLevel ("GameTitle");
        }
     
        if (Input.touchCount == 1) {
            Touch touch = Input.GetTouch (0);
            if (touch.phase == TouchPhase.Began) {
                Application.LoadLevel ("GameTitle");
            }
        }
    }

    void OnGUI () {
        GUILayout.BeginArea (new Rect (0, 0, Screen.width, Screen.height));
        GUILayout.FlexibleSpace ();
        GUILayout.BeginHorizontal ();
        GUILayout.FlexibleSpace ();
        GUILayout.BeginVertical ();

        GUILayout.Label ("Game Clear");
        GUILayout.Label ("Score : " + ScoreManager.getCurrentScore ());
        GUILayout.Label ("High Score : " + ScoreManager.getCachedHighScore());

        if (GUILayout.Button ("Replay!", GUILayout.MinHeight (50), GUILayout.MinWidth (100))) {
            Application.LoadLevel ("GameMain");
        }
        if (GUILayout.Button ("Send high score", GUILayout.MinHeight (50), GUILayout.MinWidth (100))) {
            ScoreManager.sendHighScore(ScoreManager.getHighScore ());
        }
        if (GUILayout.Button ("Return to title", GUILayout.MinHeight (50), GUILayout.MinWidth (100))) {
            Application.LoadLevel ("GameTitle");
        }
        GUILayout.EndVertical ();
        GUILayout.FlexibleSpace ();
        GUILayout.EndHorizontal ();
        GUILayout.FlexibleSpace ();
        GUILayout.EndArea ();
    }
}
