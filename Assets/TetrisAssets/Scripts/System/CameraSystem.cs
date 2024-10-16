using UnityEngine;

public class CameraSystem : TemporaryMonoSingleton<CameraSystem>
{
    [SerializeField] private Camera _camera;

    public GameState gameState;

    private float defaultCameraSize;
    private const float DEFAULT_SCREEN_RATIO = 9 / 16f;

    private GameState GameState
    {
        get
        {
            if (GameState.Instance == null) return gameState;
            return GameState.Instance;
        }
    }

    protected override void Init()
    {
        LoadComponents();
    }

    private void Start()
    {
        AdjustCameraSize();
    }

    private void Reset()
    {
        LoadComponents();
    }

    private void LoadComponents()
    {
        _camera = GetComponent<Camera>();
    }

    [ContextMenu("ADJUST CAMERA SIZE")]
    public void AdjustCameraSize()
    {
        defaultCameraSize = GameState.CameraSize;

        var currentRatio = _camera.aspect;
        _camera.orthographicSize = (defaultCameraSize * DEFAULT_SCREEN_RATIO) / currentRatio;
    }
}