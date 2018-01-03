using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinInQuestionController : MonoBehaviour {

	private Rigidbody2D rb;
	private BoxCollider2D coll;

	public float speed;
	public float height;
	public float acceleration;
	public float speedLimitUntilDestroy;
	// Use this for initialization
	void Start ()
	{
		rb = GetComponent<Rigidbody2D> ();
	}

	// Update is called once per frame
	void FixedUpdate () 
	{
		rb.velocity = new Vector2( 0, speed * Time.deltaTime );
		speed += acceleration;
		if (speed <= speedLimitUntilDestroy)
			Destroy (gameObject);
	}
}
