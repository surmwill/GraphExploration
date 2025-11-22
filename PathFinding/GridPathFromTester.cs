namespace GridPathFinding;

public class GridPathFromTester
{
    const string TestGridsFileName = "TestPathFromGrids.txt";

    public static void TestPathsFrom()
    {
        TestPathsFromWithMaxSteps(1);
    }

    private static void TestPathsFromWithMaxSteps(int maxNumSteps)
    {
        List<char[,]> grids = GridParser.ParseGridsFromFile(TestGridsFileName);

        for (int i = 0; i < grids.Count; i++)
        {
            char[,] grid = grids[i];
            SerializedGrid serializedGrid = new SerializedGrid(grid);
            
            Console.WriteLine($"------------ Path From - Max Steps ({maxNumSteps}) - Test Grid {i} ------------");

            NavigationDestinationSet navigationDestinationSet = GridPathFinder.GetPathsFrom(serializedGrid, maxNumSteps);
            if (navigationDestinationSet.HasValidDestination)
            {
                GridParser.PrintGrid(DrawDestinationsOnGrid(navigationDestinationSet, grid));
            }
            else
            {
                Console.WriteLine("No valid destinations");
                Console.WriteLine();
            }
        }
    }

    private static char[,] DrawDestinationsOnGrid(NavigationDestinationSet navigationDestinationSet, char[,] grid)
    {
        char[,] gridCopy = GridTestingUtilities.CopyGrid(grid);
        foreach (NavigationDestination navigationDestination in navigationDestinationSet.ValidDestinations)
        {
            gridCopy[navigationDestination.Position.row, navigationDestination.Position.col] = GridPoints.DEBUG_PRINT_PATH;
        }
        return gridCopy;
    }
}