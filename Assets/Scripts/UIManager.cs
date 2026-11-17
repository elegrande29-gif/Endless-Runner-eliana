using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [Header("Data Reference")]
    public RunnerMenuSO menuData;

    private VisualElement root;
    private Button startButton;
    private Button settingsButton;
    private Button infiniteButton;
    private Button oneMinuteButton;
    private Label timerLabel;
    private Label scoreLabel;

    private void OnEnable() 
    
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null)
        {
            Debug.LogError("No UIDocument found on this GameObject!");
            return;
        }

        root = uiDocument.rootVisualElement;
        startButton = root.Q<Button>("btn-start");
        settingsButton = root.Q<Button>("btn-settings");
        infiniteButton = root.Q<Button>("btn-mode-infinite");
        oneMinuteButton = root.Q<Button>("btn-mode-oneminute");
        timerLabel = root.Q<Label>("label-timer");
        scoreLabel = root.Q<Label>("label-score");

        if (startButton != null) startButton.clicked += OnStartClicked;
        if (settingsButton != null) settingsButton.clicked += OnSettingsClicked;
        if (infiniteButton != null) infiniteButton.clicked += OnInfiniteModeClicked;
        if (oneMinuteButton != null) oneMinuteButton.clicked += OnOneMinuteModeClicked;

        UpdateUI();
    }

    private void OnDisable()
    {
        if (startButton != null) startButton.clicked -= OnStartClicked;
        if (settingsButton != null) settingsButton.clicked -= OnSettingsClicked;
        if (infiniteButton != null) infiniteButton.clicked -= OnInfiniteModeClicked;
        if (oneMinuteButton != null) oneMinuteButton.clicked -= OnOneMinuteModeClicked;
    }

    private void OnStartClicked()
    {
        root.style.display = DisplayStyle.None;
    }

    private void OnSettingsClicked()
    {
        Debug.Log("Settings clicked!");
    }

    private void OnInfiniteModeClicked()
    {
        if (menuData != null)
        {
            menuData.isInfiniteMode = true;
            UpdateUI();
        }
    }

    private void OnOneMinuteModeClicked()
    {
        if (menuData != null)
        {
            menuData.isInfiniteMode = false;
            menuData.currentTimer = menuData.oneMinuteDuration;
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        if (menuData == null) return;

        if (scoreLabel != null)
            scoreLabel.text = menuData.highScore.ToString();

        if (timerLabel != null)
            timerLabel.text = menuData.isInfiniteMode ? "∞" : $"{menuData.oneMinuteDuration}s";
    }
}