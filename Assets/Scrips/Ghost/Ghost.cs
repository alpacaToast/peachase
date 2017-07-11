using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Side {Left, Right, Pause};

class Ghost : MonoBehaviour 
{
	public float stepTime;
	public GameObject tileRight;
	public GameObject tileLeft;
	public List<Side> sequence;
	public List<AudioClip> tileAudio;
	public int currentAudio;

	private int currentTile;
	private float timeSinceLastTile;
	private LevelManager lvlManager;

	void Start() 
	{
		lvlManager = GameObject.FindGameObjectWithTag ("LevelManager").GetComponent<LevelManager> ();
		currentAudio = 0;
		currentTile = 0;
	}

	void Update()
	{
		timeSinceLastTile += Time.deltaTime;

		if (timeSinceLastTile >= stepTime) 
		{
			timeSinceLastTile -= stepTime;
			currentTile = currentTile % sequence.Count;
			SpawnTile ();
			currentTile++;
		}
	}

	void SpawnTile()
	{
		GameObject Tile;
		if (sequence [currentTile] == Side.Left) {
			Tile = tileLeft;
		} else if (sequence [currentTile] == Side.Right) {
			Tile = tileRight;
		} else {
			return;
		}
			
		Vector3 tilePosition = new Vector3 (14, -4);

		var obj = Instantiate(Tile, tilePosition, Quaternion.identity, null).GetComponent<Tile>();
		obj.rb2d.velocity = new Vector2(-lvlManager.widthInUnits/stepTime, 0);

		//Debug.Log (currentAudio);
		obj.audio.clip = tileAudio [currentAudio];
		currentAudio = ++currentAudio % tileAudio.Count;

	}

	public void Move()
	{
		transform.position = new Vector3 (transform.position.x - 1.5f, transform.position.y, 0);
		if (transform.position.x <= -16) {
			lvlManager.OnGhostDeath ();
			Destroy (this.gameObject);
		}
	}
	
}
