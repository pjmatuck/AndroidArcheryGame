using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonBehavior : MonoBehaviour {

    public float balloonLiftSpeed;
    public ParticleSystem explosion;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (GameManager.gameState == GameState.GAME)
        {
            transform.Translate(0f, balloonLiftSpeed * Time.deltaTime, 0f);
        }
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Arrow")
        {
            Debug.Log("Arrow --> Balloon");
            gameObject.SetActive(false);
            Instantiate(explosion, new Vector3(transform.position.x, transform.position.y, -1f), Quaternion.identity);
        }
    }
}
