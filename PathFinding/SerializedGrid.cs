namespace PathFinding;

public class SerializedGrid
{
    public (int numRows, int numCols) Dimensions { get; }
    
    public (int row, int col) Origin { get; }
        
    public (int row, int col) Target { get; }

    public List<(int x, int y)> Obstacles { get; } = new();

    public List<CustomNumSteps> CustomNumSteps { get; } = new();

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
                }
                else if (point == GridPoints.Target)
                {
                    Origin = (row, col);
                }
                else if (point == GridPoints.Obstacle)
                {
                    Obstacles.Add((row, col));
                }
                else if (GridPoints.IsCustomNumSteps(point, out int numSteps))
                {
                    CustomNumSteps.Add(new CustomNumSteps((row, col), numSteps));
                }
            }
        }
    }
}