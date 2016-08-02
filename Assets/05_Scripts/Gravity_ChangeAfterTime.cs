 using UnityEngine;
using System.Collections;

public class Gravity_ChangeAfterTime : MonoBehaviour {

	Vector3 gravity;
	bool gravityOnEnter;

	public float dauerGravityPositiv;
	public float dauerGravityNegativ; 
	private float gravityNegativ;
	float currentDelay;
	float currentDelay2;
 

	void Start()
	{
		dauerGravityPositiv = 4.0f;
		dauerGravityNegativ = 4.0f; 
		gravity = Physics.gravity;
		gravity.y = -2;
		gravityNegativ = dauerGravityPositiv + dauerGravityNegativ;
	}

	void OnCollisionEnter (Collision col){
		if (col.gameObject.name == "floor") {
			gravityOnEnter = true;
			currentDelay = Time.time + dauerGravityPositiv;
			currentDelay2 = Time.time + gravityNegativ;
		} 
	}
		

		
	void FixedUpdate()
	{
		Physics.gravity = gravity;

		//checkCollisionExit ();
		if(gravityOnEnter)
		{
	
			if (Time.time > currentDelay) {
				//GetComponent<Renderer> ().material.color = Color.yellow;
				gravity.y =3f;
			}


			if (Time.time > currentDelay2) {//wenn mehr Zeit vergangen ist als Delaytime
				gravity.y = -3;
			}
		

		}

	}
		

}