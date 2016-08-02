using UnityEngine;
using System.Collections;

public class ClothBehavior : MonoBehaviour {

	private Cloth myCloth;
	public float dauerCountdown = 2.0f;
	float currentDelay;
	float t=0;
	float duration =4f;

	void Start () {
		
		currentDelay = Time.time + dauerCountdown;

		myCloth = GetComponent<Cloth>();
		myCloth.stretchingStiffness = 0.1f;
	
	} 


	void OnCollisionEnter (Collision col){
	//	if (col.gameObject.name == "floor") {
			myCloth.stretchingStiffness = 0.01f;
	
			currentDelay = Time.time + dauerCountdown;
			t = 0;
		
	
	}

	


	void FixedUpdate () {
		
		if  (Time.time > currentDelay) {
			myCloth.stretchingStiffness = Mathf.Lerp (0.01f, 1f, t);
			if (t <= 1)
				t += Time.deltaTime / duration;
		} 


	}


	



}
