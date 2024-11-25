
namespace Simu;
public partial class Form1 : Form
{
    [System.Runtime.InteropServices.DllImport("kernel32.dll")]
    private static extern bool AllocConsole();

    public Form1()
    {
        AllocConsole();
        InitializeComponent();
    }

}
