namespace PathFinding;

public static class GridPoints
{
    public const char Obstacle = '*';
    public const char Origin = 'S';
    public const char Target = 'T';
    public const char Clear = '\0';

    public const char DIR_BACK_TO_ORIGIN_LEFT = 'L';
    public const char DIR_BACK_TO_ORIGIN_RIGHT = 'R';
    public const char DIR_BACK_TO_ORIGIN_UP = 'U';
    public const char DIR_BACK_TO_ORIGIN_DOWN = 'B';

    public static bool IsDirectionBackToOrigin(char gridPoint)
    {
        return gridPoint == DIR_BACK_TO_ORIGIN_RIGHT || 
               gridPoint == DIR_BACK_TO_ORIGIN_LEFT || 
               gridPoint == DIR_BACK_TO_ORIGIN_UP || 
               gridPoint == DIR_BACK_TO_ORIGIN_DOWN;
    }

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