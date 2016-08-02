using UnityEngine;
using System.Collections;
using Windows.Kinect;


public class Gestensteuerung_2Personen : MonoBehaviour {

	public GameObject BodySrcManager; 
	public JointType TrackedJoint; 
	public float multiplier = 20f;
	private BodySourceManager bodyManager;
	private Body[] bodies;

	void Start(){
		if (BodySrcManager == null) {
			//Debug.Log ("");
		} 
		else {
			bodyManager = BodySrcManager.GetComponent<BodySourceManager> ();
		}
	}

	void Update (){
		if (bodyManager == null) {
			return;
		}
	
		bodies = bodyManager.GetData();

		if (bodies == null){
			return;
		}

		foreach (var body in bodies){
			if (body == null){
				continue;
			}
			if (body.IsTracked){
				var pos = body.Joints[TrackedJoint].Position;
				gameObject.transform.position= new Vector3(pos.X *multiplier,pos.Y*multiplier);
			}
		}

	}

}
