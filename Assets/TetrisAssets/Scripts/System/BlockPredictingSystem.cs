using UnityEngine;

public class BlockPredictingSystem : TemporaryMonoSingleton<BlockPredictingSystem>
{
    [SerializeField] private PositionGridView boardGridView;
    [SerializeField] private SpriteRenderer[,] rendererBoard;
    private GameBoard GameBoard => GameBoard.Instance;

    private void Start()
    {
        InitSystem();
    }

    private void InitSystem()
    {
        rendererBoard = new SpriteRenderer[GameBoard.Row, GameBoard.Column];
        var positionGridList = boardGridView.positionGrid;

        for (int i = 0; i < positionGridList.Count; i++)
        {
            var positionGridRow = positionGridList[i];
            var positionGridColumnList = positionGridRow.Positions;

            for (int j = 0; j < positionGridColumnList.Length; j++)
            {
                var positionGridColumn = positionGridColumnList[j];
                var spriteRenderer = positionGridColumn.GetComponent<SpriteRenderer>();

                spriteRenderer.enabled = false;
                rendererBoard[i, j] = spriteRenderer;
            }
        }
    }

    public void DisableAllPrediction()
    {
        foreach (var sprite in rendererBoard)
        {
            sprite.enabled = false;
        }
    }

    public void EnablePredictionAt(int row, int column)
    {
        rendererBoard[row, column].enabled = true;
    }
}