using UnityEngine;
using System.Collections;

public class PlayerController2 : MonoBehaviour {
	public CharacterController charContr;
	public Animator anim;
	public Transform target;
	public float moveSpeed = 5f;

	public bool dealDamage = false;

	private bool nextAttackInput = false;
	private bool nextAttackInputSet;
	public int attackIndex = 1;
	private bool queueNextAttack;

	public enum PlayerState{walking, attacking};
	public PlayerState currentPlayerState = PlayerState.walking;

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
			lookRot = Quaternion.Euler (new Vector3 (0f, lookRot.eulerAngles.y, 0f));
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
	}

	void InitAttack() {
		currentPlayerState = PlayerState.attacking;
	}

	void StartWalk() {
		currentPlayerState = PlayerState.walking;
		Debug.Log ("walking");
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
}
