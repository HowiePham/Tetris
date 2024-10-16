using UnityEngine;

public class PhysicSystem : TemporaryMonoSingleton<PhysicSystem>
{
    [SerializeField] private bool canMove;
    [SerializeField] private Timer timer;

    private GameBoard GameBoard => GameBoard.Instance;
    private BlockMatrix BlockMatrix => GameBoard.BlockMatrix;
    private ShapeSpawner ShapeSpawner => ShapeSpawner.Instance;
    private TetrisGameplayConfig TetrisGameplayConfig => TetrisGameplayConfig.Instance;
    private float DefaultSpeed => TetrisGameplayConfig.DefaultSpeed;
    private float MaxSpeed => TetrisGameplayConfig.MaxSpeed;

    private Timer Timer
    {
        get
        {
            if (timer == null)
                timer = new Timer();
            return timer;
        }
    }

    private bool HasPastInterval => Timer.HasPastInterval();

    private void Update()
    {
        if (!HasPastInterval) return;

        if (canMove) MoveDown();
    }

    private void OnEnable()
    {
        ResetBlockSpeed();
    }

    public void ResetBlockSpeed()
    {
        Timer.SetInterval(1 / DefaultSpeed);
    }

    public void IncreaseBlockSpeed()
    {
        Timer.SetInterval(1 / MaxSpeed);
    }

    private void MoveDown()
    {
        var shapeList = ShapeSpawner.OnMovingShapeList;

        for (var i = shapeList.Count - 1; i >= 0; i--)
        {
            var shape = shapeList[i];
            shape.MoveDown();

            if (shape.IsMoving()) continue;
            ShapeSpawner.RemoveFromShapeList(shape);
        }
    }

    public void DropDownImmediately()
    {
        var shapeList = ShapeSpawner.OnMovingShapeList;

        foreach (var shape in shapeList) shape.DropDownImmediately();
    }

    public void StopPhysic()
    {
        canMove = false;
    }

    public void MoveLeft()
    {
        var shapeList = ShapeSpawner.OnMovingShapeList;

        foreach (var shape in shapeList) shape.MoveLeft();
    }

    public void MoveRight()
    {
        var shapeList = ShapeSpawner.OnMovingShapeList;

        foreach (var shape in shapeList) shape.MoveRight();
    }

    public void RotateShape()
    {
        var shapeList = ShapeSpawner.OnMovingShapeList;

        foreach (var shape in shapeList) shape.RotateShape();
    }

    public void MoveAllBlockDown(int row)
    {
        for (var i = row - 1; i >= 0; i--)
        {
            var blocksInRow = BlockMatrix.AtRow(i);

            foreach (var block in blocksInRow) block.MoveDown();
        }

        canMove = true;
    }
}