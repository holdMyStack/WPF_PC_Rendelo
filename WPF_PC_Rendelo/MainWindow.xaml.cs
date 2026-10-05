using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_PC_Rendelo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private Random random = new();

        private void ShowError(string message)
        {
            MessageBox.Show(message, "Gép Rendelő - Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ShowInfo(string message)
        {
            MessageBox.Show(message, "Gép Rendelő - Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private bool ShowConfirm(string message)
        {
            return MessageBoxResult.OK == MessageBox.Show(message, "Gép Rendelő - Megerősítés", MessageBoxButton.OKCancel, MessageBoxImage.Question);
        }

        private bool ValidateName(string name, out string error)
        {
            error = "";

            if (name.Length < 3)
            {
                error = "A név hossza minimum 3 karakter!";
                return false;
            }

            return true;
        }

        private bool ValidateNumber(string numberString, int min, int max, out string error)
        {
            error = "";

            int number;

            try
            {
                number = Convert.ToInt32(numberString);
            }
            catch (FormatException)
            {
                error = "A szám formátuma helytelen!";
                return false;
            }
            catch (OverflowException)
            {
                error = "A szám meghaladja az elfogadott számérték határt!";
                return false;
            }

            if (number < min)
            {
                error = $"A szám minimális elfogadott értéke a {min}.";
                return false;
            }

            if (number > max)
            {
                error = $"A szám maximális elfogadott értéke a {max}.";
                return false;
            }

            return true;
        }

        private bool ValidateEmail(string email, out string error)
        {
            error = "";

            if (email.Length == 0)
            {
                error = "Az e-mail nem lehet üres!";
                return false;
            }

            if (!email.Contains("@"))
            {
                error = "Az e-mail nem tartalmaz '@' betűt.";
                return false;
            }

            if (!email.Contains("."))
            {
                error = "Az e-mail nem tartalmaz '.' betűt.";
                return false;
            }

            if (email.Contains(" "))
            {
                error = "Az e-mail tartalmaz ' ' (szóköz) betűt.";
                return false;
            }

            return true;
        }

        private int GateChecked(RadioButton button, int value)
        {
            if (button.IsChecked ?? false)
            {
                return value;
            }

            return 0;
        }

        private int GateChecked(CheckBox button, int value)
        {
            if (button.IsChecked ?? false)
            {
                return value;
            }

            return 0;
        }

        private string SelectFromList(string[] items, bool?[] conditions)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (conditions[i] ?? false)
                {
                    return items[i];
                }
            }

            return items[0];
        }

        private List<string> FilterList(string[] items, bool?[] conditions)
        {
            List<string> filtered = new();

            for (int i = 0; i < items.Length; i++)
            {
                if (conditions[i] ?? false)
                {
                    filtered.Add(items[i]);
                }
            }

            return filtered;
        }
        
        private void Rendeles_Click(object sender, RoutedEventArgs e)
        {
            string error;

            if (!ValidateName(inputName.Text, out error))
            {
                inputName.Focus();
                ShowError(error);
                return;
            }

            if (!ValidateNumber(inputAge.Text, 14, 120, out error))
            {
                inputAge.Focus();
                ShowError($"Életkor > {error}");
                return;
            }

            if (!ValidateEmail(inputEmail.Text, out error))
            {
                inputEmail.Focus();
                ShowError(error);
                return;
            }

            if (comboProcessor.SelectedIndex == 0)
            {
                comboProcessor.Focus();
                ShowError("Processzor választása kötelező!");
                return;
            }

            bool anyGPUChecked = (radioGPU4060.IsChecked ?? false) ||
                                 (radioGPU4070.IsChecked ?? false) ||
                                 (radioGPU4080.IsChecked ?? false);

            bool anyMemoryChecked = (radioRAM16.IsChecked ?? false) ||
                                    (radioRAM32.IsChecked ?? false) ||
                                    (radioRAM64.IsChecked ?? false);

            if (!anyGPUChecked)
            {
                ShowError("Videokártya kiválasztása kötelező!");
                return;
            }

            if (!anyMemoryChecked)
            {
                ShowError("Memózia mennyiség választása kötelező!");
                return;
            }

            if (!ValidateNumber(inputCount.Text, 1, 5, out error))
            {
                inputCount.Focus();
                ShowError($"Darabszám > {error}");
                return;
            }

            if (!(checkAcceptTerms.IsChecked ?? false))
            {
                checkAcceptTerms.Focus();
                ShowError("A vásárlási feltételek elfogadása kötelező!");
                return;
            }

            int[] processorPrices = { 70_000, 110_000, 65_000, 105_000 };
            int[] warrantyPrices = { 0, 30_000, 60_000 };

            string[] processorNames = { "Intel Core i5", "Intel Core i7", "AMD Ryzen 5", "AMD Ryzen 7" };
            string[] warrantyNames = { "1 éves", "3 éves", "5 éves" };

            string[] gpuNames = { "RTX 4060", "RTX 4070", "RTX 4080" };
            string[] ramNames = { "16 GB", "32 GB", "64 GB" };

            string[] extraNames = { "RGB világítás", "Windows 11", "Gamer billentyűzet", "Gamer egér" };

            int unitCost = processorPrices[comboProcessor.SelectedIndex - 1] +
                           warrantyPrices[comboWarranty.SelectedIndex] +

                           GateChecked(radioGPU4060, 130_000) +
                           GateChecked(radioGPU4070, 220_000) + 
                           GateChecked(radioGPU4080, 400_000) +
                            
                           GateChecked(radioRAM16, 20_000) +
                           GateChecked(radioRAM32, 35_000) +
                           GateChecked(radioRAM64, 65_000) +
                            
                           GateChecked(checkRGB, 15_000) +
                           GateChecked(checkWindows, 45_000) +
                           GateChecked(checkKeyboard, 25_000) +
                           GateChecked(checkMouse, 10_000);

            bool?[] gpuConditions = { radioGPU4060.IsChecked, radioGPU4070.IsChecked, radioGPU4080.IsChecked };
            bool?[] ramConditions = { radioRAM16.IsChecked, radioRAM32.IsChecked, radioRAM64.IsChecked };

            bool?[] extraConditions = { checkRGB.IsChecked, checkWindows.IsChecked, checkKeyboard.IsChecked, checkMouse.IsChecked };

            string kedvezmeny = "";
            int finalCost = unitCost * Convert.ToInt32(inputCount.Text);

            if (finalCost > 500_000)
            {
                kedvezmeny = " (5% kedvezmény)";
                finalCost = (int) (finalCost * 0.95);
            }

            string extrasString = string.Join(",", FilterList(extraNames, extraConditions));

            if (extrasString.Length == 0)
            {
                extrasString = "Nincs";
            }

            string orderSummary =
                $"==== Rendelés Összefoglalő ====\n\n" +
                $"Rendelő:\n" +
                $"    Név: {inputName.Text}\n" +
                $"    Életkor: {inputAge.Text}\n" +
                $"    E-mail: {inputEmail.Text}\n\n" +
                $"Specifikációk:\n" +
                $"    CPU: {processorNames[comboProcessor.SelectedIndex - 1]}\n" +
                $"    GPU: {SelectFromList(gpuNames, gpuConditions)}\n" +
                $"    RAM: {SelectFromList(ramNames, ramConditions)}\n" +
                $"    Extrák: {extrasString}\n\n" +
                $"Rendelés:\n" +
                $"    Garancia: {warrantyNames[comboWarranty.SelectedIndex]}\n" +
                $"    Egységár: {unitCost} Ft\n" +
                $"    Végösszeg: {finalCost} Ft{kedvezmeny}";

            if (ShowConfirm(orderSummary))
            {
                ShowInfo(
                    $"Rendelését sikeresen leadtuk!\n\n" +
                    $"Az ön rendelésének száma: {random.Next(100_000, 999_999)}\n\n" +
                    $"{orderSummary}");
            }
        }
    }
}