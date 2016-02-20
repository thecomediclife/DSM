using UnityEngine;
using System.Collections;

public class PlayerController2 : MonoBehaviour {
	public CharacterController charContr;
	public Animator anim;
	public Transform target;
	public float moveSpeed = 5f;

	private bool attacking = false;
	private bool nextAttackInput = false;
	private bool nextAttackInputSet;
	private int attackIndex = 1;

	// Use this for initialization
	void Start () {
		anim = this.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 moveDirection = transform.right * Input.GetAxis ("Horizontal") + transform.forward * Input.GetAxis ("Vertical");
		moveDirection = moveDirection * moveSpeed;
		charContr.Move (moveDirection * Time.deltaTime);

		Quaternion lookRot = Quaternion.LookRotation (target.position - transform.position);
		lookRot = Quaternion.Euler(new Vector3 (0f, lookRot.eulerAngles.y, 0f));
		transform.rotation = lookRot;

		anim.SetFloat ("X", Input.GetAxis ("Horizontal"));
		anim.SetFloat ("Y", Input.GetAxis ("Vertical"));

		if (Input.GetKeyDown (KeyCode.Z)) {

			if (!attacking) {
				anim.SetInteger ("Attack", attackIndex);
				anim.SetBool ("EndAttack", false);
				attacking = true;
			}

			if (nextAttackInput) {
				anim.SetInteger ("Attack", attackIndex);
				anim.SetBool ("EndAttack", false);
				nextAttackInputSet = true;
				nextAttackInput = false;
				Debug.Log ("run");
			}

//			if (!attacking) {
//				anim.SetInteger ("Attack", attackIndex);
//				anim.SetBool ("EndAttack", false);
//				attacking = true;
//			}
//
//			if (nextAttackInput && !nextAttackInputSet) {
//				//anim.SetInteger ("Attack", anim.GetInteger ("Attack") + 1);
//				nextAttackInputSet = true;
//				Debug.Log ("set");
//			}
		}
	}

	void InitAttack(int attackNum) {
		attackIndex = attackNum;
		nextAttackInput = false;
		nextAttackInputSet = false;
	}

	void BeginAttack() {
		//nextAttackInput = true;
	}

	void BeginNextAttack() {
		nextAttackInput = true;

//		if (nextAttackInputSet) {
//			anim.SetInteger ("Attack", anim.GetInteger ("Attack") + 1);
//			print (anim.GetInteger ("Attack"));
//		}
	}

	void EndAttack() {
		if (!nextAttackInputSet) {
			anim.SetInteger ("Attack", 0);
			attacking = false;
			nextAttackInput = false;
			nextAttackInputSet = false;
			anim.SetBool ("EndAttack", true);
		}
	}
}
