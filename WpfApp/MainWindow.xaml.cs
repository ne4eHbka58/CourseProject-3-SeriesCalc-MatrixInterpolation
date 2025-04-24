using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Logic;
using System.IO;
using System.Reflection;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfApp.FileHelper;
using WpfApp.ViewModel;
using Microsoft.Win32;
using System.Diagnostics;
using WpfApp.Model;
namespace WpfApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string filePath;
        private string fileSpravkaPath;
        

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new VmMainWindow();

            //DataContext = this;
            string fileName = "Calculations.txt";
            string executablePath = System.IO.Directory.GetCurrentDirectory();
            filePath = System.IO.Path.Combine(executablePath, fileName);
        }

        private void CalculateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(iStart.Text, out double tempStart) &&
                double.TryParse(iEnd.Text, out double tempEnd) &&
                double.TryParse(hstep.Text, out double tempHstep) &&
                double.TryParse(epsilon.Text, out double tempEpsilon) &&
                double.TryParse(gstep.Text, out double tempGstep))
            {
                if(Validate(tempStart, tempEnd, tempHstep, tempGstep))
                {
                    Dan initialData = new Dan(tempStart, tempEnd, tempHstep, tempEpsilon, tempGstep);

                    FileFiller fileFiller = new FileFiller(filePath, initialData, (VmMainWindow)DataContext);
                    fileFiller.FillFile();
                }
            }

            else
            {
                MessageBox.Show("Введите корректные значения", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private bool Validate(double tempStart, double tempEnd, double tempHstep, double tempGstep)
        {
            if (tempHstep == 0)
            {
                MessageBox.Show("Введите отличный от 0 шаг по x", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (tempGstep <= 0)
            {
                MessageBox.Show("Введите положительный шаг интерполяции (g)", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (((tempEnd - tempStart) / tempHstep) <= 0)
            {
                MessageBox.Show("Измените значения, при этих данных массивы будут пусты", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        private void ControlBtn_Click(object sender, RoutedEventArgs e)
        {
            iStart.Text = "-0,5";
            iEnd.Text = "0,5";
            hstep.Text = "0,1";
            epsilon.Text = "0,001";
            gstep.Text = "0,1";
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            iStart.Text = "";
            iEnd.Text = "";
            hstep.Text = "";
            epsilon.Text = "";
            gstep.Text = "";
        }

        

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            string fileSpravkaName = "Spravka.chm";
            string chmFilePath = System.IO.Directory.GetCurrentDirectory();
            fileSpravkaPath = System.IO.Path.Combine(chmFilePath, fileSpravkaName);
            try
            {
                Process.Start(fileSpravkaPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось открыть файл справки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveFileBtn_Click(object sender, RoutedEventArgs e)
        {
            VmMainWindow Vm = new VmMainWindow();
            Vm.SaveFile(filePath, TbFileContent.Text);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
