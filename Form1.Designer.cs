using System.Windows.Forms;
using System.Drawing;
using System;

namespace Simu
{
    partial class Form1
    {
        private Universo universo = new();
        private System.Windows.Forms.Timer timer;
        private TextBox txtQuantidadeCorpos;
        private TextBox txtQuantidadeInteracoes;
        private TextBox txtTempoEntreInteracoes;
        private Button btnIniciarSimulacao;
        private Button btnContinuarSimulacao;
        private Random random = new Random();
        int quantidadeInteracoes, tempoEntreInteracoes, quantidadeCorpos;
        int count;
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnIniciarSimulacao = new Button();
            this.btnContinuarSimulacao = new Button();
            this.txtQuantidadeCorpos = new TextBox();
            this.txtQuantidadeInteracoes = new TextBox();
            this.txtTempoEntreInteracoes = new TextBox();

            this.WindowState = FormWindowState.Maximized;
            this.Text = "Simulador de gravidade";

            this.txtQuantidadeCorpos.Location = new Point(10, 10);
            this.txtQuantidadeCorpos.Size = new Size(100, 20);
            this.txtQuantidadeCorpos.PlaceholderText = "Quantidade de Corpos";
            this.Controls.Add(this.txtQuantidadeCorpos);

            // Campo de Quantidade de Interações
            this.txtQuantidadeInteracoes.Location = new Point(10, 40);
            this.txtQuantidadeInteracoes.Size = new Size(100, 20);
            this.txtQuantidadeInteracoes.PlaceholderText = "Qtd. de Interações";
            this.Controls.Add(this.txtQuantidadeInteracoes);

            // Campo de Tempo entre Interações
            this.txtTempoEntreInteracoes.Location = new Point(10, 70);
            this.txtTempoEntreInteracoes.Size = new Size(100, 20);
            this.txtTempoEntreInteracoes.PlaceholderText = "Tempo (ms)";
            this.Controls.Add(this.txtTempoEntreInteracoes);

            this.btnIniciarSimulacao.Location = new Point(120, 10);
            this.btnIniciarSimulacao.Size = new Size(100, 30);
            this.btnIniciarSimulacao.Text = "Iniciar Simulação";
            this.btnIniciarSimulacao.Click += new EventHandler(this.btnIniciarSimulacao_Click);

            this.Controls.Add(this.btnIniciarSimulacao);

            this.btnContinuarSimulacao.Location = new Point(220, 10);
            this.btnContinuarSimulacao.Size = new Size(100, 30);
            this.btnContinuarSimulacao.Text = "Continuar Simulação";
            this.btnContinuarSimulacao.Enabled = File.Exists("arquivo.txt");
            this.btnContinuarSimulacao.Click += new EventHandler(this.ContinuarSimulacao);
            this.Controls.Add(this.btnContinuarSimulacao);
        }
        // private void ContinuarSimulacao(object Sender, EventArgs e)
        // {
        //     count = 0;
        //     universo = new();
        //     universo.CarregarConfiguracao();
        //     tempoEntreInteracoes = universo.tempoEntreInteracoes;
        //     quantidadeInteracoes = universo.quantidadeInteracoes;
        //     quantidadeCorpos = universo.qtdCorpos;
        //     InitializeTimer(tempoEntreInteracoes);
        //     Invalidate();
        // }
        private void ContinuarSimulacao(object Sender, EventArgs e)
        {
            try
            {
                count = 0;

                // Carrega a configuração do universo
                universo.CarregarConfiguracao();


                // Inicializa o timer com o intervalo configurado
                InitializeTimer(universo.tempoEntreInteracoes);

                // Revalida a tela
                Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao continuar simulação: {ex.Message}");
            }
        }
        private void btnIniciarSimulacao_Click(object Sender, EventArgs e)
        {
            count = 0;


            if (!int.TryParse(txtQuantidadeCorpos.Text, out quantidadeCorpos) || quantidadeCorpos <= 0)
            {
                MessageBox.Show("Digite uma quantidade válida de corpos.");
                return;
            }

            if (!int.TryParse(txtQuantidadeInteracoes.Text, out quantidadeInteracoes) || quantidadeInteracoes <= 0)
            {
                MessageBox.Show("Digite uma quantidade válida de interações.");
                return;
            }

            if (!int.TryParse(txtTempoEntreInteracoes.Text, out tempoEntreInteracoes) || tempoEntreInteracoes <= 0)
            {
                MessageBox.Show("Digite um tempo válido entre interações (em milissegundos).");
                return;
            }

            universo.Corpos = new Corpo[quantidadeCorpos];
            universo.qtdCorpos = quantidadeCorpos;
            universo.tempoEntreInteracoes = tempoEntreInteracoes;
            universo.quantidadeInteracoes = quantidadeInteracoes;
            for (int i = 0; i < quantidadeCorpos; i++)
            {
                double massa = random.Next(1_000_000, int.MaxValue);
                double densidade = random.Next(1_000_000, int.MaxValue);
                // double posX, posY;
                double posX = random.Next(0, this.ClientSize.Width);
                double posY = random.Next(0, this.ClientSize.Height);
                // do
                // {
                //     posX = random.Next(0, this.ClientSize.Width);
                //     posY = random.Next(0, this.ClientSize.Height);

                // } while (universo.ExistirCorpoNoLugar(posX, posY));
                universo.Corpos[i] = new Corpo($"Corpo {i + 1}", massa, densidade, posX, posY, 0, 0, 0, 0);
                universo.Corpos[i].Raio = universo.Corpos[i].CalcularRaio();
            }

            InitializeTimer(tempoEntreInteracoes); // Inicia o Timer para atualizar os corpos
            Invalidate(); // Redesenha a tela
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (universo.Corpos != null)
            {
                // Obtém o menor e o maior raio para criar uma escala
                double raioMinimo = double.MaxValue;
                double raioMaximo = double.MinValue;

                foreach (var corpo in universo.Corpos)
                {
                    if (corpo.Raio < raioMinimo) raioMinimo = corpo.Raio;
                    if (corpo.Raio > raioMaximo) raioMaximo = corpo.Raio;
                }

                float escalaMinima = 5f; // Tamanho mínimo visível
                float escalaMaxima = 50f; // Tamanho máximo visível

                foreach (var corpo in universo.Corpos)
                {
                    float raioEscalado = (float)((corpo.Raio - raioMinimo) / (raioMaximo - raioMinimo) * (escalaMaxima - escalaMinima) + escalaMinima);

                    float posX = (float)corpo.PosX;
                    float posY = (float)corpo.PosY;

                    e.Graphics.FillEllipse(Brushes.Black, posX - raioEscalado, posY - raioEscalado, raioEscalado * 2, raioEscalado * 2);

                    SizeF textSize = e.Graphics.MeasureString($"{corpo.Raio:F2}", this.Font);
                    e.Graphics.DrawString($"{corpo.PosX}", this.Font, Brushes.Red, posX - textSize.Width / 2, posY - textSize.Height / 2);
                }
            }
        }

        private void InitializeTimer(int tempo)
        {
            if (timer == null)
            {
                timer = new System.Windows.Forms.Timer();
                timer.Interval = tempo; // Aproximadamente 60 FPS
                timer.Tick += (sender, e) => AtualizarMovimento();
            }
            timer.Start();
        }

        private void AtualizarMovimento()
        {
            if (count <= quantidadeInteracoes)
            {
                Console.WriteLine(count);
                universo.CalculoInteracoesGravitacionais();
                universo.SalvarConfiguracao();
                Invalidate();
                count++;
            }
        }
    }
}
