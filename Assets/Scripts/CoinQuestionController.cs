using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinQuestionController : MonoBehaviour {

	public GameObject coin;
	public GameObject spawn;
	// Use this for initialization

	private Animator anim;

	void Start()
	{
		anim = GetComponent<Animator>();
	}

	void OnCollisionEnter2D( Collision2D player )
	{
		if (player.gameObject.tag == "Player" && player.collider == player.gameObject.transform.FindChild("TopCapsule").GetComponent<CapsuleCollider2D>()) {
			var newInstance = Instantiate (coin, spawn.transform.position, Quaternion.identity);
			newInstance.transform.parent = gameObject.transform;
			anim.SetBool ("wasHitted", true);
			Destroy (this);
		}
	}
}
