namespace GridPathFinding;

public class NavigationInstructionSet
{
    public (int row, int col) Origin { get; }
    
    public (int row, int col) Target { get; }
    
    public List<NavigationInstruction> PathToTarget { get; }
        
    public int TotalMagnitude { get; }

    public NavigationInstructionSet((int row, int col) origin, (int row, int col) target, List<NavigationInstruction> pathToTarget) :
        this(origin, target, pathToTarget, pathToTarget.Sum(instruction => instruction.Magnitude)) { }

    public NavigationInstructionSet((int row, int col) origin, (int row, int col) target, List<NavigationInstruction> pathToTarget, int totalMagnitude)
    {
        Origin = origin;
        Target = target;
        
        PathToTarget = pathToTarget;
        TotalMagnitude = totalMagnitude;
    }

    public void PrintInstructions()
    {
        for (int i = 0; i < PathToTarget.Count; i++)
        {
            NavigationInstruction navigationInstruction = PathToTarget[i];
            Console.WriteLine($"{i}: {navigationInstruction.Direction},{navigationInstruction.Magnitude}");
        }
        Console.WriteLine();
    }
}