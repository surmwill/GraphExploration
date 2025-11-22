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
    
    private static bool IsPointInGrid((int row, int col) point, (int numRows, int numCols) dimensions)
    {
        return point.row >= 0 && point.row < dimensions.numRows && point.col >= 0 && point.col < dimensions.numCols;
    }

    private static void AddSpecialGridPoints(char[,] grid, SerializedGrid serializedGrid)
    {
        (int numRows, int numCols) gridDimensions = (grid.GetLength(0), grid.GetLength(1));
        
        foreach ((int row, int col) in serializedGrid.Obstacles.Where(obstacle => IsPointInGrid(obstacle, gridDimensions)))
        {
            grid[row, col] = GridPoints.Obstacle;
        }
        
        foreach (ModificationStep modificationStep in serializedGrid.ModifySteps.Where(modificationStep => IsPointInGrid(modificationStep.Position, gridDimensions)))
        {
            grid[modificationStep.Position.x, modificationStep.Position.y] = GridPoints.ModificationStepToGridPoint(modificationStep);
        }
    }

    private static char[,] DeserializeGrid(SerializedGrid serializedGrid)
    {
        char[,] grid = CreateOrClearGrid(serializedGrid.Dimensions.numRows, serializedGrid.Dimensions.numCols);
        AddSpecialGridPoints(grid, serializedGrid);
        return grid;
    }

    public static void GetPathsFrom(SerializedGrid serializedGrid, int steps)
    {
        if (!serializedGrid.HasOrigin)
        {
            throw new ArgumentException("No origin");
        }
        
        
    }

    public static NavigationInstructionSet? GetPathTo(SerializedGrid serializedGrid)
    {
        if (!serializedGrid.HasOrigin)
        {
            throw new ArgumentException("No origin");
        }

        if (!serializedGrid.HasTarget)
        {
            throw new ArgumentException("No target");
        }
        
        if (!IsPointInGrid(serializedGrid.Target, serializedGrid.Dimensions))
        {
            return null;
        }
        
        if (serializedGrid.Origin == serializedGrid.Target)
        {
            return new NavigationInstructionSet(serializedGrid.Origin, serializedGrid.Target, []);
        }

        char[,] grid = DeserializeGrid(serializedGrid);
        grid[serializedGrid.Origin.row, serializedGrid.Origin.col] = GridPoints.Origin;
        
        Queue<(int row, int col)> nextPositions = new Queue<(int row, int col)>();
        nextPositions.Enqueue(serializedGrid.Origin);
        
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
                grid[currentPosition.row, currentPosition.col - 1] = GridPoints.DIR_BACK_TO_ORIGIN_RIGHT;
                nextPositions.Enqueue((currentPosition.row, currentPosition.col - 1));   
            }

            if (currentPosition.row < serializedGrid.Dimensions.numRows - 1 && !IsOccupiedPoint(grid[currentPosition.row + 1, currentPosition.col]))
            {
                grid[currentPosition.row + 1, currentPosition.col] = GridPoints.DIR_BACK_TO_ORIGIN_UP;
                nextPositions.Enqueue((currentPosition.row + 1, currentPosition.col));   
            }

            if (currentPosition.col < serializedGrid.Dimensions.numCols - 1 && !IsOccupiedPoint((grid[currentPosition.row, currentPosition.col + 1])))
            {
                grid[currentPosition.row, currentPosition.col + 1] = GridPoints.DIR_BACK_TO_ORIGIN_LEFT;
                nextPositions.Enqueue((currentPosition.row, currentPosition.col + 1));   
            }

            if (currentPosition.row > 0 && !IsOccupiedPoint(grid[currentPosition.row - 1, currentPosition.col]))
            {
                grid[currentPosition.row - 1, currentPosition.col] = GridPoints.DIR_BACK_TO_ORIGIN_DOWN;
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
            return gridPoint == GridPoints.Obstacle || gridPoint == GridPoints.Origin || GridPoints.IsDirectionBackToOrigin(gridPoint);
        }
    }

    private static void ReverseAndAddToPath((int row, int col) currentPosition, List<NavigationInstruction> originToTarget, ref int totalMagnitude, char[,] grid)
    {
        totalMagnitude++;
        
        char dirBackToOrigin = grid[currentPosition.row, currentPosition.col];
        if (dirBackToOrigin == GridPoints.Origin)
        {
            return;
        }
        
        switch (dirBackToOrigin)
        {
            case GridPoints.DIR_BACK_TO_ORIGIN_RIGHT:
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

            case GridPoints.DIR_BACK_TO_ORIGIN_DOWN:
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

            case GridPoints.DIR_BACK_TO_ORIGIN_LEFT:
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

            case GridPoints.DIR_BACK_TO_ORIGIN_UP:
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