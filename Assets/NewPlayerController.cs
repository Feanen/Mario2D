using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerController : MonoBehaviour {

	public Vector2 velocity; //скорость игрока
	public float jumpHeight;
	public float bounceHeight;
	public float gravity;

	public LayerMask wallMask;
	public LayerMask groundMask;

	private bool walk, walk_left, walk_right, jump;
	private bool grounded = false;
	private bool bounce = false;

	private Animator animator;

	public enum PlayerState
	{
		small,
		big,
		fire,
		invulnerable
	}

	private PlayerState playerState = PlayerState.small;

	public enum PlayerAnimationState 
	{
		idle,
		walking,
		jumping,
		crouching,
		bouncing //нет в анимации
	}

	private PlayerAnimationState playerAnimationState = PlayerAnimationState.idle;

	void Start () {

		animator = GetComponent<Animator> ();
	}
	
	void Update () {

		CheckInput ();

		UpdatePosition ();

		CheckAnimationStates ();
	}

	//метод, который обновляет позицию Марио в зависимости от ввода с клавиатуры
	void UpdatePosition () {

		Vector3 pos = transform.localPosition;
		Vector3 scale = transform.localScale;

		if (walk) {

			if (walk_left) {

				pos.x -= velocity.x * Time.deltaTime;
				scale.x = -1;
			}

			if (walk_right) {

				pos.x += velocity.x * Time.deltaTime;
				scale.x = 1;
			}

			pos = CheckWallRays (pos, scale.x);
		}

		if (jump && playerAnimationState != PlayerAnimationState.jumping) {

			playerAnimationState = PlayerAnimationState.jumping;
			velocity = new Vector2 (velocity.x, jumpHeight);
			grounded = false;
		}

		if (playerAnimationState == PlayerAnimationState.jumping) {

			pos.y += velocity.y * Time.deltaTime;
			velocity.y -= gravity * Time.deltaTime;
		}

		if (bounce && playerAnimationState != PlayerAnimationState.bouncing) {

			playerAnimationState = PlayerAnimationState.bouncing;
			velocity = new Vector2 (velocity.x, bounceHeight);
		}

		if (playerAnimationState == PlayerAnimationState.bouncing) {

			pos.y += velocity.y * Time.deltaTime;
			velocity.y -= gravity * Time.deltaTime;
		}

		if (velocity.y <= 0) {

			pos = CheckGroundRays (pos);
		}

		if (velocity.y >= 0) {

			pos = CheckCeilingRays (pos);
		}

		transform.localPosition = pos;
		transform.localScale = scale;
	}

	//метод, который проверяет ввод с клавиатуры
	void CheckInput () {

		bool input_left = Input.GetKey (KeyCode.LeftArrow);
		bool input_right = Input.GetKey (KeyCode.RightArrow);
		bool input_space = Input.GetKey (KeyCode.Space);

		walk = input_left || input_right;
		walk_left = input_left && !input_right;
		walk_right = !input_left && input_right;
		jump = input_space;
	}

	void CheckAnimationStates () {

		if (grounded && !walk) {

			animator.SetBool (AnimationParameters.IS_JUMPING, false);
			animator.SetBool (AnimationParameters.IS_WALKING, false);
		}

		if (grounded && walk) {
			
			animator.SetBool (AnimationParameters.IS_WALKING, true);
			animator.SetBool (AnimationParameters.IS_JUMPING, false);
		}

		if (playerAnimationState == PlayerAnimationState.jumping) {

			animator.SetBool (AnimationParameters.IS_JUMPING, true);
		}

	}

	//метод, определяющий RayCasts для определения коллизий со стенами
	Vector3 CheckWallRays (Vector3 pos, float dir) {

		//стартовая точка лучей
		Vector2 originTop = new Vector2 (pos.x + dir * .06f, pos.y + .06f);
		Vector2 originMiddle = new Vector2 (pos.x + dir * .06f, pos.y);
		Vector2 originBottom = new Vector2 (pos.x + dir * .06f, pos.y - .06f);

		//сами лучи
		RaycastHit2D wallTop = Physics2D.Raycast (originTop, new Vector2 (dir, 0), velocity.x * Time.deltaTime, wallMask);
		RaycastHit2D wallMiddle = Physics2D.Raycast (originMiddle, new Vector2 (dir, 0), velocity.x * Time.deltaTime, wallMask);
		RaycastHit2D wallBottom = Physics2D.Raycast (originBottom, new Vector2 (dir, 0), velocity.x * Time.deltaTime, wallMask);

		//проверка на коллизии
		if (wallTop.collider != null || wallMiddle.collider != null || wallBottom.collider != null) {

			pos.x -= velocity.x * Time.deltaTime * dir;
		}

		return pos;
	}

	//метод, определяющий RayCasts для определения коллизий с землей
	Vector3 CheckGroundRays (Vector3 pos) {

		Vector2 originLeft = new Vector2 (pos.x - .06f, pos.y - .08f);
		Vector2 originMiddle = new Vector2 (pos.x, pos.y - .08f);
		Vector2 originRight = new Vector2 (pos.x + .06f, pos.y - .08f);

		RaycastHit2D groundLeft = Physics2D.Raycast (originLeft, Vector3.down, velocity.y * Time.deltaTime, groundMask);
		RaycastHit2D groundMiddle = Physics2D.Raycast (originMiddle, Vector3.down, velocity.y * Time.deltaTime, groundMask);
		RaycastHit2D groundRight = Physics2D.Raycast (originRight, Vector3.down, velocity.y * Time.deltaTime, groundMask);

		if (groundLeft.collider != null || groundMiddle.collider != null || groundRight.collider != null) {

			RaycastHit2D hitRay = groundLeft;

			if (groundLeft) {
				hitRay = groundLeft;
			} else if (groundMiddle) {
				hitRay = groundMiddle;
			} else if (groundRight) {
				hitRay = groundRight;
			}

			if (hitRay.collider.tag == GameTagsAndLayers.ENEMY_TAG) {

				hitRay.collider.GetComponent<EnemyController> ().Crush ();
				bounce = true;
			}

			pos.y = hitRay.collider.bounds.center.y + hitRay.collider.bounds.size.y / 2 + .08f;

			grounded = true;

			playerAnimationState = PlayerAnimationState.idle;

			velocity.y = 0;

		} else {

			if (playerAnimationState != PlayerAnimationState.jumping) {

				Fall ();
			}
		}

		return pos;
	}

	//метод, определяющий RayCasts для определения коллизий с объектами над головой
	Vector3 CheckCeilingRays (Vector3 pos) {

		Vector2 originLeft = new Vector2 (pos.x - .06f, pos.y + .08f);
		Vector2 originMiddle = new Vector2 (pos.x, pos.y + .08f);
		Vector2 originRight = new Vector2 (pos.x + .06f, pos.y + .08f);

		RaycastHit2D ceilLeft = Physics2D.Raycast (originLeft, Vector3.up, velocity.y * Time.deltaTime, groundMask);
		RaycastHit2D ceilMiddle = Physics2D.Raycast (originMiddle, Vector3.up, velocity.y * Time.deltaTime, groundMask);
		RaycastHit2D ceilRight = Physics2D.Raycast (originRight, Vector3.up, velocity.y * Time.deltaTime, groundMask);

		if (ceilLeft.collider != null || ceilMiddle.collider != null || ceilRight.collider != null) {

			RaycastHit2D hitRay = ceilLeft;

			if (ceilLeft) {
				hitRay = ceilLeft;
			} else if (ceilMiddle) {
				hitRay = ceilMiddle;
			} else if (ceilRight) {
				hitRay = ceilRight;
			}

			if (hitRay.collider.tag == GameTagsAndLayers.QUESTION_BLOCK_TAG) {

				hitRay.collider.GetComponent<CoinInBlockController> ().QuestionBlockBounce ();
			}

			pos.y = hitRay.collider.bounds.center.y - hitRay.collider.bounds.size.y / 2 - .08f;

			Fall ();
		}

		return pos;
	}

	void Fall() {

		velocity.y = 0;
		playerAnimationState = PlayerAnimationState.jumping;
		grounded = false;
		bounce = false;

	}
}
