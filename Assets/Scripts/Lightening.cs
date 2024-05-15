using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightening : MonoBehaviour {

	public GameObject colliderObject,startObject,endObject;

	BoxCollider bxCol;

	void Start () {
		bxCol = colliderObject.GetComponent<BoxCollider> ();

		Vector3 pos = colliderObject.transform.localPosition;
		pos.x = 0;
		pos.y = 0;
		pos.z = (endObject.transform.localPosition.z + startObject.transform.localPosition.z) / 2;
		colliderObject.transform.localPosition = pos;
		Vector3 sz = bxCol.size; 
		sz.z = endObject.transform.localPosition.z - startObject.transform.localPosition.z; 
		bxCol.size = sz;
	}

}
