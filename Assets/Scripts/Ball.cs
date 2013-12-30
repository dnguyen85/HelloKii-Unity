using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {
    public float INIT_DEGREE = 75f;
    public float INIT_SPEED = 500f;
    public bool shotStatus;

    // Use this for initialization
    void Start () {
        shotStatus = false;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetButton ("Jump")) {
            shotBall ();
        }

        if (Input.touchCount == 1) {
            Touch touch = Input.GetTouch (0);
            if (touch.phase == TouchPhase.Began) {
                shotBall ();
            }
        }
    }

    void shotBall () {
        if (shotStatus == true) {
            return;
        }
        shotStatus = true;
        Vector3 vel = Vector3.zero;
        vel.x = INIT_SPEED * Mathf.Cos (INIT_DEGREE * Mathf.PI / 180f);
        vel.y = INIT_SPEED * Mathf.Sin (INIT_DEGREE * Mathf.PI / 180f);
        rigidbody.velocity = vel;
    }

    // Collision behavior for non-trigger
    void OnCollisionEnter (Collision col) {
        //  If ball hits the block, send "destroyBlock" message to block object.
        if (col.gameObject.CompareTag ("Block")) {
            col.gameObject.SendMessage ("destroyBlock");
        }

        // To normalize ball speed
        rigidbody.velocity = rigidbody.velocity.normalized * INIT_SPEED;

        // To avoid infinite bounce loop, add some acceleration for each angle.
        if (Mathf.Abs (rigidbody.velocity.y) < 20) {
            Vector3 vel = Vector3.zero;
            vel.x = rigidbody.velocity.x;
            vel.y = rigidbody.velocity.y * 5;
            rigidbody.velocity = vel;
        }
        if (Mathf.Abs (rigidbody.velocity.x) < 20) {
            Vector3 vel = Vector3.zero;
            vel.x = rigidbody.velocity.x * 5;
            vel.y = rigidbody.velocity.y;
            rigidbody.velocity = vel;
        }
     
    }

    // Collision behavior for trigger
    void OnTriggerEnter (Collider other) {
        // If ball hits the miss frame (bottom frame), load GameOver scene.
        if (other.CompareTag ("MissFrame")) {
            Application.LoadLevel ("5_GameOver");
        }
    }
}
