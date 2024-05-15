using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float minSpeed, maxSpeed, speedCollSpeedIncrease = 5,decelerationRate = 0.8f,powerDuration = 7;
	public GameObject explosion,playerModel,afterBurnEffect,smallExplosion;
//	public GameObject trailingObject;
	[HideInInspector]
	public bool inPowerMode = false;
	[HideInInspector]
	public float speed;

	private bool gameOver = false,speedIncreased = false, vibrate = false;
	private float orignalDecelerationRate;

	Rigidbody rb;
	SphereCollider playerCollider;
	ConstantForce cf;

	CameraBehav cameraScript;
	LevelController levelController;

	void Start () {
		levelController = GameObject.FindGameObjectWithTag ("LevelController").GetComponent<LevelController> ();
		cameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraBehav> ();
		rb = GetComponent<Rigidbody> ();
		playerCollider = GetComponent<SphereCollider> ();
		cf = GetComponent<ConstantForce> ();
		cameraScript.follow = true;
		speed = minSpeed;
		orignalDecelerationRate = decelerationRate;
		powerDuration = powerDuration - 1;
	}

	void Update () {
		if (!gameOver) {
			if (speed > minSpeed) {            // Deceleration of speed
				speed = speed - (decelerationRate * Time.deltaTime);
			}
			else {
				speed = minSpeed;
			}
		}
	}

	void FixedUpdate(){
		if (!gameOver) {
			rb.velocity = new Vector3 (0, 0, speed);
		}
	}

	public void EnterPowerMode(){
		if (!gameOver) {
			inPowerMode = true;
			speed = maxSpeed;
			decelerationRate = 0;
			afterBurnEffect.SetActive (true);
	//		trailingObject.SetActive (false);
			StartCoroutine (DuringPowerMode ());
			levelController.aud.pitch = 1.5f;
		}
	}
	IEnumerator DuringPowerMode(){
		yield return new WaitForSeconds (powerDuration);
		ExitPowerMode ();
	}
	public void ExitPowerMode(){
		levelController.ResetPowerSlider ();
		decelerationRate = orignalDecelerationRate*50;
		afterBurnEffect.SetActive (false);
	//	trailingObject.SetActive (true);
		StartCoroutine (ExitPowerModeCoroutine ());
		levelController.aud.pitch = 1f;
	}
	IEnumerator ExitPowerModeCoroutine(){
		yield return new WaitForSeconds (1);
		inPowerMode = false;
		yield return new WaitForSeconds (2);
		decelerationRate = orignalDecelerationRate;
	}

	void GameOver(){
		gameOver = true;
		cameraScript.follow = false;
	//	Destroy (trailingObject);
		Instantiate (explosion, transform.position, Quaternion.identity);
		rb.isKinematic = true;
//		playerCollider.enabled = false;
		Destroy (playerModel);
		Handheld.Vibrate ();
		levelController.GameOver ();
	}

	IEnumerator GenerateDownForce(float downForce,float timeDelay){
		yield return new WaitForSeconds (timeDelay);
		cf.force = new Vector3 (0, downForce, 0);
	}

	void OnCollisionEnter(Collision col)
	{
		if ((col.gameObject.tag == "Enemy")&&(!gameOver)) {
			if(!inPowerMode){
				GameOver ();
			}
			else{
				Vector3 pos = col.gameObject.transform.position;
				Destroy (col.gameObject);
				GameObject exp;
				exp = Instantiate (smallExplosion, pos, Quaternion.identity)as GameObject;
				Destroy (exp, 2);
				Handheld.Vibrate ();
			}
		}
		if((col.gameObject.tag == "Platform")&&(vibrate)){
			vibrate = false;
			Handheld.Vibrate ();
			cameraScript.CameraBounce ();
		}
	/*	if (col.gameObject.tag == "OtherPlayers") {
			col.gameObject.GetComponent<OtherPlayers> ().GameOver ();
			Handheld.Vibrate ();
		}*/
	}
	void OnCollisionStay(Collision col){
		if (col.gameObject.tag == "Platform") {
			if(cf.force!= Vector3.zero){
				cf.force = Vector3.zero;
			}
		}
	}
	void OnCollisionExit(Collision col)
	{
		if((col.gameObject.tag == "Ramp")&&(!vibrate)){
			vibrate = true;
		}
		if(col.gameObject.tag == "Platform"){
			StartCoroutine (GenerateDownForce (-150, 0.25f));
		}
	}
		

	void OnTriggerEnter(Collider other)
	{
		if ((other.gameObject.tag == "SpeedColl")&&(!speedIncreased)) {
			if (speed <= maxSpeed) {
				speed = speed + speedCollSpeedIncrease;
			}
			speedIncreased = true;
		}
	}
	void OnTriggerExit(Collider other)
	{
		if ((other.gameObject.tag == "SpeedColl")&&(speedIncreased)) {
			speedIncreased = false;
		}
	}

}
