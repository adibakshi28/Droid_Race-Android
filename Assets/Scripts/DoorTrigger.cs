using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour {

	private bool open = false;

	Animator anim;

	void Start () {
		anim = GetComponent<Animator> ();
	}

	void OnTriggerEnter(Collider other)
	{
		if ((other.gameObject.tag == "Player")&&(!open)) {
			open = true;
			anim.SetTrigger ("Open");
		}
	}

}
