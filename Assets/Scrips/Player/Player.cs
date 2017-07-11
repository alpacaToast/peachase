using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
class Player : MonoBehaviour
{
    public int hp;
    public float jumpForce;
    public Score score;
    public PlayerState playerState;
    public AudioSource feedback;
    public Rigidbody2D rb2d;
    public BoxCollider2D collider;
    public Animator animator;
    public ScoreStorage scoreStorage;
    public float minSlideDistance;

    public AudioClip burn;
    public AudioClip bite;
    public AudioClip jump;
    public AudioClip crawl;

    private LevelManager lvlManager;
    private Vector2 touchStartPos;
    private Vector2 touchDirection;

	private Narration narrator;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        score = new Score();
        lvlManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        animator = GetComponentInChildren<Animator>();
        feedback = GetComponent<AudioSource>();
        collider = GetComponent<BoxCollider2D>();
		narrator = GameObject.Find ("Narrator").GetComponent<Narration>();
    }

    void Start()
    {
		StartCoroutine ("Wait");
        GameObject.FindGameObjectWithTag("LifeCounter").GetComponent<LifeCounter>().ResetCounter(hp);
		if (GameObject.Find ("Pause").GetComponent<Pause> ().paused) 
		{
			GameObject.Find ("Pause").GetComponent<Pause> ().TooglePause ();
		}
        score.UpdateUI();
    }

    void Update()
    {
		if ((Input.touchCount == 2 && !GameObject.Find ("Pause").GetComponent<Pause>().paused)) 
		{
			narrator.StartNarration ("Pause");
			GameObject.Find ("Pause").GetComponent<Pause>().TooglePause();
		}

        else if (Input.touchCount == 1 &&
            Time.timeScale   == 1 &&
            !lvlManager.ghostMode &&
			!(lvlManager.tutorialMode && lvlManager.tutorialGhost))
        {

            var touchInfo = InputManager.ProcessTapInput(Input.GetTouch(0));

            if (touchInfo.tapCount == 1)
            {
                var touchLoc = touchInfo.touchLocation;

                if (touchLoc.y > .5 && this.playerState.grounded)
                {
                    this.playerState.grounded = false;
                    feedback.clip = jump;
                    feedback.Play();
                    Jump();
                }
                else if (touchLoc.y < .5 && !this.playerState.crawling)
                {
                    feedback.clip = crawl;
                    feedback.Play();
                    StartCoroutine("Crawl");
                }
            }
				

            // Touch touch = Input.GetTouch(0);
            // switch (touch.phase) 
            // {
            // case TouchPhase.Began:
            // 	touchStartPos = touch.position;
            // 	break;
            // case TouchPhase.Moved:
            // 	touchDirection = touch.position - touchStartPos;
            // 	break;
            // case TouchPhase.Ended:
            // 	ChooseInput ();
            // 	break;
            // }
        }

        if (score.multiplier == 10 && hp < 6)
        {
            hp++;
            GameObject.FindGameObjectWithTag("LifeCounter").GetComponent<LifeCounter>().ResetCounter(hp);
            score.ResetMultiplier();
        }

    }

    void ChooseInput()
    {
        if (touchDirection.magnitude > minSlideDistance)
        {
            var normTouchDirection = touchDirection.normalized;
            float absoluteDirection = Mathf.Abs(Vector2.Dot(normTouchDirection, Vector2.right));
            if (absoluteDirection < .3 &&
                 this.playerState.grounded)
            {
                if (normTouchDirection.y > .2)
                {
                    this.playerState.grounded = false;
                    feedback.clip = jump;
                    feedback.Play();
                    Jump();
                }
                else if (!this.playerState.crawling)
                {
                    feedback.clip = crawl;
                    feedback.Play();
                    StartCoroutine("Crawl");
                }
            }
        }
    }

    private void Jump()
    {
        rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        animator.SetTrigger("Jump");
    }

    private IEnumerator Crawl()
    {
        animator.SetBool("Crawling", true);
        this.playerState.crawling = true;
        var newSize = collider.size; newSize.y = 0.4f;
        var newOffset = collider.size; newOffset.y = -1.04f;
        collider.offset = newOffset;
        collider.size = newSize;
        yield return new WaitForSeconds(.75f);
        animator.SetBool("Crawling", false);
        this.playerState.crawling = false;
        newSize = collider.size; newSize.y = 1.08f;
        newOffset = collider.size; newOffset.y = -0.7f;
        collider.offset = newOffset;
        collider.size = newSize;
    }

    public void TakeDamage(Obstacle obstacle)
    {
		if (!lvlManager.tutorialMode) 
		{
			hp--;
			score.ResetMultiplier();
		}

        GameObject.FindGameObjectWithTag("LifeCounter").GetComponent<LifeCounter>().ResetCounter(hp);

        if (obstacle.name.Contains("Gaivota"))
        {
            feedback.clip = bite;
            feedback.Play();
            animator.SetTrigger("Bite");
        }
        else if (obstacle.name.Contains("Vulcano"))
        {
            feedback.clip = burn;
            feedback.Play();
            animator.SetTrigger("Burn");
        }

        if (hp == 0)
        {
            Death();
        }
    }

    void Death()
    {
        scoreStorage.Set(score);
        SceneManager.LoadScene("GameOver");
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        string colidedTag = col.collider.tag;

        switch (colidedTag)
        {
            case "Untagged":
                Debug.LogWarning("Collided with untagged object");
                break;
            case "Ground":
                playerState.grounded = true;
                break;
            default:
                Debug.Log("Collided with" + col.collider.name);
                break;
        }
    }

	IEnumerator Wait()
	{
		yield return new WaitForSeconds (0.1f);
	}

    public struct PlayerState
    {
        public bool grounded;
        public bool crawling;
    }
}