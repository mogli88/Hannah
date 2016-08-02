using UnityEngine;
using System.Collections;

public class Attractor : MonoBehaviour {

	Transform target;
	public float speedAnziehung;
	 
	private int x;
	private Vector3 position;
	public int force;

	Rigidbody rb;

	void Start() 
	{	

		GameObject targetObject = GameObject.Find("collisionSphere");
		targetObject.SetActive (true);
		target = targetObject.GetComponent<Transform>();
	
		//force = false;
		rb = GetComponent<Rigidbody>();
		speedAnziehung = (Random.Range (1, 5));
	
		x = Random.Range(-10, 10);

		force = 150;
	}
		
	void OnCollisionEnter (Collision col){
		//print (col.contacts.Length);
		if (col.gameObject.name == "collisionSphere") {
			rb.AddForce( new Vector3(0, 0, Random.Range(5, 15.0F)*force));
			//	rb.transform.localScale += new Vector3(1,1,10);
		} 

		if (col.gameObject.name == "decke") {
			rb.AddForce( new Vector3(0, -10*force,0 ));

			//	rb.transform.localScale += new Vector3(1,1,10);
		} 

		if (col.contacts.Length > 3) {
			rb.AddForce( new Vector3(0, force, x));
		
		}
	}


	void Update() {
		//Scaling ();
		float step = speedAnziehung * Time.deltaTime;
		rb.transform.position = Vector3.MoveTowards (transform.position, target.position, step);
	}

}
