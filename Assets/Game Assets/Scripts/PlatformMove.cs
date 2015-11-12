using UnityEngine;
using System.Collections;


// @NOTE the attached sprite's position should be "Top Right" or the children will not align properly
// Strech out the image as you need in the sprite render, the following script will auto-correct it when rendered in the game
[RequireComponent (typeof (SpriteRenderer))]

public class PlatformMove : MonoBehaviour {
	public GameObject platfromPrefab;
	public GameObject baddie;

	public float gridX = 0.0f;
	public float gridY = 0.0f;
	public float scrollSpeed = -2.0f;
	private Vector2 lastChildPosition;
	private Transform lastChildPositioned;

	private Vector3 rightScreenPos;

	SpriteRenderer sprite;
	
	void Awake () {
		rightScreenPos = Camera.main.ViewportToWorldPoint (new Vector3(1, 0, 0));

		sprite = GetComponent<SpriteRenderer>();

		Vector2 spriteSize_wu = new Vector2(sprite.bounds.size.x / transform.localScale.x, sprite.bounds.size.y / transform.localScale.y);
		Vector3 scale = new Vector3(1.0f, 1.0f, 1.0f);

		/*
		 * Generate a row of platforms from a prefab, leave a gap for the player to jump through
		 */
		if (0.0f != gridX) {
			float width_wu = sprite.bounds.size.x / gridX;
			scale.x = width_wu / spriteSize_wu.x;
			spriteSize_wu.x = width_wu;
		}
		
		if (0.0f != gridY) {
			float height_wu = sprite.bounds.size.y / gridY;
			scale.y = height_wu / spriteSize_wu.y;
			spriteSize_wu.y = height_wu;
		}

		float startPosition = sprite.transform.position.x-(sprite.bounds.size.x/2)+(platfromPrefab.GetComponent<SpriteRenderer>().bounds.size.x/2);
		int missingPlatform = Random.Range(0,5);
		float platformSpeed=Random.Range(-0.5f,scrollSpeed);

		for (int j = 0, w = (int)Mathf.Round(sprite.bounds.size.x); j*spriteSize_wu.x < w+platfromPrefab.GetComponent<SpriteRenderer>().bounds.size.x; j++) {
			GameObject child = Instantiate (platfromPrefab) as GameObject;
			child.transform.position = new Vector3 (startPosition + (spriteSize_wu.x * j), sprite.transform.position.y, 0);

			child.transform.localScale = scale;
			child.transform.parent = transform;

			Vector2 newSpeed = child.GetComponent<Rigidbody2D> ().velocity;
			newSpeed.x = platformSpeed;
			lastChildPosition = child.transform.position;
			lastChildPositioned = child.transform;
			child.GetComponent<Rigidbody2D> ().velocity = newSpeed;

			if (j == missingPlatform) {
				child.GetComponent<SpriteRenderer> ().enabled =false;
				child.GetComponent<EdgeCollider2D> ().enabled =false;
			}
		}

		NewBaddie ();
		
		sprite.enabled = false; // Disable this SpriteRenderer and let the prefab children render themselves
	}

	void NewBaddie(){
		Vector3 newPosition = baddie.transform.position;

		newPosition.x = rightScreenPos.x;
		newPosition.y = transform.position.y+1.0f;
		baddie.transform.position = newPosition;

		Vector2 newSpeed = baddie.GetComponent<Rigidbody2D> ().velocity;
		newSpeed.x = Random.Range(-1,-3);

		baddie.GetComponent<Rigidbody2D> ().velocity = newSpeed;
	}

	void MovePlatformRow(){
		transform.position = new Vector2(transform.position.x,transform.position.y+15);

		// change missing platform
		int missingPlatform = Random.Range(0,5);
		int ndx = 0;
		bool showPlatform;

		foreach (Transform child in transform) {
			if(ndx == missingPlatform)
			{
				showPlatform = false;
			}
			else {
				showPlatform =true;
			}

			child.GetComponent<SpriteRenderer> ().enabled =showPlatform;
			child.GetComponent<EdgeCollider2D> ().enabled =showPlatform;
			ndx++;
		}

		NewBaddie ();
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {	
		float cameraBottom = Camera.main.transform.position.y - Camera.main.orthographicSize;

		if(transform.position.y<cameraBottom)
		{
			// Move Row to just above screen
			MovePlatformRow();
		}

		foreach (Transform child in transform)
		{
			SpriteRenderer childSpriteRenderer = child.GetComponent<SpriteRenderer>();

			//child is your child transform
			if(child.transform.position.x+(childSpriteRenderer.bounds.size.x/2)<transform.position.x-sprite.bounds.size.x/2)
			{
				child.transform.position = new Vector2(lastChildPositioned.GetComponent<SpriteRenderer>().transform.position.x+childSpriteRenderer.bounds.size.x,child.transform.position.y) ;
				lastChildPositioned=child;
			}
		}
	}
}


