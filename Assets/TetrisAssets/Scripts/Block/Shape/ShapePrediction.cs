using System.Collections.Generic;
using UnityEngine;

public class ShapePrediction
{
    private List<BlockController> _bottomBlockList;
    private List<BlockController> _downSortedBlockList;
    private bool[,] _visited;
    private BlockPredictingSystem BlockPredictingSystem => BlockPredictingSystem.Instance;
    private GameBoard GameBoard => GameBoard.Instance;
    private BlockController[,] BlockMatrix => GameBoard.BlockMatrix.Matrix;

    public List<MatrixIndex> PredictedPositions { get; private set; }

    public void SetDownSortedBlockList(List<BlockController> downSortedBlockList)
    {
        _downSortedBlockList = downSortedBlockList;
        PredictBlock();

        PredictedPositions = new List<MatrixIndex>();
    }

    public void SetBottomBlockList(List<BlockController> bottomBlockList)
    {
        _bottomBlockList = bottomBlockList;

        PredictBlock();
    }

    public void PredictBlock()
    {
        if (_bottomBlockList == null || _downSortedBlockList == null) return;

        BlockPredictingSystem.DisableAllPrediction();
        PredictedPositions.Clear();

        CheckStuckBlock(out var stuckRow, out var stuckBlock);

        if (stuckBlock == null) stuckBlock = _downSortedBlockList[0];
        Debug.Log($"(PREDICTION) Stuck at: [{stuckRow},{stuckBlock.Position.Column}]");

        _visited = new bool[GameBoard.Row, GameBoard.Column];

        EnablePrediction(stuckRow, stuckBlock);
    }

    private void EnablePrediction(int row, BlockController block)
    {
        var blockPos = block.Position;
        _visited[blockPos.Row, blockPos.Column] = true;
        Debug.Log($"(PREDICTION) Enable Prediction at: [{row},{blockPos.Column}]");

        BlockPredictingSystem.EnablePredictionAt(row, blockPos.Column);
        PredictedPositions.Add(new MatrixIndex(row, blockPos.Column));

        int[] dx = { 0, 0, 1, -1, 1, 1, -1, -1 };
        int[] dy = { 1, -1, 0, 0, 1, -1, 1, -1 };

        for (var k = 0; k < 8; k++)
        {
            var newRow = row + dx[k];
            if (newRow < 0 || newRow >= GameBoard.Row) continue;

            var nx = blockPos.Row + dx[k];
            var ny = blockPos.Column + dy[k];

            if (nx < 0 || nx >= GameBoard.Row || ny < 0 || ny >= GameBoard.Column) continue;

            var checkBlock = BlockMatrix[nx, ny];
            if (checkBlock == null || _visited[nx, ny]) continue;

            EnablePrediction(newRow, checkBlock);
        }
    }

    private void CheckStuckBlock(out int stuckRow, out BlockController stuckBlock)
    {
        var stuckBlocks = GetAllCollidedBlocks(out stuckRow, out stuckBlock);

        GetStuckBlock(ref stuckRow, ref stuckBlock, stuckBlocks);
    }

    private static void GetStuckBlock(ref int stuckRow, ref BlockController stuckBlock, Dictionary<BlockController, int> stuckBlocks)
    {
        foreach (var stuck in stuckBlocks)
        {
            var row = stuck.Value;
            var block = stuck.Key;
            var blockPos = block.Position;
            var stuckBlockPos = blockPos;

            if (stuckBlock != null) stuckBlockPos = stuckBlock.Position;

            if (row < stuckRow)
            {
                stuckRow = row;
                stuckBlock = block;
                Debug.Log($"(PREDICTION) New Stuck block: {stuckRow} --- [{stuckBlock.Position.Row},{stuckBlock.Position.Column}]");
            }
        }
    }

    private Dictionary<BlockController, int> GetAllCollidedBlocks(out int stuckRow, out BlockController stuckBlock)
    {
        var collidedRow = GameBoard.Row - 1;
        stuckRow = GameBoard.Row - 1;
        stuckBlock = null;

        var stuckBlocks = new Dictionary<BlockController, int>();

        foreach (var block in _bottomBlockList)
        {
            var blockPos = block.Position;

            for (var j = blockPos.Row + 1; j < GameBoard.Row; j++)
            {
                var value = BlockMatrix[j, blockPos.Column];
                if (value == null) continue;

                collidedRow = j - 1;
                break;
            }

            if (stuckBlocks.ContainsKey(block)) continue;

            stuckBlocks.Add(block, collidedRow);
            Debug.Log($"(PREDICTION) Stuck Block Add: {stuckBlocks.Count}/{_bottomBlockList.Count} --- {collidedRow} --- [{block.Position.Row},{block.Position.Column}]");
        }

        return stuckBlocks;
    }

    private void DisableStuckBlock(BlockController blockController)
    {
        foreach (var block in _downSortedBlockList) block.ActiveRenderer(true);

        blockController.ActiveRenderer(false);
    }
}