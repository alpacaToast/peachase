using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
class Player : MonoBehaviour
{

    public float jumpForce;
    public Score score;
    public PlayerState playerState;
    public Rigidbody2D rb2d;

    private LevelManager lvlManager;
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        score = new Score();
        lvlManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    void Start()
    {

    }

    void Update()
    {

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;

            if (Mathf.Abs(touchDeltaPosition.x) < 11)
            {
                if (touchDeltaPosition.y > 0.5 && this.playerState.grounded)
                {
                    this.playerState.grounded = false;
                    Jump();
                }
                else if (touchDeltaPosition.y < 0 && !this.playerState.crawling)
                {
                    StartCoroutine("Crawl");
                }
            }
        }

        /*
        if (Input.GetButtonUp("Jump") && this.playerState.grounded)
        {
            Jump();
            this.playerState.grounded = false;
        }
        if (Input.GetAxis("Horizontal") < 0 && !this.playerState.crawling)
        {
            StartCoroutine("Crawl");
        }
         */
    }

    private void Jump()
    {
        //TODO: Se necessário, usar um método com mais controle 
        //      sobre o pulo.

        // ForceMode2D.Impulse adiciona a força instantanemente
        // ForceMode2D.Force adiciona a força aos poucos
        rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private IEnumerator Crawl()
    {
        this.playerState.crawling = true;
        transform.localScale = new Vector3(2, 0.5f, 2);
        yield return new WaitForSeconds(.75f);
        transform.localScale = new Vector3(2, 2f, 2);
        this.playerState.crawling = false;
    }

    public void TakeDamage(Obstacle obstacle)
    {
        Debug.Log("Player took Damage from " + obstacle.name);
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

    public struct PlayerState
    {
        public bool grounded;
        public bool crawling;
    }
}