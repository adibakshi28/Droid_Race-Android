using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownShooter : MonoBehaviour {

	public List<GameObject> obstical;
	public float rate,delay;
	public GameObject model;
	[HideInInspector]
	public float finalPosZ = 20,finalPosY = 4f;

	private GameObject player;
	private bool finalPosZReached = false,exiting = false;

	AudioSource aud;

	Obstical_Ossilate obsOssilateScript;
	Player playerScript;
	LevelController levelController;

	void Start () {
		levelController = GameObject.FindGameObjectWithTag ("LevelController").GetComponent<LevelController> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		playerScript = player.GetComponent<Player> ();
		obsOssilateScript = GetComponent<Obstical_Ossilate> ();
		aud = GetComponent<AudioSource> ();
		obsOssilateScript.enabled = false;
	}

	void Update () {
		if (!exiting) {
			if (finalPosZReached) {
				Vector3 pos = transform.position;
				pos.z = player.transform.position.z + finalPosZ;
				transform.position = pos;
			}
			else {
				Vector3 pos = player.transform.position;
				pos.y = finalPosY;
				pos.z = pos.z + finalPosZ - 0.2f;
				transform.position = Vector3.Lerp (transform.position, pos, 0.05f * Time.deltaTime);
				if (transform.position.z < player.transform.position.z + finalPosZ) {
					finalPosZReached = true;
					StartGenerating ();
				}
			}
		}
		if (exiting) {
			Vector3 pos = player.transform.position;
			pos.y = finalPosY;
			pos.z = pos.z - 20;
			transform.position = Vector3.Lerp (transform.position, pos, 0.01f * Time.deltaTime);
		}
	}

	public void StartGenerating(){
		InvokeRepeating ("GenerateObstical",delay, rate);
	}

	public void StopGenerating(){
		CancelInvoke ();
	}

	public void Exit(){
		obsOssilateScript.enabled = false;
		exiting = true;
	}

	void GenerateObstical(){
		if ((finalPosZReached)&&(!levelController.gameOver)) {
			obsOssilateScript.enabled = true;
			GameObject obs;
			Vector3 pos = transform.position;
	//		pos.x = pos.x - 1;
	//		pos.y = pos.y + 2.5f;
			pos.z = pos.z - 0.75f;
			aud.Play ();
			obs = Instantiate(obstical[Random.Range(0,obstical.Count)],pos,Quaternion.identity)as GameObject;
			Vector3 rot = obs.transform.eulerAngles;
			rot.x = 180;
			rot.y = 90;
			obs.transform.eulerAngles = rot;
			Destroy (obs, 5);
		}
	}
		
}
