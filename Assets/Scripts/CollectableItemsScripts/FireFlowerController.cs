using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlowerController : MonoBehaviour {

	private CircleCollider2D coll;
	public float initialSpeed;
	private float positionY;
	private bool isReadyForCollect = false;
	// Use this for initialization
	void Start () {
		coll = GetComponent<CircleCollider2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.localPosition.y <= coll.radius * 2) {
			positionY = transform.position.y + (initialSpeed * Time.deltaTime);
			transform.position = new Vector2 (transform.position.x, positionY);
		} else
			isReadyForCollect = true;
	}

	void OnCollisionEnter2D( Collision2D other )
	{
		Debug.Log ("test");
		if (other.gameObject.tag == "Player" && isReadyForCollect) {
			PlayerController.PLAYER_STATE = 2;
			Destroy (this.gameObject);
		}
	}
}
