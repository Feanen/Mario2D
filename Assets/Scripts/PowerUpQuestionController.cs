using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpQuestionController: MonoBehaviour {

	public GameObject mushroom;
	public GameObject fireFlower;
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
			if (PlayerController.PLAYER_STATE == 0)
				CreateNewInstance (mushroom);
			else if (PlayerController.PLAYER_STATE >= 1)
				CreateNewInstance (fireFlower);
			
			anim.SetBool ("wasHitted", true);
			Destroy (this);
		}
	}

	void CreateNewInstance( GameObject obj ) {
		var newInstance = Instantiate (obj, spawn.transform.position, Quaternion.identity);
		newInstance.transform.parent = gameObject.transform;
	}
}
