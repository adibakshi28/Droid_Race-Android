using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrail : MonoBehaviour {

	public float followSpeed;
	public GameObject player;

	void LateUpdate () {
		Vector3 pos = player.transform.position;
		pos.y = transform.position.y;
	//	pos.z = pos.z + 1f;
		transform.position = Vector3.Lerp (transform.position, pos, followSpeed * Time.deltaTime);
	}
}
