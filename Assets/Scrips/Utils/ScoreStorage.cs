using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ScoreStorage : ScriptableObject {
	public int score;
	public int multiplier;

	public void Set(Score s) 
	{
		score = s.points;
		multiplier = s.multiplier;
	}
}
