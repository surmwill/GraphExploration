namespace PathFinding;

public struct SerializedGrid
{
    public (int numRows, int numCols) Dimensions { get; }
    
    public (int row, int col) Origin { get; }
    
    public (int row, int col) Target { get; }
    
    public List<(int x, int y)> Obstacles => _obstacles ??= new List<(int x, int y)>();
    
    public List<ModificationStep> ModifySteps => _modificationSteps ??= new List<ModificationStep>();
    
    public bool HasOrigin { get; }
    
    public bool HasTarget { get; }

    public bool HasObstacles => _obstacles != null;
    
    public bool HasModificationSteps => _modificationSteps != null;
    
    private List<(int x, int y)>? _obstacles;

    private List<ModificationStep>? _modificationSteps;

    public SerializedGrid(
        (int numRows, int numCols) dimensions, 
        (int row, int col) origin, (int row, int col) target, 
        List<(int x, int y)>? obstacles, 
        List<ModificationStep>? modificationSteps)
    {
        Dimensions = dimensions;
        Origin = origin;
        Target = target;
        _obstacles = obstacles;
        _modificationSteps = modificationSteps;
    }

    public SerializedGrid(char[,] grid)
    {
        Dimensions = (grid.GetLength(0), grid.GetLength(1));
        
        for (int row = 0; row < grid.GetLength(0); row++)
        {
            for (int col = 0; col < grid.GetLength(1); col++)
            {
                char point = grid[row, col];
                    
                if (point == GridPoints.Origin)
                {
                    Origin = (row, col);
                    HasOrigin = true;
                }
                else if (point == GridPoints.Target)
                {
                    Target = (row, col);
                    HasTarget = true;
                }
                else if (point == GridPoints.Obstacle)
                {
                    Obstacles.Add((row, col));
                }
                else if (GridPoints.IsModificationStep(point, out int numSteps))
                {
                    ModifySteps.Add(new ModificationStep((row, col), numSteps));
                }
            }
        }
    }
}