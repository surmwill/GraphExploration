using System.Text;

namespace PathFinding;

public class NavigationInstructionSet
{
    public List<NavigationInstruction> OriginToTarget { get; }
        
    public int TotalMagnitude { get; }

    public NavigationInstructionSet(List<NavigationInstruction> originToTarget, int totalMagnitude)
    {
        OriginToTarget = originToTarget;
        TotalMagnitude = totalMagnitude;
    }

    public string GetInstructionsString()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < OriginToTarget.Count; i++)
        {
            NavigationInstruction navigationInstruction = OriginToTarget[i];
            sb.AppendLine($"{i}: {navigationInstruction.Direction},{navigationInstruction.Magnitude}");
        }

        return sb.ToString();
    }
}