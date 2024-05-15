using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstical_Ossilate : MonoBehaviour {
	
	public float speed = 100;

	private bool changed = false;

	Rigidbody rb;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		rb.velocity = new Vector3 (speed * Time.deltaTime, 0, 0);
	}

	void OnCollisionEnter(Collision col)
	{
		if ((col.gameObject.tag == "Boundry")&&(!changed)) {
			speed = (-1) * speed;
			rb.velocity = new Vector3 (speed * Time.deltaTime, 0, 0);
			changed = true;
		}
	}
	void OnCollisionExit(Collision col)
	{
		if ((col.gameObject.tag == "Boundry")&&(changed)) {
			changed = false;
		}
	}
}