using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Score{
    public int points;
    public int multiplier = 1;

    public void AddPoint(int n)
    {
        points += multiplier * n;
        multiplier = Mathf.Min(10, multiplier+1);
        UpdateUI();
    }

    public void ResetMultiplier()
    {
        multiplier = 1;
        
        UpdateUI();
    }

    public void UpdateUI()
    {
        var pointsText = GameObject.FindGameObjectWithTag("Points").GetComponent<Text>();
        pointsText.text = points.ToString();

        var multiplierText = GameObject.FindGameObjectWithTag("Multiplier").GetComponent<Text>();
        multiplierText.text = "x" +  multiplier.ToString();
    }
    
}