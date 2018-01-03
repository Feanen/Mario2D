using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollidersController : MonoBehaviour {

	public static int COLLIDERS_STATE = 0;
	private int currentState = 0;

	private BoxCollider2D boxCollider;
	private Transform groundPoint1;
	private Transform groundPoint2;
	private CapsuleCollider2D bottomCapsule;
	private CapsuleCollider2D topCapsule;
	private CapsuleCollider2D topLeftCapsule;
	// Use this for initialization
	void Start () {
		boxCollider = GetComponent<BoxCollider2D> ();
		groundPoint1 = transform.FindChild ("GroundPointLeft");
		groundPoint2 = transform.FindChild ("GroundPointRight");
		bottomCapsule = transform.FindChild ("BottomCapsule").GetComponent<CapsuleCollider2D> ();
		topCapsule = transform.FindChild ("TopCapsule").GetComponent<CapsuleCollider2D> ();
		topLeftCapsule = transform.FindChild ("TopLeftCapsule").GetComponent<CapsuleCollider2D> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (currentState != COLLIDERS_STATE) {
			ChangeCollidersPosition ();
		}

		currentState = COLLIDERS_STATE;
	}

	void ChangeCollidersPosition() {
		ChangeBoxColliderPosition ();
		ChangeBottomCapsuleCollider ();
		ChangeTopCapsuleCollider ();
		ChangeTopLeftCapsuleCollider ();
		ChangeGroundPointsPosition ();
	}

	void ChangeBoxColliderPosition() {
		switch (COLLIDERS_STATE) {
		case 0:
			boxCollider.size = new Vector2 (0.12f, 0.13f);
			boxCollider.offset = new Vector2 (0.06f, 0.075f);	
			break;

		case 1:
			boxCollider.size = new Vector2 (0.16f, 0.13f);
			boxCollider.offset = new Vector2 (0.08f, 0.075f);	
			break;

		case 2:
			boxCollider.size = new Vector2 (0.16f, 0.28f);
			boxCollider.offset = new Vector2 (0.08f, 0.16f);
			break;
		}		
	}

	void ChangeBottomCapsuleCollider(){
		switch (COLLIDERS_STATE) {
		case 0:
			bottomCapsule.size = new Vector2 (0.12f, 0.01f);
			bottomCapsule.offset = new Vector2 (0.06f, 0.005f);	
			break;

		case 1:
			bottomCapsule.size = new Vector2 (0.16f, 0.01f);
			bottomCapsule.offset = new Vector2 (0.08f, 0.005f);	
			break;

		case 2:
			bottomCapsule.size = new Vector2 (0.16f, 0.02f);
			bottomCapsule.offset = new Vector2 (0.08f, 0.01f);	
			break;

		}	
	}

	void ChangeTopCapsuleCollider(){
		switch (COLLIDERS_STATE) {
		case 0:
			topCapsule.size = new Vector2 (0.03f, 0.02f);
			topCapsule.offset = new Vector2 (0.105f, 0.15f);	
			break;

		case 1:
			topCapsule.size = new Vector2 (0.03f, 0.02f);
			topCapsule.offset = new Vector2 (0.145f, 0.15f);	
			break;

		case 2:
			topCapsule.size = new Vector2 (0.03f, 0.02f);
			topCapsule.offset = new Vector2 (0.135f, 0.31f);	
		break;

		}
	}

	void ChangeTopLeftCapsuleCollider(){
		switch (COLLIDERS_STATE) {
		case 0:
		case 1:
			topLeftCapsule.size = new Vector2 (0.03f, 0.02f);
			topLeftCapsule.offset = new Vector2 (0.015f, 0.15f);	
			break;

		case 2:
			topLeftCapsule.size = new Vector2 (0.03f, 0.02f);
			topLeftCapsule.offset = new Vector2 (0.015f, 0.31f);	
			break;

		}	
	}

	void ChangeGroundPointsPosition() {
		switch (COLLIDERS_STATE) {
		case 0:
			groundPoint1.transform.localPosition = new Vector2 ( 0.02f, 0);
			groundPoint2.transform.localPosition = new Vector2 ( 0.1f, 0);
			break;

		case 1:
		case 2:
			groundPoint2.transform.localPosition = new Vector2 ( 0.14f, 0);	
			break;

		}	
	}
}
