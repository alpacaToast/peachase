using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum TutorialStage { GAIVOTA, VULCAO, FANTASMA, FIM }

class LevelManager : MonoBehaviour
{
    // Tempo de um ciclo (quatro steps).
    // Os acontecimentos do jogo (obstaculos e fantasmas),
    // vão ocorrer com base nestes steps
    public float cycleTime;
    public int cycleCount;
    public int stepCount;
    public int obstacleCount;
    public int stepsToNextSpawn;
	public int obstaclesToNextGhost;
	public int currentGhost;
    public float timeSinceLastStep;
    public float heightInUnits { get { return 2 * Camera.main.orthographicSize; } }
    public float widthInUnits { get { return this.heightInUnits * Camera.main.aspect; } }
    public List<Obstacle> obstacles;
    public List<Ghost> ghosts;
    public bool ghostMode;
	public bool tutorialMode;
	public bool tutorialGhost;
	public TutorialStage currentStage; 

	private Vector2 touchStartPos;
	private Vector2 touchDirection;

	public AudioSource narration;
	public List<AudioClip> narrationClips;

    [HideInInspector]
    public Touch currentTouch;
	private Player player;

    void Start()
    {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player>();
		stepsToNextSpawn = Random.Range(3, 8);
		tutorialGhost = false;
    }

    void Update()
    {
        timeSinceLastStep += Time.deltaTime;

        if (timeSinceLastStep >= cycleTime/4)
        {
            timeSinceLastStep -= cycleTime/4;
            stepCount += 1; 
        }

        if (stepCount >= stepsToNextSpawn &&
			obstacleCount < obstaclesToNextGhost &&
			!tutorialMode)
        {
            stepCount = 0;
            if (!ghostMode && !tutorialMode)
            {
                SpawnObstacle();
				obstacleCount++;
            }
            stepsToNextSpawn = Random.Range(3, 8);
        }

		if (!ghostMode && !tutorialMode)
        {
			if ((obstacleCount >= obstaclesToNextGhost) && 
				GameObject.FindGameObjectsWithTag("Obstacle").Length == 0)
			{
				currentGhost = currentGhost % ghosts.Count;
				ghostMode = true;
				Instantiate(ghosts[currentGhost]);
				obstacleCount = 0;
				currentGhost++;
			}
        }

		if (tutorialMode) 
		{
			switch (currentStage) 
			{
			case (TutorialStage.GAIVOTA):
				GaivotaStage ();
				break;
			case (TutorialStage.VULCAO):
				StartCoroutine("VulcaoStage");
				break;
			case (TutorialStage.FANTASMA):
				StartCoroutine ("FantasmaStage");
				break;
			case TutorialStage.FIM:
				Fim ();
				break;
			}
			Debug.Log ("Stage " + (int)currentStage);
		}
	}

	void GaivotaStage() 
	{

		narration.clip = narrationClips[(int)currentStage];
		if (narration.time == 0.0) 
		{
			narration.Play ();
		}

		if ((narration.time >= narration.clip.length - 0.1) || !narration.isPlaying) 
		{
			narration.Pause ();
			if (stepCount >= stepsToNextSpawn)
			{
				stepCount = 0;
				SpawnObstacle(obstacles[0]);
				stepsToNextSpawn = Random.Range(5, 8);
			}
		}
		if (Input.touchCount == 1) 
		{
			Touch touch = Input.GetTouch(0);
			switch (touch.phase)
			{
			case TouchPhase.Began:
				touchStartPos = touch.position;
				break;
			case TouchPhase.Moved:
				touchDirection = touch.position - touchStartPos;
				break;
			case TouchPhase.Ended:
				ChooseInput(TutorialStage.VULCAO);
				break;
			}
		}		
	}

	IEnumerator VulcaoStage() 
	{

		narration.clip = narrationClips[(int)currentStage];
		if (narration.time == 0.0) 
		{
			narration.Play ();
		}

		if ((narration.time >= narration.clip.length - 0.1) || !narration.isPlaying) 
		{
			narration.Pause ();
			if (stepCount >= stepsToNextSpawn)
			{
				stepCount = 0;
				SpawnObstacle(obstacles[1]);
				stepsToNextSpawn = Random.Range(5, 8);
			}
		}
		yield return new WaitForSeconds (0.1f);
		if (Input.touchCount == 1) 
		{
			Touch touch = Input.GetTouch(0);
			switch (touch.phase)
			{
			case TouchPhase.Began:
				touchStartPos = touch.position;
				break;
			case TouchPhase.Moved:
				touchDirection = touch.position - touchStartPos;
				break;
			case TouchPhase.Ended:
				ChooseInput(TutorialStage.FANTASMA);
				break;
			}
		}		
	}

	IEnumerator FantasmaStage() 
	{
		narration.clip = narrationClips[(int)currentStage];
		if (narration.time == 0.0) 
		{
			narration.Play ();
		}

		if (((narration.time >= narration.clip.length - 0.1) || !narration.isPlaying)
			 && !tutorialGhost) 
		{
			narration.Pause ();
			tutorialGhost = true;
			Instantiate(ghosts[0]);
		}
		yield return new WaitForSeconds (0.1f);
		if (Input.touchCount == 1) 
		{
			Touch touch = Input.GetTouch(0);
			switch (touch.phase)
			{
			case TouchPhase.Began:
				touchStartPos = touch.position;
				break;
			case TouchPhase.Moved:
				touchDirection = touch.position - touchStartPos;
				break;
			case TouchPhase.Ended:
				ChooseInput(TutorialStage.FIM);
				break;
			}
		}		
	}

	void Fim() {
		SceneManager.LoadScene ("Menu");
	}

	void ChooseInput(TutorialStage nextStage)
	{
		if (touchDirection.magnitude > 100)
		{
			var normTouchDirection = touchDirection.normalized;
			float absoluteDirection = Mathf.Abs(Vector2.Dot(normTouchDirection, Vector2.right));
			if (absoluteDirection > .7)
			{
				if (normTouchDirection.x > .7)
				{
					touchDirection = new Vector2 ();
					currentStage = nextStage;
					narration.Stop ();
					narration.time = 0.0f;
				}
			}
		}
	}


    void SpawnObstacle()
    {
        Obstacle obstacle = obstacles[Random.Range(0, obstacles.Count)];
        Vector3 obsPosition = transform.position;
        obsPosition.y = obstacle.height;

        var obs = Instantiate(obstacle, obsPosition, Quaternion.identity, transform);
        obs.rb2d.velocity = new Vector2(-this.widthInUnits/cycleTime, 0);
    }

	void SpawnObstacle(Obstacle obstacle)
	{
		Vector3 obsPosition = transform.position;
		obsPosition.y = obstacle.height;

		var obs = Instantiate(obstacle, obsPosition, Quaternion.identity, transform);
		Debug.Log (obs);
		obs.rb2d.velocity = new Vector2(-this.widthInUnits/cycleTime, 0);
	}


    public void OnGhostDeath()
    {
		Debug.Log ("Ghost died");
		tutorialGhost = false;
		ghostMode = false;
        if(currentGhost == ghosts.Count)
        {
            SceneManager.LoadScene("FinalCutscene");
        }
    }
}