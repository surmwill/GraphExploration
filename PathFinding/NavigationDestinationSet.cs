namespace GridPathFinding;

public class NavigationDestinationSet
{
    public NavigationDestination?[,] NavigationDestinations;
    
    public List<NavigationDestination> ValidDestinations { get; }

    public bool CanMove => ValidDestinations.Count > 0;

    public NavigationDestinationSet(NavigationDestination?[,] navigationDestinations, List<NavigationDestination> validDestinations)
    {
        NavigationDestinations = navigationDestinations;
        ValidDestinations = validDestinations;
    }
}