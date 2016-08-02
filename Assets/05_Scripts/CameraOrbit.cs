using UnityEngine;
using System.Collections;

public class CameraOrbit : MonoBehaviour {

	//GameObject[] ObjectList = null;
	//int index = 0;


	public GameObject  target;

	public bool orbitY = false;

	public float zoomMaxY = 0;
	public float zoomMinY = 0;
	public float zoomSpeed = 4.5f;
	public float zoomTime = 0.025f;
	private Vector3 zoomDestination;


	// Use this for initialization
	void Start () {
		zoomDestination = target.transform.position;	
	
	}

	private void ZoomCamera(){
		float moveY = Input.GetAxis ("Mouse Y");

		if (moveY != 0)
			zoomDestination = transform.position + (transform.forward * moveY) * zoomSpeed;
			
		if (zoomDestination != target.transform.position && zoomDestination.y < zoomMaxY && zoomDestination.y > zoomMinY )
		{
			transform.position = Vector3.Lerp (transform.position, zoomDestination, zoomTime);
			
			if (transform.position == zoomDestination)
				zoomDestination = target.transform.position;
		}


		if (transform.position.y > zoomMaxY)
			transform.position = new Vector3 (transform.position.x, zoomMaxY, transform.position.z);
		if (transform.position.y < zoomMinY)
			transform.position = new Vector3 (transform.position.x, zoomMinY, transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		//ZoomCamera();
		if (target!= null) {

			transform.LookAt (target.transform);
			if (orbitY) {
				transform.RotateAround (target.transform.position, new Vector3 (0, 1, 0), Time.deltaTime * 3);
			}

		}

	}
}
