using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTriggerPlate : MonoBehaviour {

	public GameObject parentLAserObject;

	LaserBehav laserScript;

	void Start () {
		laserScript = parentLAserObject.GetComponent<LaserBehav> ();
	}

	void OnTriggerEnter(Collider other)
	{
		if ((other.gameObject.tag == "Player")&&(!laserScript.off)) {
			laserScript.off = true;
			laserScript.SwitchOff ();
		}
	}
}
