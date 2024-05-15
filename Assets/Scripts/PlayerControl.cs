using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

	public float controlSpeed = 10;

	private Vector3 fp;   //First touch position
	private Vector3 lp;   //Last touch position
	private float dragDistance;  //minimum distance for a swipe to be registered

	float touchDuration;
	Touch touch;

	LevelController levelController;
	Rigidbody rb;

	void Start(){
		levelController = GameObject.FindGameObjectWithTag ("LevelController").GetComponent<LevelController> ();
		rb = GetComponent<Rigidbody> ();
		dragDistance = 0;
	}

	void FixedUpdate ()
	{
		if ((levelController.startGame) && (!levelController.gameOver)) {
			if (Input.touchCount == 1) { // user is touching the screen with a single touch
				Touch touch = Input.GetTouch (0); // get the touch
				if (touch.phase == TouchPhase.Began) { //check for the first touch
					fp = touch.position;
					lp = touch.position;

				} else if (touch.phase == TouchPhase.Moved) { // update the last position based on where they moved
					lp = touch.position;

					if (Mathf.Abs (lp.x - fp.x) > dragDistance || Mathf.Abs (lp.y - fp.y) > dragDistance) {
						if (Mathf.Abs (lp.x - fp.x) > Mathf.Abs (lp.y - fp.y)) {   
							float pixilChange = lp.x - fp.x;
							float shift = (15f * pixilChange) / Screen.width;
							Vector3 pos = transform.position;
							pos.x = pos.x + shift;
							transform.position = Vector3.MoveTowards(transform.position,pos,controlSpeed*Time.deltaTime);
							fp = lp;

						} 
					} 
				} 
			}
		}
	}

	void Update() {
		if ((levelController.startGame) && (!levelController.gameOver)) {
			if(Input.touchCount > 0){ //if there is any touch
				touchDuration += Time.deltaTime;
				touch = Input.GetTouch(0);

				if(touch.phase == TouchPhase.Ended && touchDuration < 0.45f) //making sure it only check the touch once && it was a short touch/tap and not a dragging.
					StartCoroutine("singleOrDouble");
			}
			else
				touchDuration = 0.0f;
		}
	}

	IEnumerator singleOrDouble(){
		yield return new WaitForSeconds (0.4f);
		if (touch.tapCount == 1)
			Debug.Log ("Single");
		else if (touch.tapCount == 2) {
			//this coroutine has been called twice. We should stop the next one here otherwise we get two double tap
			StopCoroutine ("singleOrDouble");
			levelController.PowerBtn ();
		}
	}


	void LateUpdate(){
		Vector3 pos = transform.position;
		pos.x =  Mathf.Clamp(transform.position.x, -2.4f, 2.4f);
		transform.position = pos;
	}
}