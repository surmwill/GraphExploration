namespace PathFinding;

public class GridPathFinder
{
    private readonly Dictionary<(int width, int height), char[,]> _gridCache = new();

    private const char ObstacleOnGrid = '*';
    const char OriginOnGrid = 'O';
    const char ClearOnGrid = '\0';

    private char[,] CreateOrClearGrid(int width, int height)
    {
        if (!_gridCache.TryGetValue((width, height), out char[,]? grid))
        {
            grid = new char[width, height];
            _gridCache[(width, height)] = grid;
        }
        else
        {
            Array.Clear(grid, 0, grid.Length);
        }
        
        return grid;
    }

    private void AddObstaclesToGrid(char[,] grid, IEnumerable<(int x, int y)> obstacles)
    {
        (int width, int height) gridDimensions = (grid.GetLength(0), grid.GetLength(1));
        foreach ((int x, int y) in obstacles.Where(obstacle => IsPointInGrid(obstacle, gridDimensions)))
        {
            grid[x, y] = ObstacleOnGrid;
        }
    }

    private bool IsPointInGrid((int x, int y) point, (int width, int height) dimensions)
    {
        return point.x >= 0 && point.x < dimensions.width && point.y >= 0 && point.y < dimensions.height;
    }

    public NavigationInstructionSet? GetPathTo((int x, int y) origin, (int x, int y) target, (int width, int height) gridDimensions, IEnumerable<(int x, int y)> obstacles)
    {
        if (!IsPointInGrid(target, gridDimensions))
        {
            return null;
        }
        
        char[,] grid = CreateOrClearGrid(gridDimensions.width, gridDimensions.height);
        AddObstaclesToGrid(grid, obstacles);
        
        Queue<(int x, int y)> nextPositions = new Queue<(int x, int y)>();
        nextPositions.Enqueue(origin);
        
        grid[origin.x, origin.y] = OriginOnGrid;
        char prevDirectionFromTarget = ClearOnGrid;
        
        while (nextPositions.Any())
        {
            (int x, int y) currentPosition = nextPositions.Dequeue();
            if (!IsPointInGrid(currentPosition, gridDimensions) || grid[currentPosition.x, currentPosition.y] != ClearOnGrid)
            {
                continue;
            }

            if (currentPosition == target)
            {
                prevDirectionFromTarget = grid[currentPosition.x, currentPosition.y];
                break;
            }

            grid[currentPosition.x - 1, currentPosition.y] = 'R';
            nextPositions.Enqueue((currentPosition.x - 1, currentPosition.y));

            grid[currentPosition.x, currentPosition.y + 1] = 'D';
            nextPositions.Enqueue((currentPosition.x, currentPosition.y + 1));

            grid[currentPosition.x + 1, currentPosition.y] = 'L';
            nextPositions.Enqueue((currentPosition.x + 1, currentPosition.y));

            grid[currentPosition.x, currentPosition.y - 1] = 'U';
            nextPositions.Enqueue((currentPosition.x, currentPosition.y - 1));
        }

        if (prevDirectionFromTarget == ClearOnGrid)
        {
            return null;
        }

        int totalMagnitude = 0;
        List<NavigationInstruction> navigationInstructions = new List<NavigationInstruction>();
        ReverseAndAddToPath(target, navigationInstructions, ref totalMagnitude, grid);

        return new NavigationInstructionSet(navigationInstructions, totalMagnitude);
    }

    private void ReverseAndAddToPath((int x, int y) currentPosition, List<NavigationInstruction> originToTarget, ref int totalMagnitude, char[,] grid)
    {
        totalMagnitude++;
        
        char currentDirection = grid[currentPosition.x, currentPosition.y];
        if (currentDirection == OriginOnGrid)
        {
            return;
        }
        
        switch (currentDirection)
        {
            case 'R':
            {
                ReverseAndAddToPath((currentPosition.x + 1, currentPosition.y), originToTarget, ref totalMagnitude, grid);
                NavigationInstruction? lastInstruction = originToTarget.LastOrDefault();
                
                if (!lastInstruction.HasValue || lastInstruction.Value.Direction != NavigationInstruction.NavigationDirection.Right)
                {
                    originToTarget.Add(new NavigationInstruction(NavigationInstruction.NavigationDirection.Right));
                }
                else
                {
                    originToTarget.Last().IncrementMagnitude();
                }

                break;
            }

            case 'D':
            {
                ReverseAndAddToPath((currentPosition.x, currentPosition.y - 1), originToTarget, ref totalMagnitude, grid);
                NavigationInstruction? lastInstruction = originToTarget.LastOrDefault();
                
                if (!lastInstruction.HasValue || lastInstruction.Value.Direction != NavigationInstruction.NavigationDirection.Down)
                {
                    originToTarget.Add(new NavigationInstruction(NavigationInstruction.NavigationDirection.Down));
                }
                else
                {
                    originToTarget.Last().IncrementMagnitude();
                }
                
                break;
            }

            case 'L':
            {
                ReverseAndAddToPath((currentPosition.x - 1, currentPosition.y), originToTarget, ref totalMagnitude, grid);
                NavigationInstruction? lastInstruction = originToTarget.LastOrDefault();
                
                if (!lastInstruction.HasValue || lastInstruction.Value.Direction != NavigationInstruction.NavigationDirection.Left)
                {
                    originToTarget.Add(new NavigationInstruction(NavigationInstruction.NavigationDirection.Left));
                }
                else
                {
                    originToTarget.Last().IncrementMagnitude();
                }
                
                break;
            }

            case 'U':
            {
                ReverseAndAddToPath((currentPosition.x, currentPosition.y + 1), originToTarget, ref totalMagnitude, grid);
                NavigationInstruction? lastInstruction = originToTarget.LastOrDefault();
                
                if (!lastInstruction.HasValue || lastInstruction.Value.Direction != NavigationInstruction.NavigationDirection.Up)
                {
                    originToTarget.Add(new NavigationInstruction(NavigationInstruction.NavigationDirection.Up));
                }
                else
                {
                    originToTarget.Last().IncrementMagnitude();
                }
                
                break;   
            }
        }
    }
}