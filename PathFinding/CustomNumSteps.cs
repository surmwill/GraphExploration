namespace PathFinding;

public struct CustomNumSteps
{
    public (int x, int y) GridPosition { get; }
    
    public int RequiredSteps { get; }

    public CustomNumSteps((int x, int y) gridPosition, int requiredSteps)
    {
        GridPosition = gridPosition;
        RequiredSteps = requiredSteps;
    }
}