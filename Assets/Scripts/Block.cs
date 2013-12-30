using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {

    public int ADD_SCORE = 10;
    static int iBlockCnt;

    // Use this for initialization
    void Start () {
        iBlockCnt = GameObject.FindGameObjectsWithTag ("Block").Length;
    }

    // Update is called once per frame
    void Update () {

    }

    // Destroy block
    public void destroyBlock () {
        // Destroy block
        Destroy (gameObject);
        // Add score
        ScoreManager.addCurrentScore (ADD_SCORE);
        // Decrement block count
        iBlockCnt--;
        // If block count is 0, move "GameClear" scene.
        if (iBlockCnt == 0) {
            Application.LoadLevel ("4_GameClear");
        }
    }
}
