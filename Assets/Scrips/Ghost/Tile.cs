using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
class Tile : MonoBehaviour
{
    public bool active;
    public TileType type;
    private Player player;
    public Rigidbody2D rb2d;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.gravityScale = 0;
    }

    void Update()
    {
        if (Mathf.Abs(player.transform.position.x - this.transform.position.x) <= 1)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;

                if ((touchDeltaPosition.x > 0.5 && type == TileType.RIGHT) ||
                    (touchDeltaPosition.x < -0.5 && type == TileType.LEFT))
                {
                    Debug.Log("Acertou");
                }
                else 
                {
                    Debug.Log("Errooou");
                }
            }
        }
    }

}

public enum TileType { LEFT, RIGHT };