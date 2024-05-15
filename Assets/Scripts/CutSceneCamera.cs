using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneCamera : MonoBehaviour {

	Animator anim;

	void Start () {
		anim = GetComponent<Animator> ();
		anim.SetTrigger ("CutScene");
	}
		
}
