using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehav : MonoBehaviour {   // Camera is child of player object

	public float followSpeed = 3;
	[HideInInspector]
	public bool follow = false;

	private GameObject player;       
	private Vector3  followPos,orignalPosition,orignalRotation;


	Player playerScript;

	void Awake () 
	{
		player = this.gameObject.transform.parent.transform.gameObject;
		playerScript = player.GetComponent<Player> ();
	}

	void Start(){
		followPos = transform.localPosition;
		orignalPosition = followPos;
		orignalRotation = transform.localEulerAngles;
	}



	void FixedUpdate(){
		Vector3 rot = transform.localEulerAngles;
		rot.x = orignalRotation.x - (playerScript.speed - playerScript.minSpeed) * 0.5f;
		transform.localEulerAngles = Vector3.Lerp (transform.localEulerAngles, rot, 10f * Time.deltaTime);
		followPos.z = orignalPosition.z - ((playerScript.speed - playerScript.minSpeed) * 0.2f);
		followPos.y = orignalPosition.y + ((playerScript.speed - playerScript.minSpeed) * 0.04f);
	}

	void LateUpdate () 
	{
		if (follow) {
			transform.localPosition = Vector3.Lerp(transform.localPosition ,followPos ,followSpeed*Time.deltaTime);
			Vector3 rot = transform.localEulerAngles;
			rot.z = transform.position.x * -1.75f;
			transform.localEulerAngles = rot;
		}
	}

	public void CameraBounce(){
		StartCoroutine (Bounce (1));	
	}
	IEnumerator Bounce(int times){
		Vector3 orignalOffset = followPos;
		for (int j = 0; j < times; j++) {
			for (int i = 0; i < 5; i++) {
				switch (j) {
				case 0:
					followPos.y = followPos.y - 0.1f;
					yield return new WaitForSeconds (0.025f);
					break;
				case 1:
					followPos.y = followPos.y - 0.075f;
					yield return new WaitForSeconds (0.01f);
					break;
				}
			}
			for (int i = 0; i < 5; i++) {
				switch (j) {
				case 0:
					followPos.y = followPos.y + 0.1f;
					yield return new WaitForSeconds (0.025f);
					break;
				case 1:
					followPos.y = followPos.y + 0.075f;
					yield return new WaitForSeconds (0.01f);
					break;
				}
			}
		}
		followPos = orignalOffset;
	}
}