using System;
using System.Collections;
using UnityEngine;

namespace UnityStandardAssets.Cameras
{
    public abstract class AbstractTargetFollower : MonoBehaviour
    {


        public enum UpdateType // The available methods of updating are:
        {
            FixedUpdate, // Update in FixedUpdate (for tracking rigidbodies).
            LateUpdate, // Update in LateUpdate. (for tracking objects that are moved in Update)
            ManualUpdate, // user must call to update camera
        }

        [SerializeField] protected Transform m_Target; 
		[SerializeField] protected GameObject m_Target1; // The target object to follow
		[SerializeField] protected GameObject m_Target2; // The second target object to follow
		[SerializeField] protected GameObject m_Target3; 
		[SerializeField] protected GameObject m_Target4;

		public Windows.Kinect.JointType partToTrack1;
		public GameObject BodySrcManager;
		private BodySourceManager bodyManager;

		public Transform movementObject1;


        [SerializeField] private bool m_AutoTargetPlayer = true;  // Whether the rig should automatically target the player.
        [SerializeField] private UpdateType m_UpdateType;         // stores the selected update type

		Transform target; 

        protected Rigidbody targetRigidbody;
	

		void Awake (){
		
			//GameObject targetObject = GameObject.Find("collisionSphere");
			//targetObject.SetActive (true);
			//target = targetObject.GetComponent<Transform>();
		
		}

		protected virtual void Start()   {
			// if auto targeting is used, find the object tagged "Player"
			// any class inheriting from this should call base.Start() to perform this action!
			if (m_AutoTargetPlayer) {
				FindAndTargetPlayer ();
			}
			if (m_Target == null)
				return;
			targetRigidbody = m_Target.GetComponent<Rigidbody> ();

			InvokeRepeating ("SwitchTarget", 5.0f, 5.0f);//sek wann zum 1. wiederholt wird, sek die danach immer wieder wiederholt werden; 

		
			if (BodySrcManager == null) {
				Debug.Log ("BodySourceManager bei Kamera einfügen");
			} 
			else {
				bodyManager = BodySrcManager.GetComponent<BodySourceManager> ();
			}


        }




		void SwitchTarget()
		{

			int x = UnityEngine.Random.Range(0,4);
			Debug.Log (x);

			if (x == 0) {
				m_Target =  m_Target1.transform; 
			}

			if (x == 1) {
				m_Target =  m_Target2.transform; 
			}

			if (x == 2) 		{
				m_Target =  m_Target3.transform; 
			}

			if (x == 3) {
				m_Target =  m_Target4.transform; 
			}

		}



        private void FixedUpdate()
        {
			if (Input.GetKey("up")){
				m_Target =  m_Target2.transform; 
			}

		if (Input.GetKey("down")){
				
			




			if (bodyManager == null){
				return;
			}

			Windows.Kinect.Body[] data = bodyManager.GetData();
			if (data == null) {
				return;
			}

			int counter = 0; 

			foreach (var body in data) {
				counter++;

				if (body == null) {
					continue;
				}

				if (body.IsTracked) {
					var pos = body.Joints [partToTrack1].Position;  
				
					//Mit Gravitation 
					movementObject1.position = (new Vector3 (pos.X, movementObject1.transform.position.y, -pos.Z));
						m_Target =  movementObject1.transform; 

					break;
						Debug.Log ("Targeting Is Working");

				}
			}
			
			}








		
            // we update from here if updatetype is set to Fixed, or in auto mode,
            // if the target has a rigidbody, and isn't kinematic.
            if (m_AutoTargetPlayer && (m_Target == null || !m_Target.gameObject.activeSelf))
            {
                FindAndTargetPlayer();
            }
            if (m_UpdateType == UpdateType.FixedUpdate)
            {
                FollowTarget(Time.deltaTime);
            }
        }


        private void LateUpdate()
        {
			if (Input.GetKey("up")){
				m_Target =  m_Target2.transform; 
			}

			if (Input.GetKey("down")){
				//m_Target2.transform =  m_Target; 
				Debug.Log ("down");
			}

            // we update from here if updatetype is set to Late, or in auto mode,
            // if the target does not have a rigidbody, or - does have a rigidbody but is set to kinematic.
            if (m_AutoTargetPlayer && (m_Target == null || !m_Target.gameObject.activeSelf))
            {
                FindAndTargetPlayer();
            }
            if (m_UpdateType == UpdateType.LateUpdate)
            {
                FollowTarget(Time.deltaTime);
            }
        }


        public void ManualUpdate()
        {
            // we update from here if updatetype is set to Late, or in auto mode,
            // if the target does not have a rigidbody, or - does have a rigidbody but is set to kinematic.
            if (m_AutoTargetPlayer && (m_Target == null || !m_Target.gameObject.activeSelf))
            {
                FindAndTargetPlayer();
            }
            if (m_UpdateType == UpdateType.ManualUpdate)
            {
                FollowTarget(Time.deltaTime);
            }
        }

        protected abstract void FollowTarget(float deltaTime);


        public void FindAndTargetPlayer()
        {
            // auto target an object tagged player, if no target has been assigned
            var targetObj = GameObject.FindGameObjectWithTag("Player");
            if (targetObj)
            {
                SetTarget(targetObj.transform);
            }
        }


        public virtual void SetTarget(Transform newTransform)
        {
            m_Target = newTransform;
        }


        public Transform Target
        {
            get { return m_Target; }
        }
    }
}
