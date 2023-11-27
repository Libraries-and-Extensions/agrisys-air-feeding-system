namespace AgrisysAirFeedingSystem.Utils;

public class MathExtentions
{
    public static double Map(int inMin,int inMax,int outMin,int outMax, int value)
    {
        return (double)((value - inMin) * (outMax - outMin)) / (inMax - inMin) + outMin;
    }
}