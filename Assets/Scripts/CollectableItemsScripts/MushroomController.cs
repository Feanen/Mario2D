using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomController : MonoBehaviour {

	private Rigidbody2D rb;
	private BoxCollider2D coll;
	private bool isReadyToMove = false;
	public float speed;
	public float initialSpeed;
	private float positionY;
	// Use this for initialization
	void Start () {
		coll = GetComponent<BoxCollider2D> ();
		rb = GetComponent<Rigidbody2D> ();
		//rb.isKinematic = true;
	}

	// Update is called once per frame
	void Update () {
		if (!isReadyToMove) {
			if (transform.localPosition.y <= coll.size.y) {
				positionY = transform.position.y + (initialSpeed * Time.deltaTime);
				transform.position = new Vector2 (transform.position.x, positionY);
			} else {
				rb.isKinematic = false;
				isReadyToMove = true;
				Collider2D triggerCollider = gameObject.GetComponentInChildren<BoxCollider2D> ();
				if( triggerCollider != coll )
					triggerCollider.isTrigger = true;
			}
		} else {
			transform.Translate ( speed * Time.deltaTime, 0, 0 );
		}
	}

	void OnTriggerEnter2D( Collider2D other )
	{
		if( isReadyToMove && other.tag == "ground") {
			speed *= -1.0f;
		}
	}

	void OnCollisionEnter2D( Collision2D other )
	{
		if (other.gameObject.tag == "Player" && isReadyToMove) {
			PlayerController.PLAYER_STATE = 1;
			Destroy (this.gameObject);
		}
	}
}
