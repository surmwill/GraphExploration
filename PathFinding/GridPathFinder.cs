namespace PathFinding;

public static class GridPathFinder
{
    private static readonly Dictionary<(int numRows, int numCols), char[,]> GridCache = new();

    private static char[,] CreateOrClearGrid(int numRows, int numCols)
    {
        if (!GridCache.TryGetValue((numRows, numCols), out char[,]? grid))
        {
            grid = new char[numRows, numCols];
            GridCache[(numRows, numCols)] = grid;
        }
        else
        {
            Array.Clear(grid, 0, grid.Length);
        }
        
        return grid;
    }

    private static void AddObstaclesToGrid(char[,] grid, IEnumerable<(int row, int col)> obstacles)
    {
        (int numRows, int numCols) gridDimensions = (grid.GetLength(0), grid.GetLength(1));
        foreach ((int row, int col) in obstacles.Where(obstacle => IsPointInGrid(obstacle, gridDimensions)))
        {
            grid[row, col] = GridPoints.Obstacle;
        }
    }

    private static bool IsPointInGrid((int row, int col) point, (int numRows, int numCols) dimensions)
    {
        return point.row >= 0 && point.row < dimensions.numRows && point.col >= 0 && point.col < dimensions.numCols;
    }

    public static void GetPathsFrom(SerializedGrid serializedGrid)
    {
        
    }

    public static NavigationInstructionSet? GetPathTo(SerializedGrid serializedGrid)
    {
        if (!IsPointInGrid(serializedGrid.Target, serializedGrid.Dimensions))
        {
            return null;
        }
        
        if (serializedGrid.Origin == serializedGrid.Target)
        {
            return new NavigationInstructionSet(serializedGrid.Origin, serializedGrid.Target, new List<NavigationInstruction>());
        }
        
        char[,] grid = CreateOrClearGrid(serializedGrid.Dimensions.numRows, serializedGrid.Dimensions.numCols);
        AddObstaclesToGrid(grid, serializedGrid.Obstacles);
        
        Queue<(int row, int col)> nextPositions = new Queue<(int row, int col)>();
        nextPositions.Enqueue(serializedGrid.Origin);
        
        grid[serializedGrid.Origin.row, serializedGrid.Origin.col] = GridPoints.Origin;
        bool found = false;
        
        while (nextPositions.Any())
        {
            (int row, int col) currentPosition = nextPositions.Dequeue();
            if (currentPosition == serializedGrid.Target)
            {
                found = true;
                break;
            }
            
            if (currentPosition.col > 0 && !IsOccupiedPoint(grid[currentPosition.row, currentPosition.col - 1]))
            {
                grid[currentPosition.row, currentPosition.col - 1] = 'R';
                nextPositions.Enqueue((currentPosition.row, currentPosition.col - 1));   
            }

            if (currentPosition.row < serializedGrid.Dimensions.numRows - 1 && !IsOccupiedPoint(grid[currentPosition.row + 1, currentPosition.col]))
            {
                grid[currentPosition.row + 1, currentPosition.col] = 'U';
                nextPositions.Enqueue((currentPosition.row + 1, currentPosition.col));   
            }

            if (currentPosition.col < serializedGrid.Dimensions.numCols - 1 && !IsOccupiedPoint((grid[currentPosition.row, currentPosition.col + 1])))
            {
                grid[currentPosition.row, currentPosition.col + 1] = 'L';
                nextPositions.Enqueue((currentPosition.row, currentPosition.col + 1));   
            }

            if (currentPosition.row > 0 && !IsOccupiedPoint(grid[currentPosition.row - 1, currentPosition.col]))
            {
                grid[currentPosition.row - 1, currentPosition.col] = 'D';
                nextPositions.Enqueue((currentPosition.row - 1, currentPosition.col));   
            }
        }

        if (!found)
        {
            return null;
        }

        int totalMagnitude = 0;
        List<NavigationInstruction> navigationInstructions = new List<NavigationInstruction>();
        ReverseAndAddToPath(serializedGrid.Target, navigationInstructions, ref totalMagnitude, grid);

        return new NavigationInstructionSet(serializedGrid.Origin, serializedGrid.Target, navigationInstructions, totalMagnitude);

        bool IsOccupiedPoint(char gridPoint)
        {
            return gridPoint == GridPoints.Obstacle || gridPoint == GridPoints.Origin ||
                   gridPoint == 'R' || gridPoint == 'L' || gridPoint == 'U' || gridPoint == 'D';
        }
    }

    private static void ReverseAndAddToPath((int row, int col) currentPosition, List<NavigationInstruction> originToTarget, ref int totalMagnitude, char[,] grid)
    {
        totalMagnitude++;
        
        char currentDirection = grid[currentPosition.row, currentPosition.col];
        if (currentDirection == GridPoints.Origin)
        {
            return;
        }
        
        switch (currentDirection)
        {
            case 'R':
            {
                ReverseAndAddToPath((currentPosition.row, currentPosition.col + 1), originToTarget, ref totalMagnitude, grid);
                if (!originToTarget.Any() || originToTarget.Last().Direction != NavigationInstruction.NavigationDirection.Left)
                {
                    originToTarget.Add(new NavigationInstruction(NavigationInstruction.NavigationDirection.Left));
                }
                else
                {
                    NavigationInstruction last = originToTarget.Last();
                    last.IncrementMagnitude();
                    originToTarget[^1] = last;
                }

                break;
            }

            case 'D':
            {
                ReverseAndAddToPath((currentPosition.row + 1, currentPosition.col), originToTarget, ref totalMagnitude, grid);
                if (!originToTarget.Any() || originToTarget.Last().Direction != NavigationInstruction.NavigationDirection.Up)
                {
                    originToTarget.Add(new NavigationInstruction(NavigationInstruction.NavigationDirection.Up));
                }
                else
                {
                    NavigationInstruction last = originToTarget.Last();
                    last.IncrementMagnitude();
                    originToTarget[^1] = last;
                }
                
                break;
            }

            case 'L':
            {
                ReverseAndAddToPath((currentPosition.row, currentPosition.col - 1), originToTarget, ref totalMagnitude, grid);
                if (!originToTarget.Any() || originToTarget.Last().Direction != NavigationInstruction.NavigationDirection.Right)
                {
                    originToTarget.Add(new NavigationInstruction(NavigationInstruction.NavigationDirection.Right));
                }
                else
                {
                    NavigationInstruction last = originToTarget.Last();
                    last.IncrementMagnitude();
                    originToTarget[^1] = last;
                }
                
                break;
            }

            case 'U':
            {
                ReverseAndAddToPath((currentPosition.row - 1, currentPosition.col), originToTarget, ref totalMagnitude, grid);
                if (!originToTarget.Any() || originToTarget.Last().Direction != NavigationInstruction.NavigationDirection.Down)
                {
                    originToTarget.Add(new NavigationInstruction(NavigationInstruction.NavigationDirection.Down));
                }
                else
                {
                    NavigationInstruction last = originToTarget.Last();
                    last.IncrementMagnitude();
                    originToTarget[^1] = last;
                }
                
                break;   
            }
        }
    }
}