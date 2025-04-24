using LiveCharts;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp.Model;

namespace WpfApp.ViewModel
{
    public class VmMainWindow : INotifyPropertyChanged
    {
        #region IsTabsEnable
        // Свойство для отключения вкладок до выполнения расчетов
        private bool _isTabsEnable = false;
        public bool IsTabsEnable 
        {
            get { return _isTabsEnable; }
            set
            {
                if (_isTabsEnable != value)
                {
                    _isTabsEnable = value;
                    OnPropertyChanged(nameof(IsTabsEnable)); // Уведомление об изменении свойства
                }
            }
        }
        #endregion
        #region FileContent
        private string _fileContent;
        public string FileContent
        {
            get { return _fileContent; }
            set
            {
                if (_fileContent != value)
                {
                    _fileContent = value;
                    OnPropertyChanged(nameof(FileContent)); // Уведомление об изменении свойства
                }
            }
        }
        #endregion
        #region ListA
        private ObservableCollection<ResultArrayA> _listA;

        public ObservableCollection<ResultArrayA> ListA
        {
            get { return _listA; }
            set
            {
                _listA = value;
                OnPropertyChanged(nameof(ListA));
            }
        }
        #endregion
        #region ListB
        private ObservableCollection<ResultArrayB> _listB;

        public ObservableCollection<ResultArrayB> ListB
        {
            get { return _listB; }
            set
            {
                _listB = value;
                OnPropertyChanged(nameof(ListB));
            }
        }
#endregion
        #region ListC
        private ObservableCollection<ResultArray> _listC;

        public ObservableCollection<ResultArray> ListC
        {
            get { return _listC; }
            set
            {
                _listC = value;
                OnPropertyChanged(nameof(ListC));
            }
        }
        #endregion
        #region ListY
        private ObservableCollection<ResultArray> _listY;

        public ObservableCollection<ResultArray> ListY
        {
            get { return _listY; }
            set
            {
                _listY = value;
                OnPropertyChanged(nameof(ListY));
            }
        }
        #endregion
        #region ListSortY
        private ObservableCollection<ResultArray> _listSortY;

        public ObservableCollection<ResultArray> ListSortY
        {
            get { return _listSortY; }
            set
            {
                _listSortY = value;
                OnPropertyChanged(nameof(ListSortY));
            }
        }
        #endregion
        #region AxisLabels 
        //Массив для подписей к точкам на графике
        private string[] _axisXLabels;
        public string[] AxisXLabels 
        {
            get { return _axisXLabels; }
            set
            {
                _axisXLabels = value;
                OnPropertyChanged(nameof(AxisXLabels)); // Уведомляем об изменении свойства
            }
        } 
        #endregion
        #region Series
        //Коллекция для вывода данных в график
        public SeriesCollection _series;

        public SeriesCollection Series
        {
            get { return _series; }
            set
            {
                _series = value;
                OnPropertyChanged(nameof(Series));
            }
        }
        #endregion

        /// <summary>
        /// Получение коллекции из массива A и массива контрольных сумм
        /// </summary>
        public ObservableCollection<ResultArrayA>
            GetListFromArray(double[] SeriesSumArray, double[] ControlSumsArray)
        {
            ObservableCollection<ResultArrayA> result = new ObservableCollection<ResultArrayA>();
            for (int i = 0; i < SeriesSumArray.Length; i++)
            {
                ResultArrayA res = new ResultArrayA
                {
                    x = i,
                    y = SeriesSumArray[i],
                    ControlSum = ControlSumsArray[i]
                };
                result.Add(res);
            }
            return result;
        }
        
        /// <summary>
        /// Получение коллекции из массива
        /// </summary>
        public ObservableCollection<ResultArray>
            GetListFromArray(double[] array)
        {
            ObservableCollection<ResultArray> result = new ObservableCollection<ResultArray>();
            for (int i = 0; i < array.Length; i++)
            {
                ResultArray res = new ResultArray
                {
                    x = i,
                    y = array[i],
                };
                result.Add(res);
            }
            return result;
        }

        /// <summary>
        /// Получение коллекции из двумерного массива Y
        /// </summary>
        public ObservableCollection<ResultArray>
            GetListFromArray(double[,] array)
        {
            ObservableCollection<ResultArray> result = new ObservableCollection<ResultArray>();
            for (int i = 0; i < array.GetLength(0); i++)
            {
                ResultArrayA res = new ResultArrayA
                {
                    x = array[i, 0],
                    y = array[i, 1],
                };
                result.Add(res);
            }
            return result;
        }

        /// <summary>
        /// Получение коллекции из двумерного массива B
        /// </summary>
        /// <param name="array">Массив</param>
        /// <returns>Коллекция</returns>
        public ObservableCollection<ResultArrayB>
            GetListFromArrayB(double[,] array)
        {
            ObservableCollection<ResultArrayB> result = new ObservableCollection<ResultArrayB>();
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    ResultArrayB res = new ResultArrayB
                    {
                        x = $"[{i}, {j}]",
                        y = array[i, j],
                    };
                    result.Add(res);
                }
            }
            return result;
        }

        /// <summary>
        /// Чтение файла для вывода в TextBlock
        /// </summary>
        public void LoadFileContent(string filePath)
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                FileContent = sr.ReadToEnd();
            }
        }

        public void SaveFile(string filePath, string fileContent)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Выберите место для сохранения";
            saveFileDialog.FileName = System.IO.Path.GetFileNameWithoutExtension(filePath); // Измененное имя
            saveFileDialog.DefaultExt = ".txt";  // Всегда сохраняем как .txt
            saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*"; // Фильтры файлов

            if (saveFileDialog.ShowDialog() == true)
            {
                string destinationFilePath = saveFileDialog.FileName;

                try
                {
                    using (StreamWriter sw = new StreamWriter(destinationFilePath))
                    {
                        sw.Write(fileContent);
                    }
                    MessageBox.Show("Файл успешно сохранён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
