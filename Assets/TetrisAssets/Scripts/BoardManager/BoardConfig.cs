using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "BoardConfig", order = 1)]
public class BoardConfig : ScriptableObject
{
    [Header("GAMEPLAY BOARD CONFIG")] [SerializeField]
    private int gameplayBoardRow;

    [SerializeField] private int gameplayBoardColumn;
    [SerializeField] private float gameplayBoardVerticalSpace;
    [SerializeField] private float gameplayBoardHorizontalSpace;

    [Space(10)] [Header("QUEUE BOARD CONFIG")] [SerializeField]
    private int queueBoardRow;

    [SerializeField] private int queueBoardColumn;
    [SerializeField] private float queueBoardVerticalSpace;
    [SerializeField] private float queueBoardHorizontalSpace;

    [Space(10)] [Header("CAMERA")] [SerializeField]
    private float cameraSize;

    public float GameplayBoardVerticalSpace => gameplayBoardVerticalSpace;
    public float GameplayBoardHorizontalSpace => gameplayBoardHorizontalSpace;
    public int GameplayBoardRow => gameplayBoardRow;
    public int GameplayBoardColumn => gameplayBoardColumn;
    public int QueueBoardRow => queueBoardRow;
    public int QueueBoardColumn => queueBoardColumn;
    public float QueueBoardVerticalSpace => queueBoardVerticalSpace;
    public float QueueBoardHorizontalSpace => queueBoardHorizontalSpace;
    public float CameraSize => cameraSize;
}