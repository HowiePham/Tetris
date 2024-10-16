using UnityEngine;

public class TetrisGameplayConfig : TemporaryMonoSingleton<TetrisGameplayConfig>
{
    [SerializeField] private TetrisGameplayConfigSO tetrisGameplayConfigSO;
    [SerializeField] private float holdThreshold;
    [SerializeField] private float swipeDownThreshold;
    [SerializeField] private float horizontalSwipeThreshold;
    [SerializeField] private float verticalSwipeThreshold;
    [SerializeField] private float defaultSpeed;
    [SerializeField] private float maxSpeed;

    public float HoldThreshold
    {
        get => holdThreshold;
        set => holdThreshold = value;
    }

    public float SwipeDownThreshold
    {
        get => swipeDownThreshold;
        set => swipeDownThreshold = value;
    }

    public float HorizontalSwipeThreshold
    {
        get => horizontalSwipeThreshold;
        set => horizontalSwipeThreshold = value;
    }

    public float VerticalSwipeThreshold
    {
        get => verticalSwipeThreshold;
        set => verticalSwipeThreshold = value;
    }

    public float DefaultSpeed
    {
        get => defaultSpeed;
        set => defaultSpeed = value;
    }

    public float MaxSpeed
    {
        get => maxSpeed;
        set => maxSpeed = value;
    }

    private void Start()
    {
        InitDefaultConfig();
    }

    private void InitDefaultConfig()
    {
        Application.targetFrameRate = 60;

        HoldThreshold = tetrisGameplayConfigSO.HoldThreshold;
        SwipeDownThreshold = tetrisGameplayConfigSO.SwipeDownThreshold;
        HorizontalSwipeThreshold = tetrisGameplayConfigSO.HorizontalSwipeThreshold;
        VerticalSwipeThreshold = tetrisGameplayConfigSO.VerticalSwipeThreshold;
        DefaultSpeed = tetrisGameplayConfigSO.DefaultSpeed;
        MaxSpeed = tetrisGameplayConfigSO.MaxSpeed;
    }
}