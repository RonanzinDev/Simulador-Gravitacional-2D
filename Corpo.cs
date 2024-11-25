namespace Simu;
public class Corpo
{
    public string Nome { get; set; }
    public double Massa { get; set; }
    public double Raio { get; set; }
    public double Densidade { get; set; }
    public double PosX { get; set; }
    public double PosY { get; set; }
    public double VelX { get; set; }
    public double VelY { get; set; }
    public double ForcaX { get; set; }
    public double ForcaY { get; set; }

    public double CalcularRaio()
    {

        return Math.Pow((3 * Massa) / (4 * Math.PI * Densidade), 1.0 / 3.0);
    }

    public Corpo(string nome, double massa, double densidade, double posX, double posY, double velX, double velY, double forX, double forY)
    {
        Nome = nome;
        Massa = massa;
        Densidade = densidade;
        PosX = posX;
        PosY = posY;
        VelX = velX;
        VelY = velY;
        ForcaX = forX;
        ForcaY = forY;

    }
      public Corpo(string nome, double massa, double densidade, double raio, double posX, double posY, double velX, double velY)
    {
        Nome = nome;
        Massa = massa;
        Densidade = densidade;
        Raio = raio;
        PosX = posX;
        PosY = posY;
        VelX = velX;
        VelY = velY;

    }

    public static Corpo operator +(Corpo corpoA, Corpo corpoB)
    {
        if (corpoA == null || corpoB == null) throw new ArgumentNullException("Corpos não podem ser nulos.");
        double massaTotal = corpoA.Massa + corpoB.Massa;
        corpoA.VelX = (corpoA.VelX * corpoA.Massa + corpoB.VelX * corpoB.Massa) / massaTotal;
        corpoA.VelY = (corpoA.VelY * corpoA.Massa + corpoB.VelY * corpoB.Massa) / massaTotal;
        corpoA.Massa = massaTotal;
          corpoA.Raio = Math.Sqrt(corpoA.Raio * corpoA.Raio + corpoB.Raio * corpoB.Raio);
        return corpoA;
    }

}
