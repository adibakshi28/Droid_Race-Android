using UnityEngine;
using System.Collections;

public class LaserBehav : MonoBehaviour {

	public GameObject start, end;
	public GameObject particleSys1,particleSys2;
	public bool vertical;
	public AudioClip switchOffAudio;

	[HideInInspector]
	public bool off = false;

	BoxCollider bx;
	AudioSource aud;
	LineRenderer lineRenderer;

	void Start () {
		aud = GetComponent<AudioSource> ();
		bx = GetComponent<BoxCollider> ();
		lineRenderer = GetComponent<LineRenderer>();
		if (!vertical) {
			float bxXSize = start.transform.position.x-end.transform.position.x;
			bx.size = new Vector3 (Mathf.Abs(bxXSize), bx.size.y, bx.size.z);
			float centreX = (start.transform.position.x + end.transform.position.x) / 2;
			bx.center = new Vector3 (centreX, bx.center.y, bx.center.z);
		} 
		else {
			float bxZSize = start.transform.position.z-end.transform.position.z;
			bx.size = new Vector3 (bx.size.x, bx.size.y, Mathf.Abs(bxZSize));
			float centreZ = (start.transform.position.z + end.transform.position.z) / 2;
			bx.center = new Vector3 (bx.size.x, bx.center.y, centreZ);
		}
		lineRenderer.SetPosition(0,start.transform.position);
		lineRenderer.SetPosition(1,end.transform.position);
	}

	public void SwitchOff(){
		bx.enabled = false;
		lineRenderer.enabled = false;
		particleSys1.SetActive (false);
		particleSys2.SetActive (false);
		aud.clip = switchOffAudio;
		aud.Play ();
	}
		
}
