using UnityEngine;
using System.Collections;

public class RandomRotationStrength : MonoBehaviour {

	Rigidbody rb;
	public float FloatStrenght = 0.1f;
	public float RandomRotationStrenght;

	public float CollSpeedMin;
	public float CollSpeedMax;



	void Start()
	{
		FloatStrenght = Random.Range(0.01f, 0.5f);
		RandomRotationStrenght = Random.Range(0.1f, 1f);

		rb = GetComponent<Rigidbody>();

		CollSpeedMin = 50f;
		CollSpeedMax = 400f;
	}

	void OnCollisionEnter(Collision col){
		if (col.gameObject.name == "floor") {
			rb.AddRelativeForce (new Vector3 (0, Random.Range (CollSpeedMin , CollSpeedMax ), Random.Range (CollSpeedMin/2, CollSpeedMax/2)));
		}
	}


	void Update () {
		rb.AddForce(Vector3.up *FloatStrenght);
		transform.Rotate(RandomRotationStrenght,RandomRotationStrenght,RandomRotationStrenght);
	}
}
