using UnityEngine;
using System.Collections;

public class PlayerController2 : MonoBehaviour {
	public CharacterController charContr;
	public Animator anim;
	public Transform target;
	public float moveSpeed = 5f;
	public float dashSpeed = 7.5f;

	public bool dealDamage = false, invulnerable = false;

	private bool nextAttackInput = false;
	private bool nextAttackInputSet;
	public int attackIndex = 1;
	private bool queueNextAttack;

	public enum PlayerState{walking, attacking, dodging};
	public PlayerState currentPlayerState = PlayerState.walking;

	private Vector3 dashDirection;
	private bool dashMove;

	// Use this for initialization
	void Start () {
		anim = this.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (currentPlayerState == PlayerState.walking) {
			Vector3 moveDirection = transform.right * Input.GetAxis ("Horizontal") + transform.forward * Input.GetAxis ("Vertical");
			moveDirection = moveDirection * moveSpeed;
			charContr.Move (moveDirection * Time.deltaTime);

			Quaternion lookRot = Quaternion.LookRotation (target.position - transform.position);
			lookRot = Quaternion.Euler(new Vector3(0f, lookRot.eulerAngles.y, 0f));
			transform.rotation = lookRot;

			anim.SetFloat ("X", Input.GetAxis ("Horizontal"));
			anim.SetFloat ("Y", Input.GetAxis ("Vertical"));
		}

		if (Input.GetKeyDown (KeyCode.Z)) {
			if (!nextAttackInput) {
				anim.SetInteger ("Attack", 1);
			} else {
				nextAttackInputSet = true;
			}

			if (queueNextAttack) {
				anim.SetInteger ("Attack", attackIndex);
			}
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			Quaternion lookRot = Quaternion.identity;

			if (Input.GetAxis ("Vertical") >= 0f) {
				lookRot = Quaternion.LookRotation (transform.right * Input.GetAxis ("Horizontal") + transform.forward * Input.GetAxis ("Vertical"));
			} else {
				lookRot = Quaternion.LookRotation (-transform.right * Input.GetAxis ("Horizontal") - transform.forward * Input.GetAxis ("Vertical"));
			}
			lookRot = Quaternion.Euler(new Vector3(0f, lookRot.eulerAngles.y, 0f));
			transform.rotation = lookRot;

			if (Input.GetAxis ("Vertical") >= 0f) {
				dashDirection = transform.forward;
				anim.SetFloat ("Y", 1f);
			} else {
				dashDirection = -transform.forward;
				anim.SetFloat ("Y", -1f);
			}

			anim.SetBool ("Dash", true);
			currentPlayerState = PlayerState.dodging;
		}

		if (dashMove) {
			charContr.Move (dashDirection * dashSpeed * Time.deltaTime);
		}
	}

	void InitAttack() {
		currentPlayerState = PlayerState.attacking;
	}

	void StartWalk() {
		currentPlayerState = PlayerState.walking;
		anim.SetBool ("Dash", false);
	}

	void BeginAttack(int attackNum) {
		nextAttackInput = true;
		dealDamage = true;

		attackIndex = attackNum;
	}

	void BeginNextAttack() {
		if (nextAttackInputSet) {
			anim.SetInteger ("Attack", attackIndex);
		}

		queueNextAttack = true;
	}

	void EndAttack() {
		anim.SetInteger ("Attack", 0);
		dealDamage = false;
		nextAttackInput = false;
		nextAttackInputSet = false;
		queueNextAttack = false;
	}

	void BeginDash() {
		dashMove = true;
		invulnerable = true;
	}

	void EndDash() {
		dashMove = false;
		invulnerable = false;
	}
}
