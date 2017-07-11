using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


class LifeCounter : MonoBehaviour
{
    public float offset;
    public GameObject lifeImage;

    public GameObject youngestLife;

    void Start()
    {
		ResetCounter(GameObject.FindGameObjectWithTag("Player").GetComponent<Player> ().hp);
    }

    public void ResetCounter(int count)
    {
        if (count >= 0)
        {
            var children = GetComponentsInChildren<Image>();
            foreach (var c in children)
            {
                Destroy(c.gameObject);
            }

            youngestLife = null;
            
            for (int i = 0; i < count; i++)
            {
                var lifeImg = Instantiate(lifeImage, this.transform);
                var rectTransform = lifeImg.GetComponent<RectTransform>();

                if (youngestLife)
                {
                    var newPos = youngestLife.GetComponent<RectTransform>().position;
                    newPos.x += offset;

                    rectTransform.position = newPos;
                }

                youngestLife = lifeImg;
            }
        }
    }
}