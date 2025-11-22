namespace PathFinding;

public struct ModifyStepBy
{
    public (int x, int y) GridPosition { get; }
    
    public int Value { get; }

    public ModifyStepBy((int x, int y) gridPosition, int value)
    {
        GridPosition = gridPosition;
        Value = value;
    }
}