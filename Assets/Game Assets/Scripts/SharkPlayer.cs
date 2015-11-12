using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class SharkPlayer : MonoBehaviour {

	public Vector2 speed = new Vector2(10,10);
	public float maxVelocity;
	public float jumpForce =50f;
	private float lockPos = 0;

	private Vector3 leftScreenPos;
	private Vector3 rightScreenPos;

	private string currentPlatformName="";
	private GameObject legs;

	private bool isWrappingX = false;
	private bool isGrounded = true;
	private bool isJumping = false;
	private bool isDoubleJump =false;
	private bool isDead = false;

	private SceneManager sceneManager= null;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.

	// Use this for initialization
	void Start () {
		sceneManager = GameObject.FindObjectOfType<SceneManager> ();
		leftScreenPos = Camera.main.ViewportToWorldPoint (new Vector3(0, 0, 0));
		rightScreenPos = Camera.main.ViewportToWorldPoint (new Vector3(1, 0, 0));
		legs = transform.Find ("male_leg").gameObject;
	}


	void ScreenWrap(){
		Vector3 wrld = Camera.main.WorldToViewportPoint(transform.position);
		Vector3 newPosition = transform.position;
		
		if (wrld.x < 0) {
			newPosition.x = rightScreenPos.x;
			transform.position = newPosition;
		}
		else if (wrld.x > 1) {
			newPosition.x = leftScreenPos.x;
			transform.position = newPosition;
		}
	}

	public void PlayerDied()
	{
		if (!isDead) {
			isDead = true;
			Invoke ("Reset", 3);
		}
	}

	void Reset(){
		sceneManager.RestartLevel ();
	}

	// Update is called once per frame
	void Update () {
		ScreenWrap ();
	}


	public void Jump() {
		if ((isGrounded && !isJumping)) {
			GetComponent<Rigidbody2D> ().AddForce(new Vector2(0f, jumpForce));
			isJumping=true;
		}
	}

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void FixedUpdate()
	{
		float inputX = CrossPlatformInputManager.GetAxis ("Horizontal");
		bool groundTest = false;

		if (inputX == 0) {
			inputX = Input.GetAxis ("Horizontal");
		}

		/* Raycast from the player to see if touching a platform (on the ground) */
		Collider2D[] colliders = Physics2D.OverlapCircleAll(legs.transform.position, 0.1f);
		for (int i = 0; !groundTest && i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != legs)
			{
				if(colliders[i].gameObject.transform.parent && currentPlatformName!=colliders[i].gameObject.transform.parent.name)
				{
					if(currentPlatformName!="")
					{
						sceneManager.updateScore(100);
					}
					currentPlatformName=colliders[i].gameObject.transform.parent.name;
				}
				groundTest = true;
			}	
		}

		// If the player is touching the ground and moving down, then not still jumping
		if (groundTest && GetComponent<Rigidbody2D>().velocity.y<0) {
			isJumping=false;
		}

		if (!isGrounded && groundTest) {
			//transform.rotation = Quaternion.Euler(lockPos, lockPos, lockPos);
		}

		isGrounded = groundTest;

		// Limit velocity
		GetComponent<Rigidbody2D>().velocity = new Vector2(speed.x*inputX, GetComponent<Rigidbody2D>().velocity.y);

		if(GetComponent<Rigidbody2D>().velocity.sqrMagnitude > maxVelocity)
		{
			//smoothness of the slowdown is controlled by the 0.99f, 
			//0.5f is less smooth, 0.9999f is more smooth
			GetComponent<Rigidbody2D>().velocity *= 0.99f;
		}

		if (Input.GetKeyDown ("space") || Input.GetKey(KeyCode.UpArrow)){
			Jump();
		} 

		if (inputX > 0.2 && !m_FacingRight)
		{
			// ... flip the player.
			Flip();
		}

		// Otherwise if the input is moving the player left and the player is facing right...
		else if (inputX < -0.2 && m_FacingRight)
		{
			// ... flip the player.
			Flip();
		}

		if (groundTest && !isDead) {
			//transform.rotation = Quaternion.Euler(lockPos, lockPos, lockPos);
		}
	}
}
