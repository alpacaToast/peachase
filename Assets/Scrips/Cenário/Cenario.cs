using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cenario : MonoBehaviour {

	public Transform PivotPoint;
	public Transform cenarioEnd;
	public Vector2 baseVelocity;

	public List<Rigidbody2D> firstSpeed;
	public List<Rigidbody2D> secondSpeed;
	public List<Rigidbody2D> thirdSpeed;
	public List<Rigidbody2D> fourthSpeed;

	// Use this for initialization
	void Start () {
		foreach (var rb in firstSpeed)
		{
			rb.velocity = baseVelocity / 4;
		}

		foreach (var rb in secondSpeed)
		{
			rb.velocity = baseVelocity / 3;
		}

		foreach (var rb in thirdSpeed)
		{
			rb.velocity = baseVelocity / 2;
		}

		foreach (var rb in fourthSpeed)
		{
			rb.velocity = baseVelocity;
		}
	}
	
	// Update is called once per frame
	void Update () {
		foreach (var rb in secondSpeed)
		{
			if (rb.transform.position.x < cenarioEnd.transform.position.x) 
			{
				var newPos = rb.transform.position;
				newPos.x = PivotPoint.transform.position.x;
				rb.transform.position = newPos;
			}
		}

		foreach (var rb in thirdSpeed)
		{
			if (rb.transform.position.x < cenarioEnd.transform.position.x) 
			{
				var newPos = rb.transform.position;
				newPos.x = PivotPoint.transform.position.x;
				rb.transform.position = newPos;
			}
		}

		foreach (var rb in fourthSpeed)
		{
			if (rb.transform.position.x < cenarioEnd.transform.position.x) 
			{
				var newPos = rb.transform.position;
				newPos.x = PivotPoint.transform.position.x;
				rb.transform.position = newPos;
			}
		}


	}
}
