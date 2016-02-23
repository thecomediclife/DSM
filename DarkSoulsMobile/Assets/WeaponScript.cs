using UnityEngine;
using System.Collections;

public class WeaponScript : MonoBehaviour {
	public PlayerController2 player;
	private BoxCollider collid;

	private bool hit;

	// Use this for initialization
	void Start () {
		collid = this.GetComponent<BoxCollider> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (player.dealDamage) {
			collid.enabled = true;
		} else {
			hit = false;
			collid.enabled = false;
		}
	}


	void OnTriggerEnter(Collider other) {
		if (other.tag == "Enemy") {
			hit = true;
			Debug.Log ("Hit!");
		}
	}
}
