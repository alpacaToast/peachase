using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct CutsceneImage
{
    public Sprite image;
    public AudioClip audio;
    public float delay;
}

public class Cutscene : MonoBehaviour
{
    public string nextScene;
    public Image image;
    public List<CutsceneImage> cutscenes;
    public AudioSource audio;
    public int currentImage;
    public bool changing = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

		if (Input.touchCount == 1) {
			var touchInfo = InputManager.ProcessTapInput (Input.GetTouch (0));

			if (touchInfo.tapCount == 1 && touchInfo.phase == TouchPhase.Ended) {
				var touchLoc = touchInfo.touchLocation;
				if (touchLoc.x > .5) {
					ForceNext ();
				} else if (touchLoc.x < .5) {
					Previous ();
				}
			}
		} else if (Input.touchCount == 2)
		{
			StartCoroutine ("Wait");
			SceneManager.LoadScene (nextScene);
		}
        Next();
    }
	IEnumerator Wait()
	{
		yield return new WaitForSeconds (0.1f);
	}

    public void Next()
    {
        if (!audio.isPlaying)
        {
            ForceNext();
        }
    }

    public void ForceNext()
    {
        if (currentImage == cutscenes.Count)
        {
            SceneManager.LoadScene(nextScene);
            return;
        }
        audio.clip = cutscenes[currentImage].audio;
        audio.Play(0);
        image.sprite = cutscenes[currentImage].image;
        currentImage++;
    }

	public void Previous()
	{
		currentImage--;
		if (currentImage == -1)
		{
			SceneManager.LoadScene ("Menu");
		}
		audio.clip = cutscenes[currentImage].audio;
		audio.Play(0);
		image.sprite = cutscenes[currentImage].image;
	}
}
