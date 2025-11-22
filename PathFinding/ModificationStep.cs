namespace PathFinding;

public struct ModificationStep
{
    public (int x, int y) GridPosition { get; }
    
    public int Value { get; }

    public ModificationStep((int x, int y) gridPosition, int value)
    {
        GridPosition = gridPosition;
        Value = value;
    }
}