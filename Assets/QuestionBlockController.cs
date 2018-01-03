using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionBlockController : MonoBehaviour {

	public float bounceHeight;
	public float bounceSpeed;

	private Vector2 originPosition;

	private bool canBounce = true;
	// Use this for initialization
	void Start () {
		originPosition = transform.localPosition;
	}

	public void QuestionBlockBounce() {

		if (canBounce) {

			canBounce = false;
			StartCoroutine (Bounce ());
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator Bounce() {

		while (true) {

			transform.localPosition = new Vector2 (transform.localPosition.x, transform.localPosition.y + bounceSpeed * Time.deltaTime);

			if (transform.localPosition.y >= originPosition.y + bounceHeight) {
				break;
			}

			yield return null;
		}

		while (true) {

			transform.localPosition = new Vector2 (transform.localPosition.x, transform.localPosition.y - bounceSpeed * Time.deltaTime);

			if (transform.localPosition.y <= originPosition.y) {

				transform.localPosition = originPosition;
				break;
			}

			yield return null;
		}
	}
}
