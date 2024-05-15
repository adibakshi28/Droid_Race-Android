using UnityEngine;
using System.Collections;

public class RotateSkybox : MonoBehaviour {

	public float speed;
	private float rotation; 
	private Skybox skybox;

	void Start () {
		skybox = GetComponent<Skybox> ();
	}
	void Update () {
		rotation -= Time.deltaTime*speed;
		skybox.material.SetFloat ("_Rotation", rotation);
	}
}