using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D player)
	{
		if(player.tag == "Player") {
			Destroy (gameObject);
		}	
	}
}
