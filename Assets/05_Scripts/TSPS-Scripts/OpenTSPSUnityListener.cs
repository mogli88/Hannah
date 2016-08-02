/**
 * OpenTSPS + Unity3d Extension
 * Created by James George on 11/24/2010
 * 
 * This example is distributed under The MIT License
 *
 * Copyright (c) 2010 James George
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TSPS;

public class OpenTSPSUnityListener : MonoBehaviour, OpenTSPSListener  {
	
	public int port = 12000; //set this from the UI to change the port
	
	//create some materials and apply a different one to each new person
	public Material	[] materials;
	
	private OpenTSPSReceiver receiver;
	//a place to hold game objects that we attach to people, maps person ID => their object
	private Dictionary<int,GameObject> peopleCubes = new Dictionary<int,GameObject>();


	public GameObject boundingPlane; //put the people on this plane
	public GameObject [] objectArray; //Hier werden alle Objekte gespeichert
	int randomValue; //aktuelles, ufälliges Objekt
	GameObject personObject;

	public GameObject cam1;
	public GameObject cam2;




	void Start() {
		cam1.SetActive (true);
		cam2.SetActive (false);

		receiver = new OpenTSPSReceiver( port );
		receiver.addPersonListener( this );
		Security.PrefetchSocketPolicy("localhost",8843);
		receiver.connect();
		//Debug.Log("created receiver on port " + port);

		 //cam1 = GameObject.Find("Camera_Vogel_Orbit");
		//cam2 = GameObject.Find("FreeLookCameraRig");
	}
			
	void Update () {
		//call this to receiver messages
		receiver.update();


//////////// Switches Cameras

		if (personObject == null) {
			
			cam1.SetActive (true);
			cam2.SetActive (false);
			Debug.Log ("kein objekt");
		}
		else {
			cam1.SetActive (false);
			cam2.SetActive (true);

		}
	
	}
	
	
	void OnGUI() {
		if( receiver.isConnected() ) {
			GUI.Label( new Rect( 10, 10, 500, 100), "Connected to TSPS on Port " + port );
		}
	}
	
	public void personEntered(OpenTSPSPerson person){
	//	Debug.Log(" person entered with ID " + person.id);

	//checkt ob mindest ein Objekt im Array integriert ist
		if (objectArray.Length != 0 && objectArray [randomValue] != null) {
			///zufälliger Wert je nach Länge des Arrays
			randomValue = Random.Range (0, objectArray.Length);
					
			personObject = Instantiate (objectArray [randomValue], positionForPerson (person), Quaternion.identity) as GameObject;
			personObject.GetComponent<Renderer> ().material = materials [person.id % materials.Length];
			peopleCubes [person.id] = personObject;

			Debug.Log (objectArray [randomValue].transform);
		}
	}

	public void personUpdated(OpenTSPSPerson person) {
		//don't need to handle the Updated method any differently for this example
		personMoved(person);
	}

	public void personMoved(OpenTSPSPerson person){
		//Debug.Log("Person updated with ID " + person.id);
		if(peopleCubes.ContainsKey(person.id)){
			GameObject cubeToMove = peopleCubes[person.id];
			cubeToMove.transform.position = positionForPerson(person);
		}
	}

	public void personWillLeave(OpenTSPSPerson person){
		//Debug.Log("Person leaving with ID " + person.id);
		if(peopleCubes.ContainsKey(person.id)){
			GameObject cubeToRemove = peopleCubes[person.id];
			peopleCubes.Remove(person.id);
			//delete it from the scene	
			Destroy(cubeToRemove);
		}
	}
	
	//maps the OpenTSPS coordinate system into one that matches the size of the boundingPlane
	private Vector3 positionForPerson(OpenTSPSPerson person){
		Bounds meshBounds = boundingPlane.GetComponent<MeshFilter>().sharedMesh.bounds;
		return new Vector3( (float)(.5 - person.centroidX) * meshBounds.size.x,objectArray[randomValue].transform.position.y, (float)(person.centroidY - .5) * meshBounds.size.z );
	}
}
