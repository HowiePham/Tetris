using System.Collections.Generic;
using UnityEngine;

public class GameState : TemporaryMonoSingleton<GameState>
{
    [SerializeField] private BoardConfig boardConfig;
    [SerializeField] private BlockColorTemplate blockColorTemplate;
    [SerializeField] private List<PositionGridView> PositionGridViews = new List<PositionGridView>();

    [SerializeField] private CameraSystem cameraSystem;

    public float GameplayBoardVerticalSpace => boardConfig.GameplayBoardVerticalSpace;
    public float GameplayBoardHorizontalSpace => boardConfig.GameplayBoardHorizontalSpace;
    public int GameplayBoardRow => boardConfig.GameplayBoardRow;
    public int GameplayBoardColumn => boardConfig.GameplayBoardColumn;
    public int QueueBoardRow => boardConfig.QueueBoardRow;
    public int QueueBoardColumn => boardConfig.QueueBoardColumn;
    public float QueueBoardVerticalSpace => boardConfig.QueueBoardVerticalSpace;
    public float QueueBoardHorizontalSpace => boardConfig.QueueBoardHorizontalSpace;
    public float CameraSize => boardConfig.CameraSize;
    public List<Sprite> BlockColorTemplate => blockColorTemplate.BlockColorTemplateList;

    public int GetBoardRow(GridViewType gridViewType)
    {
        switch (gridViewType)
        {
            case GridViewType.GAMEPLAY_BOARD:
                return GameplayBoardRow;
            case GridViewType.QUEUE_BOARD:
                return QueueBoardRow;
            default:
                return 0;
        }
    }

    public int GetBoardColumn(GridViewType gridViewType)
    {
        switch (gridViewType)
        {
            case GridViewType.GAMEPLAY_BOARD:
                return GameplayBoardColumn;
            case GridViewType.QUEUE_BOARD:
                return QueueBoardColumn;
            default:
                return 0;
        }
    }

    public float GetBoardVerticalSpace(GridViewType gridViewType)
    {
        switch (gridViewType)
        {
            case GridViewType.GAMEPLAY_BOARD:
                return GameplayBoardVerticalSpace;
            case GridViewType.QUEUE_BOARD:
                return QueueBoardVerticalSpace;
            default:
                return 0;
        }
    }

    public float GetBoardHorizontalSpace(GridViewType gridViewType)
    {
        switch (gridViewType)
        {
            case GridViewType.GAMEPLAY_BOARD:
                return GameplayBoardHorizontalSpace;
            case GridViewType.QUEUE_BOARD:
                return QueueBoardHorizontalSpace;
            default:
                return 0;
        }
    }

    [ContextMenu("APPLY CONFIG")]
    private void ApplyConfig()
    {
        cameraSystem.AdjustCameraSize();

        foreach (var positionGrid in PositionGridViews)
        {
            positionGrid.ClearAll();
            positionGrid.CreateGridRow();
        }
    }
}