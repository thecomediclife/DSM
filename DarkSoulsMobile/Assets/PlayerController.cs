using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private Vector2 originalPos;
	public float speed = 5f;

	public Transform indicator, arrow;
	public Camera cam;

	private bool moving;

	private Vector3 lastPosition;
	private bool dash;
	private Vector3 dashPosition;
	public float dashSpeed = 10f;
	public float dashDistance = 5f;

	private float timeCounter;
	private bool attack;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
			originalPos = Input.GetTouch(0).position;
			moving = true;

			indicator.position = cam.ScreenToWorldPoint(new Vector3(originalPos.x, originalPos.y, 5f));
			indicator.GetComponent<SpriteRenderer>().enabled = true;
			arrow.GetComponent<SpriteRenderer>().enabled = true;
			arrow.position = cam.ScreenToWorldPoint(new Vector3(originalPos.x, originalPos.y, 5f));

			timeCounter = Time.time;
		}

		if (Input.touchCount > 0 && moving) {
			//Debug.Log(Input.GetTouch(0).position);

			//////Calculate next position
			float deltaX = Input.GetTouch(0).position.x - originalPos.x;
			float deltaY = Input.GetTouch(0).position.y - originalPos.y;
			Vector3 direction = new Vector3(deltaX, deltaY, 0f);
			direction.Normalize();

			if (!dash)
				transform.position += direction * speed * Time.deltaTime;

			//////Set arrow location.
			arrow.position = cam.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 5f));
			float angle  = Mathf.Atan2(deltaX, deltaY);
			angle = Mathf.Rad2Deg * angle * -1f;
			arrow.rotation = Quaternion.Euler(0f, 0f, angle);

			//////Calculate direction from previous update
			deltaX = Input.GetTouch(0).position.x - lastPosition.x;
			deltaY = Input.GetTouch(0).position.y - lastPosition.y;
			direction = new Vector3(deltaX, deltaY, 0f);
			direction.Normalize();

			////Check if Player has swiped screen
			if (Input.GetTouch(0).deltaPosition.magnitude > 80f && !dash) {
				dash = true;
				dashPosition = transform.position + (direction * dashDistance);
				//dashPosition = cam.ScreenToWorldPoint(new Vector3(dashPosition.x, dashPosition.y, 10f));
				this.GetComponent<Renderer>().material.color = Color.yellow;
			}

			//////Save the last position of this update for use in future updates
			lastPosition = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 0f);
		}

		/////Play attack animation
		if (attack) {
			attack = false;
			this.GetComponent<Renderer>().material.color = Color.white;
		}

		//////End touchp phase
		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Ended) {
			moving  = false;

			indicator.GetComponent<SpriteRenderer>().enabled = false;
			arrow.GetComponent<SpriteRenderer>().enabled = false;

			if (Time.time - timeCounter < 0.5f) {
				attack = true;
				this.GetComponent<Renderer>().material.color = Color.green;
			}
		}

		/////Dashing
		if (dash) {
			transform.position = Vector3.MoveTowards (transform.position, dashPosition, dashSpeed * Time.deltaTime);

			if (Vector3.SqrMagnitude(transform.position - dashPosition) < 16f) {
				this.GetComponent<Renderer>().material.color = Color.red;
			}

			if (Vector3.SqrMagnitude(transform.position - dashPosition) < 0.01f) {
				dash = false;
				this.GetComponent<Renderer>().material.color = Color.white;
			}
		}
	}
}
