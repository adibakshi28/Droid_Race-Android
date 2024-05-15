using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

	public int scoreInc = 10;
	public GameObject mesh;
	private bool scoreIncreased = false;

	AudioSource aud;
	Light lt;
	LevelController levelController;

	void Start () {
		levelController = GameObject.FindGameObjectWithTag ("LevelController").GetComponent<LevelController> ();
		lt = GetComponent<Light> ();
		aud = GetComponent<AudioSource> ();
	}

	void OnTriggerEnter(Collider other)
	{
		if ((other.gameObject.tag == "Player")&&(!scoreIncreased)) {
			scoreIncreased = true;
			levelController.ScoreIncrease (scoreInc);
			levelController.PowerSliderIncrease (1);
			Destroy (mesh);
			lt.enabled = true;
			aud.Play ();
			Destroy (this.gameObject, 1);
		}
	}

}
