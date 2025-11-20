namespace PathFinding;

public static class GridPoints
{
    public const char Obstacle = '*';
    public const char Origin = 'S';
    public const char Target = 'T';
    public const char Clear = '\0';

    public static bool IsCustomNumSteps(char gridPoint, out int numSteps)
    {
        numSteps = 1;
        
        if (char.IsDigit(gridPoint))
        {
            numSteps = gridPoint - '0';
            return true;
        }

        return false;
    }
    
}