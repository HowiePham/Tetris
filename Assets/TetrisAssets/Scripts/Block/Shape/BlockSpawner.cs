using System;
using System.Collections.Generic;
using MarchingBytes;
using UnityEngine;

public class BlockSpawner
{
    private List<BlockController> _blockList;
    private List<BlockController> _bottomBlocks;
    private List<BlockController> _rightSideBlocks;
    private List<BlockController> _leftSideBlocks;
    private Transform _blockRoot;
    private string _blockPoolName;

    public BlockSpawner(Shape shape)
    {
        Shape = shape;
    }

    private int Column { get; set; }
    private int Row { get; set; }
    private Shape Shape { get; }

    public List<BlockController> BlockList
    {
        get => _blockList;
        private set => _blockList = value;
    }

    private int[,] Matrix { get; set; }
    private EasyObjectPool EasyObjectPool => EasyObjectPool.instance;
    private GameState GameState => GameState.Instance;

    public List<BlockController> BottomBlocks
    {
        get => _bottomBlocks;
        private set => _bottomBlocks = value;
    }

    public List<BlockController> RightSideBlocks
    {
        get => _rightSideBlocks;
        set => _rightSideBlocks = value;
    }

    public List<BlockController> LeftSideBlocks
    {
        get => _leftSideBlocks;
        set => _leftSideBlocks = value;
    }

    public void SetMatrix(int[,] matrix)
    {
        Matrix = matrix;
        Row = Matrix.GetLength(0);
        Column = Matrix.GetLength(1);
    }

    private void InitBlockList()
    {
        if (BlockList == null)
        {
            BlockList = new List<BlockController>();
            BottomBlocks = new List<BlockController>();
            RightSideBlocks = new List<BlockController>();
            LeftSideBlocks = new List<BlockController>();
        }
        else
        {
            ClearAllBlock();
        }
    }

    private void ClearAllBlock()
    {
        foreach (var block in BlockList)
        {
            block.ClearBlock();
        }

        BlockList.Clear();
        BottomBlocks.Clear();
        RightSideBlocks.Clear();
        LeftSideBlocks.Clear();
    }

    public void CreateVirtualShape(int startRow, int startColumn, int objectValue, Transform[,] virtualPositionGrid, Transform blockRoot = null, string blockPoolName = "", bool isQueueBlock = false)
    {
        if (blockRoot != null)
        {
            _blockRoot = blockRoot;
        }

        if (!String.IsNullOrEmpty(blockPoolName))
        {
            _blockPoolName = blockPoolName;
        }

        InitBlockList();

        for (var i = 0; i < Row; i++)
        for (var j = 0; j < Column; j++)
        {
            if (Matrix[i, j] == 0)
                continue;

            var block = CreateNewBlock(startRow + i, startColumn + j, virtualPositionGrid, _blockRoot, _blockPoolName, objectValue, isQueueBlock);

            var blockSprite = GameState.BlockColorTemplate[objectValue];
            block.SetRenderer(blockSprite);

            if (isQueueBlock) continue;

            HandleShapeSideBlockList(i, j, block);
        }
    }

    private void HandleShapeSideBlockList(int currentRow, int currentColumn, BlockController block)
    {
        if (currentRow == Row - 1 || Matrix[currentRow + 1, currentColumn] == 0)
        {
            BottomBlocks.Add(block);
        }

        if (currentColumn == Column - 1 || Matrix[currentRow, currentColumn + 1] == 0)
        {
            RightSideBlocks.Add(block);
        }

        if (currentColumn == 0 || Matrix[currentRow, currentColumn - 1] == 0)
        {
            LeftSideBlocks.Add(block);
        }
    }

    private BlockController CreateNewBlock(int row, int column, Transform[,] virtualPositionGrid, Transform blockRoot, string blockPoolName, int objectValue = 1, bool isQueueSand = false)
    {
        var block = EasyObjectPool.GetObjectFromPool(blockPoolName, Vector3.zero, Quaternion.identity);
        block.transform.SetParent(blockRoot);
        block.gameObject.SetActive(true);

        BlockController blockController = block.GetComponent<BlockController>();

        blockController.SetData(row, column, objectValue, isQueueSand, virtualPositionGrid);

        BlockList.Add(blockController);
        blockController.SetCurrentShape(Shape);

        return blockController;
    }

    public void RemoveFromBlockList(BlockController block)
    {
        BlockList.Remove(block);
    }

    public void DestroyAllBlock()
    {
        for (int i = BlockList.Count - 1; i >= 0; i--)
        {
            BlockList[i].DestroyBlock();
            BlockList.RemoveAt(i);
        }
    }

    public void ActiveQueueBlock(bool set)
    {
        foreach (var block in BlockList)
        {
            block.gameObject.SetActive(set);
        }
    }
}