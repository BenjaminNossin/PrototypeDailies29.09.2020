using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New HighScore Data", menuName = "Data/Score/HighScore")]
public class HighScoreData : ScriptableObject
{
    public int CurrentHighScore { get; set; }
}
