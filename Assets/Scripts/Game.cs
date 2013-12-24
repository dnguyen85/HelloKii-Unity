using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {

    private static int iScore;

    // Use this for initialization
    void Start () {
        Game.initGame ();
    }

    // Update is called once per frame
    void Update () {

    }

    void OnGUI () {
        GUI.Label (new Rect (10, 10, 100, 20), "Score : " + ScoreManager.getCurrentScore ());
        GUI.Label (new Rect (110, 10, 100, 20), "High Score : " + ScoreManager.getCachedHighScore ());
    }

    public static void initGame () {
        ScoreManager.initCurrentScore ();
    }
}
