using System.Collections.Generic;

public class ShapePhysic
{
    private readonly ShapePrediction _shapePrediction;
    private List<BlockController> _bottomBlockList;
    private List<BlockController> _downSortedBlockList;
    private bool _isMoving;
    private List<BlockController> _leftSideBlockList;

    private List<BlockController> _leftSortedBlockList;
    private List<BlockController> _rightSideBlockList;
    private List<BlockController> _rightSortedBlockList;

    public ShapePhysic()
    {
        _shapePrediction = new ShapePrediction();
    }

    private BlockPredictingSystem BlockPredictingSystem => BlockPredictingSystem.Instance;

    public MatrixIndex ShapePosition { get; set; }

    public void SetShapePosition(MatrixIndex pos)
    {
        ShapePosition = pos;
    }

    public void SetAllSideBlockList(List<BlockController> blockList)
    {
        _leftSortedBlockList = new List<BlockController>(blockList);
        _rightSortedBlockList = new List<BlockController>(blockList);
        _downSortedBlockList = new List<BlockController>(blockList);

        _downSortedBlockList.Sort((a, b) => b.Position.Row.CompareTo(a.Position.Row));
        _leftSortedBlockList.Sort((a, b) => a.Position.Column.CompareTo(b.Position.Column));
        _rightSortedBlockList.Sort((a, b) => b.Position.Column.CompareTo(a.Position.Column));

        _shapePrediction.SetDownSortedBlockList(_downSortedBlockList);
    }

    public void SetShapeSideBlocks(List<BlockController> bottomBlocks, List<BlockController> leftSideBlocks,
        List<BlockController> rightSideBlocks)
    {
        _bottomBlockList = bottomBlocks;
        _leftSideBlockList = leftSideBlocks;
        _rightSideBlockList = rightSideBlocks;

        _bottomBlockList.Sort((a, b) => b.Position.Row.CompareTo(a.Position.Row));
        _shapePrediction.SetBottomBlockList(_bottomBlockList);
    }

    public void MoveDown()
    {
        if (!CanMoveDown())
        {
            _isMoving = false;
            return;
        }

        _isMoving = true;

        foreach (var block in _downSortedBlockList) block.CheckMoveDown();

        SetShapePosition(new MatrixIndex(ShapePosition.Row + 1, ShapePosition.Column));
    }

    public void MoveLeft()
    {
        if (!CanMoveLeft()) return;

        foreach (var block in _leftSortedBlockList) block.CheckMoveLeft();

        SetShapePosition(new MatrixIndex(ShapePosition.Row, ShapePosition.Column - 1));
        _shapePrediction.PredictBlock();
    }

    public void MoveRight()
    {
        if (!CanMoveRight()) return;

        foreach (var block in _rightSortedBlockList) block.CheckMoveRight();

        SetShapePosition(new MatrixIndex(ShapePosition.Row, ShapePosition.Column + 1));
        _shapePrediction.PredictBlock();
    }

    public void DropDownImmediately()
    {
        var predictedPos = _shapePrediction.PredictedPositions;

        for (var i = 0; i < predictedPos.Count; i++)
        {
            var block = _downSortedBlockList[i];
            var pos = predictedPos[i];

            block.UpdateAllPosition(pos);
        }
    }


    private bool CanMoveDown()
    {
        for (var i = _bottomBlockList.Count - 1; i >= 0; i--)
        {
            var block = _bottomBlockList[i];
            if (block.IsAtBottomRow() || !block.CanMoveDown()) return false;
        }

        return true;
    }

    private bool CanMoveLeft()
    {
        if (!CanMoveDown()) return false;

        for (var i = _leftSideBlockList.Count - 1; i >= 0; i--)
        {
            var block = _leftSideBlockList[i];
            if (block.IsAtLeftBoardSide() || !block.CanMoveLeft()) return false;
        }

        return true;
    }

    private bool CanMoveRight()
    {
        if (!CanMoveDown()) return false;

        for (var i = _rightSideBlockList.Count - 1; i >= 0; i--)
        {
            var block = _rightSideBlockList[i];
            if (block.IsAtRightBoardSide() || !block.CanMoveRight()) return false;
        }

        return true;
    }

    public bool IsMoving()
    {
        return _isMoving;
    }
}