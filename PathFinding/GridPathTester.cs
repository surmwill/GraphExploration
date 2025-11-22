using static PathFinding.NavigationInstruction;

namespace PathFinding;

public static class GridPathTester
{
    const string TestGridPathsFileName = "TestGridPaths.txt";

    public static void TestPathTo()
    {
        List<char[,]> grids = GridParser.ParseGridsFromFile(TestGridPathsFileName);

        for (int i = 0; i < grids.Count; i++)
        {
            char[,] grid = grids[i];
            SerializedGrid serializedGrid = new SerializedGrid(grid);
            
            Console.WriteLine($"------------ Test Grid {i} ------------");
            
            NavigationInstructionSet? instructionSet = GridPathFinder.GetPathTo(serializedGrid);
            if (instructionSet != null)
            {
                if (instructionSet.Origin == instructionSet.Target)
                {
                    Console.WriteLine("No path: origin is the same as target");
                }
                else
                {
                    GridParser.PrintGrid(DrawPathOnGrid(instructionSet, grid));
                    instructionSet.PrintInstructions();
                }
            }
            else
            {
                Console.WriteLine("No path found");
                Console.WriteLine();
            }
        }
    }

    private static char[,] DrawPathOnGrid(NavigationInstructionSet instructionSet, char[,] grid)
    {
        char[,] gridCopy = CopyGrid(grid);
        gridCopy[instructionSet.Origin.row, instructionSet.Origin.col] = GridPoints.Origin;
        
        (int row, int col) currentPosition = instructionSet.Origin;
        foreach (NavigationInstruction navigationInstruction in instructionSet.PathToTarget)
        {
            (int moveCols, int moveRows) = (0, 0);

            switch (navigationInstruction.Direction)
            {
                case NavigationDirection.Left:
                    moveCols = navigationInstruction.Magnitude * -1;
                    break;
                
                case NavigationDirection.Right:
                    moveCols = navigationInstruction.Magnitude;
                    break;
                
                case NavigationDirection.Up:
                    moveRows = navigationInstruction.Magnitude * -1;
                    break;
                
                case NavigationDirection.Down:
                    moveRows = navigationInstruction.Magnitude;
                    break;
            }

            for (int i = 0; i < Math.Abs(moveCols); i++)
            {
                currentPosition.col += moveCols > 0 ? 1 : -1;
                gridCopy[currentPosition.row, currentPosition.col] = '.';
            }
            
            for (int i = 0; i < Math.Abs(moveRows); i++)
            {
                currentPosition.row += moveRows > 0 ? 1 : -1;
                gridCopy[currentPosition.row, currentPosition.col] = '.';
            }
        }
        
        gridCopy[instructionSet.Target.row, instructionSet.Target.col] = GridPoints.Target;
        return gridCopy;
    }

    private static char[,] CopyGrid(char[,] grid)
    {
        char[,] copiedGrid = new char[grid.GetLength(0), grid.GetLength(1)];
        Array.Copy(grid, copiedGrid, grid.Length);
        return copiedGrid;
    }
}