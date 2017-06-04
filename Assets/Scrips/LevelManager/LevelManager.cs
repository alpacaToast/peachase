using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public float timeSinceLastStep;
    public float heightInUnits { get { return 2 * Camera.main.orthographicSize; } }
    public float widthInUnits { get { return this.heightInUnits * Camera.main.aspect; } }
    public List<Obstacle> obstacles;
    public List<Tile> tiles;
    public bool ghostMode;

    [HideInInspector]
    public Touch currentTouch;

    void Start()
    {

    }

    void Update()
    {
        timeSinceLastStep += Time.deltaTime;

        if (timeSinceLastStep >= cycleTime/4)
        {
            timeSinceLastStep -= cycleTime/4;
            stepCount += 1; 
        }

        if (stepCount >= stepsToNextSpawn)
        {
            stepCount = 0;
            if (!ghostMode)
            {
                SpawnObstacle();
            }
            else 
            {
                SpawnTile();
            }

            stepsToNextSpawn = Random.Range(3, 8);
        }

        if (obstacleCount > 3)
        {
            ghostMode = true;
            obstacleCount = 0;
        }
    }

    void SpawnObstacle()
    {
        Obstacle obstacle = obstacles[Random.Range(0, obstacles.Count)];
        Vector3 obsPosition = transform.position;
        obsPosition.y += obstacle.height;

        var obs = Instantiate(obstacle, obsPosition, Quaternion.identity, transform);
        obs.rb2d.velocity = new Vector2(-this.widthInUnits/cycleTime, 0);
    }

    void SpawnTile()
    {
        Tile tile = tiles[Random.Range(0, tiles.Count)];
        Vector3 tilePosition = transform.position;
        tilePosition.y += 1;

        var obj = Instantiate(tile, tilePosition, Quaternion.identity, transform);
        obj.rb2d.velocity = new Vector2(-this.widthInUnits/cycleTime, 0);
    }
}