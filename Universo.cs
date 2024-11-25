namespace Simu;

public class Universo
{
    public const double G = 6.6743e-11;
    public Corpo[] Corpos { get; set; }
    public int tempoEntreInteracoes { get; set; }
    public int quantidadeInteracoes { get; set; }
    public int qtdCorpos { get; set; }
    public Universo()
    {
    }

    public void CalculoInteracoesGravitacionais()
    {
        foreach (Corpo corpo in Corpos)
        {
            corpo.ForcaX = 0;
            corpo.ForcaY = 0;
        }
        for (int i = 0; i < qtdCorpos; i++)
        {
            for (int j = i + 1; j < qtdCorpos; j++)
            {
                var corpoA = Corpos[i];
                var corpoB = Corpos[j];
                double deltaX = corpoB.PosX - corpoA.PosX;
                double deltaY = corpoB.PosY - corpoA.PosY;
                double distancia = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
                if (distancia <= corpoA.Raio + corpoB.Raio)
                {
                    corpoA += corpoB;
                    RemoverCorpo(corpoB);
                    j--;
                    continue;
                }
                if (distancia > 0)
                {
                    double forca = G * corpoA.Massa * corpoB.Massa / (distancia * distancia);
                    double forcaX = forca * deltaX / distancia;
                    double forcaY = forca * deltaY / distancia;

                    corpoA.ForcaX += forcaX;
                    corpoA.ForcaY += forcaY;
                    corpoB.ForcaX -= forcaX;
                    corpoB.ForcaY -= forcaY;
                }
            }
        }

        foreach (var corpo in Corpos)
        {


            // corpo.PosX += corpo.VelX * escalaVelocidade;
            // corpo.PosY += corpo.VelY * escalaVelocidade;
            Parallel.Invoke(() => CalculasPosX(corpo), () => CalculasPosY(corpo));
        }
    }
    const double escalaVelocidade = 10.0; // Escala visual da velocidade
    const double deltaTime = 10.0; // Escala de tempo para acelerar o movimento
    public void CalculasPosX(Corpo corpo)
    {
        double acelX = corpo.ForcaX / corpo.Massa;
        corpo.VelX += acelX * deltaTime;
        corpo.PosX += corpo.VelX * escalaVelocidade;

    }
    public void CalculasPosY(Corpo corpo)
    {
        double acelY = corpo.ForcaY / corpo.Massa;
        corpo.VelY += acelY * deltaTime;
        corpo.PosY += corpo.VelY * escalaVelocidade;

    }
    public bool ExistirCorpoNoLugar(double posX, double posY)
    {
        foreach (var corpo in Corpos)
        {
            if (corpo != null)
            {
                double distancia = Math.Sqrt(Math.Pow(corpo.PosX - posX, 2) + Math.Pow(corpo.PosY - posY, 2));
                if (distancia < corpo.Raio * 2) return true;
            }
        }
        return false;
    }
    public void RemoverCorpo(Corpo corpoRemovido)
    {
        Corpos = Corpos.Where(corpo => corpo != corpoRemovido).ToArray();
        qtdCorpos = Corpos.Length;
    }
    public void SalvarConfiguracao(string caminhoArquivo = "arquivo.txt")
    {
        using (StreamWriter writer = new StreamWriter(caminhoArquivo))
        {
            writer.WriteLine($"{qtdCorpos};{quantidadeInteracoes};{tempoEntreInteracoes}");
            foreach (var corpo in Corpos)
            {
                if (corpo != null)
                {
                    writer.WriteLine($"{corpo.Nome};{corpo.Massa};{corpo.Densidade};{corpo.Raio};{corpo.PosX};{corpo.PosY};{corpo.VelX};{corpo.VelY}");
                }
            }
        }
    }

    public void CarregarConfiguracao(string caminhoArquivo = "arquivo.txt")
    {
        using (StreamReader reader = new StreamReader(caminhoArquivo))
        {
            string[] configuracoes = reader.ReadLine().Split(';');
            qtdCorpos = int.Parse(configuracoes[0]);
            quantidadeInteracoes = int.Parse(configuracoes[1]);
            tempoEntreInteracoes = int.Parse(configuracoes[2]);

            Corpos = new Corpo[qtdCorpos];
            int index = 0;
            string linha;
            while ((linha = reader.ReadLine()) != null)
            {
                string[] dados = linha.Split(';');
                string nome = dados[0];
                double massa = double.Parse(dados[1]);
                double densidade = double.Parse(dados[2]);
                double raio = double.Parse(dados[3]);
                double posX = double.Parse(dados[4]);
                double posY = double.Parse(dados[5]);
                double velX = double.Parse(dados[6]);
                double velY = double.Parse(dados[7]);

                Corpos[index] = new Corpo(nome, massa, densidade, raio, posX, posY, velX, velY);
                index++;
            }
        }
    }
}
