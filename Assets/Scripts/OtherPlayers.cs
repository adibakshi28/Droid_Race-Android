using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayers : MonoBehaviour {

	public float maxSpeed,minSpeed,speedCollSpeedIncrease = 2,decelerationRate = 0.4f;
	public GameObject explosion,frontCollider,body,bottomSphereForTrail;
//	public GameObject trailingObject;
	public bool reverseTrack = false; 

	private bool gameOver = false, trackChanged = false, speedIncreased = false;
	private int currentTrackIndex;
	public float speed;
//	private GameObject trailObj;

	int n = 0;

	Rigidbody rb;
	ConstantForce cf;

	Player playerScript;
	LevelController levelController;

	void Start () {
		levelController = GameObject.FindGameObjectWithTag ("LevelController").GetComponent<LevelController> ();
		playerScript = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();

		rb = GetComponent<Rigidbody> ();
//		mesh = body.GetComponent<MeshRenderer> ();
		cf = GetComponent<ConstantForce> ();

		levelController.noOfOtherPlayerInGame++;
		speed = Random.Range (minSpeed,maxSpeed);
//		int matIndex = Random.Range (0, mats.Count);
//		mesh.material = mats [matIndex];
		currentTrackIndex = (int)transform.position.x;
		Vector3 pos = transform.position;
		pos.y = pos.y - transform.localScale.y/2 + 0.2f;
//		trailObj = Instantiate(trailingObject,pos,Quaternion.identity);
//		trailObj.GetComponent<PlayerTrail> ().player = bottomSphereForTrail;
	}


	void Update(){
		if (transform.position.y < -4f) {        // if it falls down then destroy it
			GameOver ();
		}
		if (speed > minSpeed) {            // Deceleration of speed
			speed = speed - (decelerationRate * Time.deltaTime);
		}
		else {
			speed = minSpeed;
		}
	}

	void FixedUpdate(){
		rb.velocity = new Vector3 (0, 0, speed);
	}


	IEnumerator ChangeTrack(){
		n++;
		Vector3 pos;
		pos = frontCollider.transform.position;
		if (!reverseTrack) {
			for (int x = -2; x < 3; x++) {
				pos.x = x;
				frontCollider.transform.position = pos;
				yield return new WaitForSeconds(0.02f);
				if (!trackChanged) {
					break;
				}
			}
		}
		else {
			for (int x = 2; x > -3; x--) {
				pos.x = x;
				frontCollider.transform.position = pos;
				yield return new WaitForSeconds(0.02f);
				if (!trackChanged) {
					break;
				}
			}
		}
		float StepLength = 0.175f;
		int steps = (int)((float)(Mathf.Abs (pos.x - transform.position.x)) / StepLength);
		for(int i =0;i<steps;i++){
			if (transform.position.x > pos.x) {
				Vector3 p = transform.position;
				p.x = p.x - StepLength;
				transform.position = p;
				yield return new WaitForSeconds (0.015f);
			}
			else {
				Vector3 p = transform.position;
				p.x = p.x + StepLength;
				transform.position = p;
				yield return new WaitForSeconds (0.015f);
			}
		}
		frontCollider.GetComponent<BoxCollider> ().enabled = false;
		Vector3 po = transform.position;
		po.x = pos.x;
		transform.position = po;
		frontCollider.transform.localPosition = Vector3.zero;
		frontCollider.GetComponent<BoxCollider> ().enabled = true;
	}

	public void GameOver(){
		gameOver = true;
		levelController.noOfOtherPlayerInGame--;
		GameObject exp;
		exp = Instantiate (explosion, transform.position, Quaternion.identity)as GameObject;
		Destroy (exp, 2);
//		Destroy (trailObj);
		Destroy (this.gameObject);
	}

	void OnCollisionEnter(Collision col)
	{
		if ((col.gameObject.tag == "Enemy") && (!gameOver)) {
			GameOver ();
		}
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
		if(col.gameObject.tag == "Platform"){
			cf.force = new Vector3 (0, -120, 0);
		}
	}


	void OnTriggerEnter(Collider other)
	{
		if ((other.gameObject.tag == "Enemy")&&(!trackChanged)) {
			trackChanged = true;
			StartCoroutine (ChangeTrack ());
		}
		if ((other.gameObject.tag == "SpeedColl")&&(!speedIncreased)) {
			if (speed <= maxSpeed) {
				speed = speed + speedCollSpeedIncrease;
			}
			speedIncreased = true;
		}
	}
	void OnTriggerExit(Collider other)
	{
		if ((other.gameObject.tag == "Enemy")&&(trackChanged)) {
			trackChanged = false;
		}
		if ((other.gameObject.tag == "SpeedColl")&&(speedIncreased)) {
			speedIncreased = false;
		}
	}
}
