using UnityEngine;

[CreateAssetMenu(menuName = "TetrisGameplayConfig", order = 1)]
public class TetrisGameplayConfigSO : ScriptableObject
{
    [SerializeField] private float holdThreshold;
    [SerializeField] private float swipeDownThreshold;
    [SerializeField] private float horizontalSwipeThreshold;
    [SerializeField] private float verticalSwipeThreshold;
    [SerializeField] private float defaultSpeed;
    [SerializeField] private float maxSpeed;

    public float HoldThreshold
    {
        get => holdThreshold;
        private set => holdThreshold = value;
    }

    public float SwipeDownThreshold
    {
        get => swipeDownThreshold;
        private set => swipeDownThreshold = value;
    }

    public float HorizontalSwipeThreshold
    {
        get => horizontalSwipeThreshold;
        private set => horizontalSwipeThreshold = value;
    }

    public float VerticalSwipeThreshold
    {
        get => verticalSwipeThreshold;
        private set => verticalSwipeThreshold = value;
    }

    public float DefaultSpeed
    {
        get => defaultSpeed;
        private set => defaultSpeed = value;
    }

    public float MaxSpeed
    {
        get => maxSpeed;
        private set => maxSpeed = value;
    }
}