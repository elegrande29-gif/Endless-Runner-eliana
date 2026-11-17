using UnityEngine;

[CreateAssetMenu(fileName = "RunnerMenuData", menuName = "Runner/Menu Data")]
public class RunnerMenuSO : ScriptableObject
{
    [Header("Game Mode Settings")]
    public bool isInfiniteMode = true;
    public float oneMinuteDuration = 60f;

    [Header("Player Stats")]
    public float currentTimer = 60f;
    public int highScore = 0;

    [Header("Runner Gameplay Settings")]
    public float playerSpeed = 10f;
    public float masterVolume = 80f;
}