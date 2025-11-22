using System.Text;

namespace GridPathFinding;

public static class GridParser
{
    public static List<char[,]> ParseGridsFromFile(string filePath)
    {
        List<char[,]> grids = new List<char[,]>();
        
        List<string> gridLines = new List<string>();
        foreach (string line in File.ReadLines(filePath))
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                grids.Add(ParseGrid(gridLines));
                gridLines.Clear();
            }
            else
            {
                gridLines.Add(line);
            }
        }

        if (gridLines.Count > 0)
        {
            grids.Add(ParseGrid(gridLines));
        }

        return grids;
    }

    public static void PrintGrid(char[,] grid)
    {
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                char point = grid[x, y];
                Console.Write(point == GridPoints.Clear ? '.' : point); // Null doesn't show up
            }
            
            Console.WriteLine();
        }
        
        Console.WriteLine();
    }

    public static char[,] ParseGrid(List<string> lines)
    {
        (int gridNumRows, int gridNumCols) = (lines.Count, lines[0].Length);
        char[,] grid = new char[gridNumRows, gridNumCols];

        for (int row = 0; row < gridNumRows; row++)
        {
            string line = lines[row];
            for (int col = 0; col < gridNumCols; col++)
            {
                grid[row, col] = line[col];
            }
        }

        return grid;
    }
}