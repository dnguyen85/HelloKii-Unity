using UnityEngine;
using System.Collections;

public class Bar : MonoBehaviour {

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        // Get input position
        Vector3 inputPos = Input.mousePosition;
        // Substruct camera z-position for coordinate transform
        inputPos.z = -Camera.main.transform.position.z;
        // Get input posision on game screen
        Vector3 inputPosOnGame = Camera.main.ScreenToWorldPoint (inputPos);

        // Get bar position
        Vector3 barPosition = rigidbody.position;
        // Change bar x-position to input x-position 
        barPosition.x = inputPosOnGame.x;

        // Apply position
        rigidbody.MovePosition (barPosition);
    }
}
