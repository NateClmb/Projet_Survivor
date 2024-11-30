namespace Projet_Survivor;

public class Position
{
    private double x;
    private double y;

    public double X
    {
        get => x;
        set => x = value;
    }

    public double Y
    {
        get => y;
        set => y = value;
    }

    public Position()
    {
        x = 100;
        y = 100;
    }
}