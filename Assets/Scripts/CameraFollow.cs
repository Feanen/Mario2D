using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour 
{
	[SerializeField] private Transform player;

	private Camera camera;
	public static float CAMERA_VIEWPORT_RIGHT_SIDE_X;

	[SerializeField] private float xDifference;

	[SerializeField] private float xTreshhold = 0.3f;

	[SerializeField] private float speed;
	//private GameObject player;

	private Vector2 topRightCorner;

	// Use this for initialization
	void Start () 
	{
		camera = GetComponent<Camera> ();
		topRightCorner = new Vector2 (1, 1);
		//player = GameObject.FindGameObjectWithTag ("Player");
	}
		
	void Update () 
	{
		xDifference = ( player.transform.position.x + player.localScale.x * PlayerController.CELL_WIDTH ) - transform.position.x;

		if ( xDifference >= xTreshhold ) {
			transform.position = Vector3.MoveTowards (transform.position, new Vector3(player.transform.position.x, transform.position.y, transform.position.z), speed * Time.deltaTime);
		}

		Vector2 tmpPos = camera.ViewportToWorldPoint (topRightCorner);

		CAMERA_VIEWPORT_RIGHT_SIDE_X = tmpPos.x;
		//transform.position = new Vector3 (player.transform.position.x, transform.position.y, transform.position.z);
	}
}
