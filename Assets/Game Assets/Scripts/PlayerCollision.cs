using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour {
	private SharkPlayer mainPlayerObject;

	// Use this for initialization
	void Start () {
		mainPlayerObject = GameObject.FindObjectOfType<SharkPlayer> ();
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		Debug.Log ("DIED!!!");
		mainPlayerObject.PlayerDied ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
