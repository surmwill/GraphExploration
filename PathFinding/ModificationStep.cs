namespace PathFinding;

public struct ModificationStep
{
    public (int x, int y) Position { get; }
    
    public int Value { get; }

    public ModificationStep((int x, int y) position, int value)
    {
        Position = position;
        Value = value;
    }
}