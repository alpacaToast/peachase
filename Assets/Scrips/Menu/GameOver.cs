using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

	public Text score;
	public ScoreStorage storage;

	private Narration narrator;

	private Vector2 touchStartPos;
	private Vector2 touchDirection;

	// Use this for initialization
	void Start () {
		narrator = GameObject.Find ("Narrator").GetComponent<Narration> ();
		narrator.StartNarration ("GameOver");
		score.text = storage.score.ToString();
	}

	// Update is called once per frame
	void Update () {
		var xInput = Input.GetAxisRaw ("Horizontal");
		var yInput = Input.GetAxisRaw ("Vertical");

		if (xInput > 0) 
		{

			SceneManager.LoadScene ("Main");
		}
		else if (xInput < 0)
		{
			SceneManager.LoadScene("Menu");
		}

		if (Input.touchCount == 1) {

			Touch touch = Input.GetTouch (0);
			switch (touch.phase) 
			{
			case TouchPhase.Began:
				touchStartPos = touch.position;
				break;
			case TouchPhase.Moved:
				touchDirection = touch.position - touchStartPos;
				break;
			case TouchPhase.Ended:
				ChooseInput ();
				break;
			}
		}
	}

	void ChooseInput() 
	{
		if (touchDirection.magnitude > 100) 
		{
			var normTouchDirection = touchDirection.normalized;
			float absoluteDirection = Mathf.Abs (Vector2.Dot (normTouchDirection, Vector2.right));
			if ( absoluteDirection > .7) {
				if (normTouchDirection.x < 0) {
					narrator.Stop ();
					SceneManager.LoadScene ("Menu");
				} else if(normTouchDirection.x > 0){
					narrator.Stop ();
					SceneManager.LoadScene ("Main");
				}
			}
		}
	}
}
