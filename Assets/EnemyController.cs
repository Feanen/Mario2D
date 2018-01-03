using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	public float gravity;
	public Vector2 velocity;
	public bool isWalkingLeft = true;

	private bool grounded = false;

	public LayerMask floorMask;
	public LayerMask wallMask;

	private bool shouldDie = false;
	private float timerToDestroy = 0;

	public float timerBeforeDestroy = 1.0f;

	private enum EnemyState
	{
		
		walking,
		falling,
		dead
	}

	private EnemyState state = EnemyState.falling;

	void Start () {
		
		enabled = false; //блокирует скрипт до нужного момента
		Fall();
	}

	void FixedUpdate () {

		ChangeEnemyPosition ();

		CheckedCrush ();
	}

	public void Crush () {

		state = EnemyState.dead;

		GetComponent<Animator> ().SetBool ("crushed", true);

		GetComponent<BoxCollider2D> ().enabled = false;

		shouldDie = true;
	}

	void CheckedCrush() {

		if (shouldDie) {

			if (timerToDestroy <= timerBeforeDestroy) {

				timerToDestroy += Time.deltaTime;

			} else {

				shouldDie = false;

				Destroy (this.gameObject);
			}
		}
	}

	void ChangeEnemyPosition () {

		if (state != EnemyState.dead) {

			Vector3 pos = transform.localPosition;
			Vector3 scale = transform.localScale;

			if (state == EnemyState.falling) {
				
				pos.y += velocity.y * Time.deltaTime;
				velocity.y -= gravity * Time.deltaTime;
			}

//			if (state == EnemyState.walking) {

			if (isWalkingLeft) {
				
					pos.x -= velocity.x * Time.deltaTime;

					scale.x = -1;
				} else {
				
					pos.x += velocity.x * Time.deltaTime;
					
					scale.x = 1;
				}
//			}

			if (velocity.y <= 0)
				pos = CheckGround (pos);

			CheckWalls (pos, scale.x);

			transform.localPosition = pos;
			transform.localScale = scale;
		}
	}

	Vector3 CheckGround (Vector3 pos) {
	
		Vector2 originLeft = new Vector2 (pos.x - .08f + .02f, pos.y - .08f);
		Vector2 originMiddle = new Vector2 (pos.x, pos.y - .08f);
		Vector2 originRight = new Vector2 (pos.x + .08f - .02f, pos.y - .08f);

		RaycastHit2D groundLeft = Physics2D.Raycast (originLeft, Vector2.down, velocity.y * Time.deltaTime, floorMask);
		RaycastHit2D groundMiddle = Physics2D.Raycast (originMiddle, Vector2.down, velocity.y * Time.deltaTime, floorMask);
		RaycastHit2D groundRight = Physics2D.Raycast (originRight, Vector2.down, velocity.y * Time.deltaTime, floorMask);

		if (groundLeft.collider != null || groundMiddle.collider != null || groundRight.collider != null) {

			RaycastHit2D hitRay = groundLeft;

			if (groundLeft) {
				hitRay = groundLeft;
			} else if (groundMiddle) {
				hitRay = groundMiddle;
			} else if (groundRight) {
				hitRay = groundRight;
			}
				
			pos.y = hitRay.collider.bounds.center.y + hitRay.collider.bounds.size.y / 2 + .08f;

			grounded = true;

			velocity.y = 0;

			state = EnemyState.walking;
		} else {

			if (state != EnemyState.falling) {

				Fall ();
			}
		}
			
		return pos;
	}

	void CheckWalls (Vector3 pos, float direction) {

		Vector2 originTop = new Vector2 (pos.x + direction * .06f, pos.y + .06f);
		Vector2 originMiddle = new Vector2 (pos.x + direction * .06f, pos.y);
		Vector2 originBottom = new Vector2 (pos.x + direction * .06f, pos.y - .06f);

		RaycastHit2D wallTop = Physics2D.Raycast (originTop, new Vector2 (direction, 0), velocity.x * Time.deltaTime, wallMask);
		RaycastHit2D wallMiddle = Physics2D.Raycast (originMiddle, new Vector2 (direction, 0), velocity.x * Time.deltaTime, wallMask);
		RaycastHit2D wallBottom = Physics2D.Raycast (originBottom, new Vector2 (direction, 0), velocity.x * Time.deltaTime, wallMask);

		if (wallTop.collider != null || wallMiddle.collider != null || wallBottom.collider != null) {

			RaycastHit2D hitRay = wallTop;

			if (wallTop) {
				hitRay = wallTop;
			} else if (wallMiddle) {
				hitRay = wallMiddle;
			} else if (wallBottom) {
				hitRay = wallBottom;
			}

			if (hitRay.collider.tag == GameTagsAndLayers.GROUND_TAG || ( hitRay.collider.tag == GameTagsAndLayers.ENEMY_TAG && hitRay.collider.gameObject != this.gameObject) ) {

				isWalkingLeft = !isWalkingLeft;
			}

			Debug.Log (hitRay.collider.tag);
		}
	}

	void OnBecameVisible() {

		this.enabled = true;
	}

	void Fall() {

		velocity.y = 0;
		state = EnemyState.falling;
		grounded = false;
	}
}
