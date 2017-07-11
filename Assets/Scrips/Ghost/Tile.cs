using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
class Tile : MonoBehaviour
{
    public bool active;
    public bool audioPlayed;
    public TileType type;
    public AudioSource audio;
    public AudioSource sucess;
    private Player player;
    private Ghost ghost;
    public Rigidbody2D rb2d;

    private Vector2 touchStartPos;
    private Vector2 touchDirection;

    void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        ghost = GameObject.FindGameObjectWithTag("Ghost").GetComponent<Ghost>();
        audio = GetComponents<AudioSource>()[0];
        sucess = GetComponents<AudioSource>()[1];
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.gravityScale = 0;

        active = true;
    }

    void Update()
    {
        if (this.transform.localPosition.x <= 2.0 &&
            this.transform.localPosition.x >= 0.0 &&
            !audioPlayed)
        {
            audio.Play();
            audioPlayed = true;
        }

        if (Input.touchCount == 1)
        {
            if (this.transform.localPosition.x <= 2 && active)
            {
                var touchInfo = InputManager.ProcessTapInput(Input.GetTouch(0));

                if(touchInfo.tapCount == 1)
                {
                    var touchLoc = touchInfo.touchLocation;

                    if( touchLoc.x > .5 && type == TileType.RIGHT ||
                        touchLoc.x < .5 && type == TileType.LEFT )
                    {
                        ghost.Move ();
                        player.score.AddPoint (1);
                    }
                    else {
                        sucess.Play();
                    }
                    active = false;
                }
                // Touch touch = Input.GetTouch (0);
                // switch (touch.phase) 
                // {
                // case TouchPhase.Began:
                // 	touchStartPos = touch.position;
                // 	break;
                // case TouchPhase.Moved:
                // 	touchDirection = touch.position - touchStartPos;
                // 	break;
                // case TouchPhase.Ended:
                // 	if (touchDirection.magnitude > 30) 
                // 	{
                // 		var normTouchDirection = touchDirection.normalized;
                // 		float absoluteDirection = Mathf.Abs (Vector2.Dot (normTouchDirection, Vector2.right));
                // 		if ( absoluteDirection > .7) {
                // 			if ((normTouchDirection.x > 0 && type == TileType.RIGHT) ||
                // 			    (normTouchDirection.x < 0 && type == TileType.LEFT)) {
                // 				ghost.Move ();
                // 				player.score.AddPoint (1);
                // 			} else {
                // 				sucess.Play ();
                // 			}
                // 		}
                // 	}
                // 	active = false;
                // 	break;
                // }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.tag == "TileBarrier")
        {
            Destroy(this.gameObject);
        }
    }

}

public enum TileType { LEFT, RIGHT };
