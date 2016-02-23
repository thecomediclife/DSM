using UnityEngine;
using System.Collections;

public class CamMove : MonoBehaviour {
	
	public Transform player, target;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
		Vector3 transForward = new Vector3(target.position.x, 0f, target.position.z) - new Vector3(player.position.x, 0f, player.position.z);
		transForward.Normalize ();

		Vector3 location = player.position - (transForward * 15f);
		location = new Vector3 (location.x, 2.5f, location.z);
		transform.position = location;

		transform.rotation = Quaternion.LookRotation (transForward, new Vector3(0f,1f,0f));
		transform.rotation = Quaternion.Euler (new Vector3 (-15f, transform.rotation.eulerAngles.y, 0f));
	}
}
