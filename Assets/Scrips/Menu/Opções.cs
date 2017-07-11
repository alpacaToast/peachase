﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Opções : MonoBehaviour {

	private Vector2 touchStartPos;
	private Vector2 touchDirection;

	private Narration narrator;

	void Start()
	{
		narrator = GameObject.Find ("Narrator").GetComponent<Narration> ();
		narrator.StartNarration ("Opcoes");
	}

	void Update()
	{
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
			if ( absoluteDirection< .3) {
				if (normTouchDirection.y < 0) 
				{
					ToogleNarration ();
				} 
				else if(normTouchDirection.y > 0)
				{
					
				}
			}
			else if (absoluteDirection > .7) 
			{
				if (normTouchDirection.x < 0) 
				{
					narrator.Stop ();
					SceneManager.LoadScene ("Menu");
				}
				else if( normTouchDirection.x > 0)
				{
					narrator.Stop ();
					Debug.Log ("Mudar dificuldade");
				}
			}
		}
	}

	void ToogleNarration()
	{
		narrator.Stop ();
		narrator.disabled = (narrator.disabled)? false: true;
	}
}
