using PathFinding;

const string TestGridPathsFileName = "TestGridPaths.txt";

List<char[,]> grids = GridParser.ParseGridsFromFile(TestGridPathsFileName);

foreach (char[,] grid in grids)
{
    GridParser.PrintGrid(grid); 
    Console.WriteLine();
}

Console.WriteLine(AppContext.BaseDirectory);