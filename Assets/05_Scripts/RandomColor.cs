using UnityEngine;
using System.Collections;

public class RandomColor : MonoBehaviour {

	//public GameObject [] gameObjectsArray;
	public Material [] randomMaterials;
	GameObject [] gameObjectsArray;
	int randomValue;

	void Start (){
		 gameObjectsArray = GameObject.FindGameObjectsWithTag("randomColor");
			foreach (GameObject anObject in gameObjectsArray) { 


			anObject.GetComponent<Renderer> ().material = randomMaterials [Random.Range (0, randomMaterials.Length)];
		}

	}



	// Use this for initialization

	void OnCollisionEnter(Collision col) {


		/////das muss umgeändert werden - dass nur das object die farbe ändert das gerade den floor berührt
		col.gameObject.GetComponent<Renderer> ().material = randomMaterials [Random.Range (0, randomMaterials.Length)];
		if (col.contacts.Length > 1) {

			if (col.gameObject.name == "floor") {
	
				foreach (GameObject anObject in gameObjectsArray) { 

					


					anObject.GetComponent<Renderer> ().material = randomMaterials [Random.Range (0, randomMaterials.Length)];
					//GetComponent<Renderer> ().material.color = Color.yellow;
				}
				Debug.Log (col.collider.name);

			}
		
		}


	}

}
