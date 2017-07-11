using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MenuNarration 
{
	public List<AudioClip> clips;
	public int currentClip;
}

public class Narration : MonoBehaviour {
	
	AudioSource audio;
	public bool stop;
	public bool disabled;

	public MenuNarration menu;
	public MenuNarration opcoes;
	public MenuNarration pause;
	public MenuNarration gameOver;
	public MenuNarration tutorialGaivota;
	public MenuNarration tutorialVulcao;
	public MenuNarration tutorialFantasma;

	void Awake () 
	{
		DontDestroyOnLoad (gameObject);
		audio = GetComponent<AudioSource> ();
	}

	public void StartNarration(string menuName)
	{
		stop = false;

		if (!disabled)
			StartCoroutine (menuName + "Corroutine");
	}

	public IEnumerator MenuCorroutine()
	{
		Debug.Log ("Initiated");
		var currentAudio = 0;
		while(currentAudio < menu.clips.Count && !stop)
		{
			audio.clip = menu.clips[currentAudio];
			audio.Play();
			yield return new WaitForSeconds(audio.clip.length + 0.3f);
			currentAudio ++;
		}
	}

	public IEnumerator OpcoesCorroutine()
	{
		Debug.Log ("Initiated");
		var currentAudio = 0;
		while(currentAudio < opcoes.clips.Count && !stop)
		{
			audio.clip = opcoes.clips[currentAudio];
			audio.Play();
			yield return new WaitForSeconds(audio.clip.length + 0.3f);
			currentAudio ++;
		}
	}

	public IEnumerator GameOverCorroutine()
	{
		Debug.Log ("Initiated");
		var currentAudio = 0;
		while(currentAudio < gameOver.clips.Count && !stop)
		{
			audio.clip = gameOver.clips[currentAudio];
			audio.Play();
			yield return new WaitForSeconds(audio.clip.length + 0.3f);
			currentAudio ++;
		}
	}

	public IEnumerator PauseCorroutine()
	{
		Debug.Log ("Initiated");
		var currentAudio = 0;
		while(currentAudio < pause.clips.Count && !stop)
		{
			audio.clip = pause.clips[currentAudio];
			audio.Play();
			yield return new WaitForSeconds(audio.clip.length + 0.3f);
			currentAudio ++;
		}
	}

	public IEnumerator GaivotaTutorialCorroutine()
	{
		Debug.Log ("Initiated");
		var currentAudio = 0;
		while(currentAudio < tutorialGaivota.clips.Count && !stop)
		{
			audio.clip = tutorialGaivota.clips[currentAudio];
			audio.Play();
			yield return new WaitForSeconds(audio.clip.length + 0.3f);
			currentAudio ++;
		}
	}

	public void Stop()
	{
		stop = true;
		audio.Stop ();
	}


}
