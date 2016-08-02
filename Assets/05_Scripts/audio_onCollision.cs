using UnityEngine;
using System.Collections;

public class audio_onCollision : MonoBehaviour {

	public AudioClip clip1;
	public AudioClip clip2;


	private AudioSource source;
	private float lowPitchRange = .75F;
	private float highPitchRange = 1.5F;
	private float velToVol = .2F;
	public float velocityClipSplit = 10F; //choose which audio clip to pick


	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource>();
	}

	void OnCollisionEnter (Collision coll)
	{
		source.pitch = Random.Range (lowPitchRange,highPitchRange);
		float hitVol = coll.relativeVelocity.magnitude * velToVol;
		if (coll.relativeVelocity.magnitude < velocityClipSplit)//wie stark die kollision ist.
			source.PlayOneShot(clip1,hitVol);
		else 
			source.PlayOneShot(clip2,hitVol);
	//	Debug.Log ("hitVol");
	}

}
