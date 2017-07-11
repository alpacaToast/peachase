using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
	public AudioMixerGroup mixer;

    private Player player;
    public Image image;
    public bool paused;

    public Sprite gameOver;
    public bool isGameOver;

    private Vector2[] touchStartPos;
    private Vector2[] touchDirection;
    private bool[] touchEnded;
    private bool[] canPause;

	private Narration narrator;

    void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        image = GetComponentsInChildren<Image>()[1];
        image.enabled = false;

		narrator = GameObject.Find ("Narrator").GetComponent<Narration> ();
    }

    void Start()
    {
        touchStartPos = new Vector2[2];
        touchDirection = new Vector2[2];
        touchEnded = new bool[2];
        canPause = new bool[2];
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.touchCount == 2) {
        //     for (int i = 0; i < 2; i++) 
        //     {
        //         Touch touch = Input.GetTouch (i);
        //         switch (touch.phase) 
        //         {
        //         case TouchPhase.Began:
        //             touchStartPos [i] = touch.position;
        //             touchEnded [i] = false;
        //             canPause [i] = false;
        //             break;
        //         case TouchPhase.Moved:
        //             touchDirection[i] = touch.position - touchStartPos[i];
        //             break;
        //         case TouchPhase.Ended:
        //             touchEnded [i] = true;
        //             break;
        //         }
        //     }
        // }

        // if (touchEnded [0] == true && touchEnded [1] == true) 
        // {
        //     ChooseInput ();
        // }

        // if (canPause[0] == true && canPause[1] == true) 
        // {
        //     canPause [0] = canPause[1] = false;
        //     TooglePause ();
        // }

//        if (Input.touchCount == 2)
//        {
//            var firstTouchInfo = InputManager.ProcessTapInput(Input.GetTouch(0));
//            var secondTouchInfo = InputManager.ProcessTapInput(Input.GetTouch(1));
//            if (firstTouchInfo.tapCount == 2 &&
//                secondTouchInfo.tapCount == 2)
//            {
//                TooglePause();
//            }
//        }

        if (paused)
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        touchStartPos[0] = touch.position;
                        touchEnded[0] = false;
                        canPause[0] = false;
                        break;
                    case TouchPhase.Moved:
                        touchDirection[0] = touch.position - touchStartPos[0];
                        break;
                    case TouchPhase.Ended:
                        touchEnded[0] = true;
                        ChooseInput();
                        break;
                }
            }
        }
    }

    void ChooseInput()
    {
//        if (!paused)
//        {
//            for (int i = 0; i < 2; i++)
//            {
//                if (touchDirection[i].magnitude > 50)
//                {
//                    var normTouchDirection = touchDirection[i].normalized;
//                    float absoluteDirection = Mathf.Abs(Vector2.Dot(normTouchDirection, Vector2.right));
//                    if (absoluteDirection < 1)
//                    {
//                        if (normTouchDirection.y < 0)
//                        {
//                            canPause[i] = true;
//                        }
//                    }
//                }
//            }
//        }
//        else
//        {
            if (touchDirection[0].magnitude > 50)
            {
                var normTouchDirection = touchDirection[0].normalized;
                float absoluteDirection = Mathf.Abs(Vector2.Dot(normTouchDirection, Vector2.right));
                if (absoluteDirection < .2)
                {
                    if (normTouchDirection.y < 0)
                    {
                        TooglePause();
						narrator.Stop ();
                    }
                }
                else if (absoluteDirection > .8)
                {
                    if (normTouchDirection.x < 0)
                    {
                        TooglePause();
						narrator.Stop ();
                        SceneManager.LoadScene("Menu");
                    }
					else if (normTouchDirection.x > 0)
                    {
                        TooglePause();
						narrator.Stop ();
						SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    }
                }
           	}
        //}
    }

    public void TooglePause()
    {
        paused = !paused;
        Time.timeScale = (Time.timeScale == 0) ? 1 : 0;
        //player.GetComponent<AudioListener>().enabled = !player.GetComponent<AudioListener>().enabled;
		float newVolume;
		mixer.audioMixer.GetFloat("Volume", out newVolume);
		newVolume = (newVolume == -80.0f)? 0.0f: -80.0f;

		mixer.audioMixer.SetFloat ("Volume", newVolume);
        image.enabled = !image.enabled;
    }
}
