using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Settings/Game Settings Data")]
public class GameSettingsSO : ScriptableObject
{
    [Header("Audio Settings")]
    [Range(0f, 100f)]
    public float masterVolume = 80f;

    [Range(0f, 100f)]
    public float musicVolume = 70f;

    public bool isMuted = false;

    [Header("Gameplay Settings")]
    [Range(1f, 20f)]
    public float playerSpeed = 5f;

    public string playerName = "Player1";

    public bool invertYAxis = false;
}