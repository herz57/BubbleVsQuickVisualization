using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace BubbleVsQuickVisualization
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public event EventHandler<CustomEventsArgs> BubbleSortProgressUpdate;
        public event EventHandler<CustomEventsArgs> QuickSortProgressUpdate;

        private List<Line> _lines = new List<Line>();
        private List<Line> _lines2 = new List<Line>();
        readonly List<int> _randomLineLengthSequence = new List<int>();

        private int _lineWidth;
        private int _marginLeft;
        private int _lineArea;
        private int _xPosition;
        private int _canvasHeight;
        private int _sleep;
        private bool _bubblePause;
        private bool _quickPause;

        public MainWindow()
        {
            InitializeComponent();
            InitializeParameters();
            SetRandomLineLengths();
            DrawArrayLines(canvas, _lines, Brushes.Coral);
            DrawArrayLines(canvas2, _lines2, Brushes.Coral);
        }

        private void InitializeParameters()
        {
            _lineWidth = 3;
            _marginLeft = 2;
            _lineArea = _lineWidth + _marginLeft;
            _xPosition = 2;
            _canvasHeight = Convert.ToInt32(canvas.Height);
            _sleep = 20;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sleep.Text != "")
            {
                _sleep = int.Parse(sleep.Text);
            } 

            double t;
            BubbleSortProgressUpdate += (s, e) => 
            { 
                Dispatcher.Invoke((Action)delegate() 
                {
                    t = _lines[e.Indexes[0]].Y2;
                    _lines[e.Indexes[0]].Y2 = _lines[e.Indexes[1]].Y2;
                    _lines[e.Indexes[1]].Y2 = t;
                });
            };

            QuickSortProgressUpdate += (s, e) =>
            {
                Dispatcher.Invoke((Action)delegate()
                {
                    
                    t = _lines2[e.Indexes[0]].Y2;
                    _lines2[e.Indexes[0]].Y2 = _lines2[e.Indexes[1]].Y2;
                    _lines2[e.Indexes[1]].Y2 = t;
                });
            };

            double[] array = _lines.Select(l => l.Y2).ToArray();
            double[] array2 = _lines.Select(l => l.Y2).ToArray();

            Task.Run(() =>
            {
                BubbleSort(array);
            });
            Task.Run(() =>
            {
                QuickSort(array2, 0, array2.Length - 1);
            });
        }

        public void BubbleSort(object linesSizes)
        {
            double[] array = (double[])linesSizes;
            double t;

            for (int i = 0; i < array.Length; i++)
            {
                for (int h = 0; h < array.Length - 1; h++)
                {

                    if (array[h] < array[h + 1])
                    {
                        t = array[h];
                        array[h] = array[h + 1];
                        array[h + 1] = t;
                        BubbleSortProgressUpdate(this, new CustomEventsArgs(new int[] { h, h + 1 }));
                        
                    }
                    Thread.Sleep(_sleep);
                }
            }
        }

        public void QuickSort(double[] array, int startIndex, int endIndex)
        {
            int left = startIndex,
                right = endIndex;

            double pivot = array[(startIndex + endIndex) / 2],
                    tmp;

            if (startIndex < endIndex)
            {
                while (left <= right)
                {
                    while (array[left] > pivot)
                    {
                        left++;
                        Thread.Sleep(_sleep);
                    }
                    while (array[right] < pivot)
                    {
                        right--;
                        Thread.Sleep(_sleep);
                    }

                    if (left <= right)
                    {
                        tmp = array[left];
                        array[left] = array[right];
                        array[right] = tmp;
                        QuickSortProgressUpdate(this, new CustomEventsArgs(new int[] { left, right }));
                        left++;
                        right--;
                    }
                    Thread.Sleep(_sleep);
                }
                QuickSort(array, startIndex, right);
                QuickSort(array, left, endIndex);
            }
        }

        private void DrawArrayLines(Canvas canvas, List<Line> lines, SolidColorBrush solidColor)
        {
            int index = 0;

            while (_xPosition < canvas.Width)
            {
                lines.Add(new Line 
                {
                    Stroke = solidColor,
                    X1 = _xPosition,
                    X2 = _xPosition,
                    Y1 = _canvasHeight,
                    Y2 = _randomLineLengthSequence[index],
                    StrokeThickness = _lineWidth
                });
                
                canvas.Children.Add(lines[index]);
                _xPosition += _lineArea;
                index++;
            }
            _xPosition = 2;
        }

        private void SetRandomLineLengths()
        {
            Random rand = new Random();

            for (int i = 0; i < canvas.Width / _lineArea; i++)
            {
                _randomLineLengthSequence.Add(rand.Next(30, _canvasHeight - 10));
            }
        }

        private void bublePause_Click(object sender, RoutedEventArgs e)
        {
            _bubblePause = !_bubblePause;
        }

        private void quickPause_Click(object sender, RoutedEventArgs e)
        {
            _quickPause = !_quickPause;
        }
    }
}
