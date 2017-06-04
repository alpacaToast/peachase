using UnityEngine;

class Obstacle : MonoBehaviour {

    public float height;
    public Rigidbody2D rb2d;
    public bool canAddScore;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.gravityScale = 0;
        canAddScore = true;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            //Debug.Log(this.name + " colided with Player");
            this.canAddScore = false;
            Player player = col.GetComponent<Player>();
            player.TakeDamage(this);
        }

        if (col.tag == "ScoreCollider")
        {
            if (this.canAddScore)
            {
                Debug.Log(this.name + " colided with ScoreCollider");
                GameObject.Find("Player").GetComponent<Player>().score.AddPoint(1);
            }
        }

        if (col.tag == "Barrier")
        {
            GetComponentInParent<LevelManager>().obstacleCount++;
            Destroy(this.gameObject);
        }
    }

}