using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class SceneManager : MonoBehaviour {
	public SharkPlayer player;
	private int score=0;
	Text scoreText;
	Text hiScoreText;

	// Use this for initialization
	void Start () {
		GameObject uiObject = GameObject.Find ("scoreText");
		if (uiObject != null) {
			scoreText = uiObject.GetComponent<Text> ();

			scoreText.text = "Score: 0";
		}
		uiObject = GameObject.Find ("hiScoreText");
		if (uiObject != null) {
			hiScoreText = uiObject.GetComponent<Text> ();
			
			hiScoreText.text = "Hi-Score: 0";
		}

		UpdateUI ();
	}

	public void RestartLevel(){
		Application.LoadLevel(Application.loadedLevelName);
	}

	public int checkAndSetHighScore()
	{
		int currentHighScore = PlayerPrefs.GetInt ("High Score");
		
		if (score > currentHighScore) {
			PlayerPrefs.SetInt ("High Score", score);
			return score;
		}
		return currentHighScore;
	}

	public void updateScore(int value){
		score+=value;
		UpdateUI();
	}

	public void UpdateUI(){
		if (scoreText != null) {
			scoreText.text = "Score: "+score;
			hiScoreText.text = "Hi-Score: "+checkAndSetHighScore();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp (0))
		{
			player.Jump ();
		}
	}

 }
