namespace GridPathFinding;

public struct NavigationDestination
{
    public (int row, int col) Position { get; }
    
    public (int row, int col) Origin { get; }
    
    public int StepsRequired { get; }

    private char[,] _solvedGrid;

    public NavigationDestination((int row, int col) position, (int row, int col) origin, char[,] solvedGrid)
    {
        Position = position;
        Origin = origin;
        _solvedGrid = solvedGrid;
    }
    
}