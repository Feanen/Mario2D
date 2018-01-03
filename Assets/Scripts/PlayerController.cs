using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
	public static int PLAYER_STATE;
	public const float CELL_WIDTH = 0.16f;

	private Rigidbody2D rb;
	private Animator anim;
	private Transform trans;
	//private BoxCollider2D coll;

	private float jumpHeightByDefault;

	//переменные движения
	public float speed; //скорость передвижения


	//переменные, которые уточняют, куда смотрит персонаж
	private bool flippedRight; //ориентация Марио(смотреть налево или направо)


	//переменные для прыжка
	[SerializeField]
	private Transform[] groundPoints; //массив пустых объектов, которые располагаются у ног персонажа, для будущей проверки на коллизии с землей
	[SerializeField]
	private float groundRadius; //радиус для circle-объектов на проверку коллизий с землей
	private bool isGrounded = true; //бул-переменная, отвечающая за определение состояния нахождения на земле
	[SerializeField]
	private LayerMask whatIsGround; //маска для слоев, которая определяет все слои, с которыми должны производится рассчеты на коллизии
	public float jumpHeight; //высота прыжка
	[SerializeField]
	public float additionalJumpHeight; //дополнительная высота прыжка, если персонаж движется
	private int playerStatement; //переменная, хранящая последнее значение состояния

	// Use this for initialization
	void Start () 
	{
		flippedRight = true;
		rb = GetComponent<Rigidbody2D> ();
		trans = GetComponent<Transform> ();
		anim = GetComponentInChildren<Animator> ();
		//coll = gameObject.GetComponent<BoxCollider2D> ();
		jumpHeightByDefault = jumpHeight;
	}
		
	void Update()
	{
		CheckGround();
		PlayerStateListener ();
	}

	// Update is called once per frame
	void FixedUpdate ()
	{
		float horizontal = Input.GetAxis ("Horizontal");
		Move(horizontal);

		if (Input.GetKeyDown(KeyCode.Y))
			PLAYER_STATE = 0;
	}

	private void Move(float horizontalVel)
	{

		if (anim.GetBool ("animationInProgress")) {
			rb.velocity = new Vector2 (0,0);
			rb.isKinematic = true;

			rb.constraints = RigidbodyConstraints2D.FreezeAll;
		} else {
			if (rb.isKinematic) {
				rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
				rb.isKinematic = false;
			}
			
			rb.velocity = new Vector2 (horizontalVel * speed, rb.velocity.y);
			Flip (horizontalVel);

			anim.SetFloat ("speed", Mathf.Abs(horizontalVel));

			if ( rb.velocity.y == 0 ) {
				Jump ();
			}
		}
	}

	private void Flip(float horizontalVel)
	{
		if ((horizontalVel > 0 && !flippedRight) ||(horizontalVel < 0 && flippedRight) ) 
		{
			flippedRight = !flippedRight;

			Vector3 locScale = trans.localScale;
			locScale.x *= -1;
			trans.localScale = locScale;
			trans.Translate (new Vector2 ( - CELL_WIDTH * locScale.x , 0 ));
		} 
	}

	private void Jump()
	{
		if (isGrounded && Input.GetButton("Jump") ) 
		{
			isGrounded = false;
			anim.SetBool ("isGrounded", isGrounded);	
			if ( Input.GetButton("Horizontal") ) {
				jumpHeight += additionalJumpHeight;
			}
			rb.AddForce (new Vector2 (0, jumpHeight)); //метод, который заставляет передвинутся Rigidbody на новую позицию(толкает)

			jumpHeight = jumpHeightByDefault;
		}


	}
		
	private bool CheckGround()
	{
		if (rb.velocity.y == 0) 
		{
			foreach (Transform point in groundPoints)
			{
				isGrounded = Physics2D.OverlapCircle (point.position, groundRadius, whatIsGround);

				if( isGrounded )
				{

					anim.SetBool ("isGrounded", isGrounded);
					return isGrounded;
				}
			}
		}
			
		return false;
	}

	void PlayerStateListener()
	{
		if (PLAYER_STATE != playerStatement) {
//			if( PLAYER_STATE == 1)
//				anim.SetInteger ("playerState", PLAYER_STATE);
//			else if (PLAYER_STATE == 2)
			anim.SetInteger ("playerState", PLAYER_STATE);
		}
				

		playerStatement = PLAYER_STATE;
	}
}
