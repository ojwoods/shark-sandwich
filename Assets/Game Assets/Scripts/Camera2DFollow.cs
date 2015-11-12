using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class Camera2DFollow : MonoBehaviour
    {
		public float interpVelocity;
		public float minDistance;
		public float followDistance;
		public float bottomOfScreen;
		public GameObject target;
		public Vector3 offset;
		Vector3 targetPos;
		private Vector3 startPosition;

		// Use this for initialization
		void Start () {

			// Make sure camera has player and ground at bottom of the screen
			float cameraBottom = Camera.main.transform.position.y - Camera.main.orthographicSize;
			float moveScreenY = bottomOfScreen - cameraBottom;
			startPosition = new Vector3 (transform.position.x, moveScreenY, transform.position.z);
			transform.position = startPosition;
			targetPos = transform.position;
		}
		
		// Update is called once per frame
		void FixedUpdate () {
			float cameraBottom = Camera.main.transform.position.y - Camera.main.orthographicSize;
		
			// If player is above certain point, move the screen to follow
			if (target && (target.transform.position.y>cameraBottom))
				{
				Vector3 posNoZ = transform.position;
				posNoZ.z = target.transform.position.z;
				
				Vector3 targetDirection = (target.transform.position - posNoZ);
				
				interpVelocity = targetDirection.magnitude * 5f;				
				targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime); 

				targetPos.x=0;
				if(targetPos.y<startPosition.y)
				{
					targetPos.y = startPosition.y;
				}

				transform.position = Vector3.Lerp( transform.position, targetPos + offset, 0.25f);
			}
		}
	}
}