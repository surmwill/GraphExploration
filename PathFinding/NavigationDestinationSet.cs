namespace GridPathFinding;

public class NavigationDestinationSet
{
    public (int row, int col) Origin { get; }
    
    public int MaxNumSteps { get; }
    
    public NavigationDestination?[,] DestinationMap;
    
    public List<NavigationDestination> ValidDestinations { get; }

    public bool HasValidDestination => ValidDestinations.Count > 0;

    public NavigationDestinationSet((int row, int col) origin, int maxNumSteps, NavigationDestination?[,] destinationMap, List<NavigationDestination> validDestinations)
    {
        Origin = origin;
        MaxNumSteps = maxNumSteps;
        
        DestinationMap = destinationMap;
        ValidDestinations = validDestinations;
    }
}