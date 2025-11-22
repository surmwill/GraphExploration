namespace GridPathFinding;

public static class GridTestingUtilities
{
    public static char[,] CopyGrid(char[,] grid)
    {
        char[,] copiedGrid = new char[grid.GetLength(0), grid.GetLength(1)];
        Array.Copy(grid, copiedGrid, grid.Length);
        return copiedGrid;
    }
}