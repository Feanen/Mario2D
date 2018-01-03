using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraColliderController : MonoBehaviour {
	private Collider2D coll;

	void Start()
	{
		coll = GetComponent<Collider2D> ();
	}

	void OnCollisionEnter2D( Collision2D collision )
	{
		if (collision.gameObject.tag != "Player")
			Physics2D.IgnoreCollision (coll, collision.collider);
	}
}
