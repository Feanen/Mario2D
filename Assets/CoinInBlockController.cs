using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinInBlockController : MonoBehaviour {

	private Animator anim;

	public float coinMoveSpeed;
	public float coinMoveHeight;
	public float coinFallDistance;
	public GameObject instGameObject;

	public float bounceHeight;
	public float bounceSpeed;
	public float gravity;

	private bool canBounce = true;

	private Vector2 originPosition;
	// Use this for initialization
	void Start () {
		
		anim = GetComponent<Animator> ();
		originPosition = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void InstantiateCoin () {

		GameObject coin = Instantiate (instGameObject);
		coin.transform.SetParent (this.transform.parent);
		coin.transform.localPosition = new Vector2 (originPosition.x, originPosition.y);
		StartCoroutine (MoveCoin (coin));
	}

	IEnumerator MoveCoin( GameObject coin ) {

		while (true) {

			coin.transform.localPosition = new Vector2 (coin.transform.localPosition.x, coin.transform.localPosition.y + coinMoveSpeed * Time.deltaTime);

			coinMoveSpeed -= gravity;

			if (coinMoveSpeed < 0)
				break;

			yield return null;
		}

		while (true) {

			coin.transform.localPosition = new Vector2 (coin.transform.localPosition.x, coin.transform.localPosition.y + coinMoveSpeed * Time.deltaTime);

			coinMoveSpeed -= gravity;

			if (coin.transform.localPosition.y <= originPosition.y + coinFallDistance) {
				
				Destroy (coin.gameObject);
				break;
			}

			yield return null;
		}
	}

	public void QuestionBlockBounce() {

		if (canBounce) {

			canBounce = false;
			StartCoroutine (Bounce ());
		}
	}

	IEnumerator Bounce() {

		anim.SetBool (AnimationParameters.WAS_HITTED, true);

		InstantiateCoin ();

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
