using UnityEngine;
using System.Collections;

public class MovementTracker : MonoBehaviour {

	public Windows.Kinect.JointType partToTrack1; 
	//public Windows.Kinect.JointType partToTrack2;
	public float posFactor; 
	public float lowPassFactor = 10.1f;
	private Vector3 currentPos1;
	//private Vector3 currentPos2;
	BodySourceManager bodySourceManager;

	public Transform movementObject1;
	//public Transform movementObject2;


	void Start () {
		bodySourceManager = GetComponent<BodySourceManager> ();
		posFactor = 10f;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (bodySourceManager == null){
			return;
		}

		Windows.Kinect.Body[] data = bodySourceManager.GetData();
		if (data == null) {
			return;
		}

		int counter = 0; 

		foreach (var body in data) {
			counter++;

			if (body == null){
				continue;
			}

			if (body.IsTracked){
				var pos = body.Joints[partToTrack1].Position;  
				Debug.Log("Tracking Is Working");

				//Ohne Gravitation 
				//CalcLowPassValues (new Vector3 (pos.X, pos.Y, -pos.Z), ref currentPos1);
				//movementObject1.position = currentPos1 * posFactor;

				//Mit Gravitation 
				movementObject1.position =(new Vector3 (pos.X,movementObject1.transform.position.y, -pos.Z));


				/*
				 * pos =  body.Joints[partToTrack2].Position; 
				CalcLowPassValues (new Vector3 (pos.X, pos.Y, -pos.Z), ref currentPos2);
				movementObject2.position = currentPos2 * posFactor;
				*/

				break;

			}
				
		}
	}



	void CalcLowPassValues (Vector3 newPos, ref Vector3 resultPos){
		resultPos = Vector3.Lerp (resultPos, newPos, lowPassFactor);
	}
}
