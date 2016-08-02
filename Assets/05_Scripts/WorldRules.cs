using UnityEngine;
using System.Collections;

public class WorldRules : MonoBehaviour {

	public bool cursorVis; 



	void Start(){
		
	}


	void Update() {

		if(cursorVis == false){
			Cursor.visible = false;
		}

		if(cursorVis ==true){
			Cursor.visible = true;
		}




	}
}
		
