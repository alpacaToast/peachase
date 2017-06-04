using System.Collections;

[System.Serializable]
class Score {
    int points;
    int multiplier;

    public void AddPoint(int n)
    {
        points += multiplier * n;
        multiplier += 1;
    }

    private void ResetMultiplier()
    {
        multiplier = 1;
    }
    
}