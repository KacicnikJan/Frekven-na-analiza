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
using System.Drawing;
using System.IO;
using System.Security;
using Microsoft.Win32;
using System.Windows.Controls.DataVisualization;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.DataVisualization.Charting;




namespace VIZ_Kacicnik__
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Dictionary<char, int> sifrirano = new Dictionary<char, int>();
        public Dictionary<char, int> referencno = new Dictionary<char, int>();
        public Dictionary<char, int> desif = new Dictionary<char, int>();

        public MainWindow()
        {
            InitializeComponent();
        }

        public string vsebinaReferencno;
        public string vsebinaSifrirano;
        string desifrirano;

        public void btnReferencno_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".txt";
            ofd.Filter = "Text Document (.txt)|*.txt";
            if (ofd.ShowDialog() == true)
            {
                string filename = ofd.FileName;
                // txtboxRef.Text = filename;
                txtboxRef.Text = File.ReadAllText(filename);

                vsebinaReferencno = File.ReadAllText(filename);

            }
            txtCountRef.Clear();
        }
        private void txtboxRef_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnSifrirano_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".txt";
            ofd.Filter = "Text Document (.txt)|*.txt";
            if (ofd.ShowDialog() == true)
            {
                string filename = ofd.FileName;
                // txtboxRef.Text = filename;
                txboxSif.Text = File.ReadAllText(filename);

                vsebinaSifrirano = File.ReadAllText(filename);

            }
            txtCountSif.Clear();

        }

        private void btnFrekSif_Click(object sender, RoutedEventArgs e)
        {
            sifrirano.Clear();
            for (int i = 0; i < vsebinaSifrirano.Length; i++)
            {
                if (Char.IsLetter(vsebinaSifrirano[i]))
                {
                    if (sifrirano.ContainsKey(vsebinaSifrirano[i]))
                    {
                        sifrirano[vsebinaSifrirano[i]]++;
                    }
                    else
                    {
                        sifrirano.Add(vsebinaSifrirano[i], 1);
                    }
                }
            }
            string sifrirano_to_string = string.Join("\n", sifrirano.Select(x => x.Key + "=" + x.Value + ";").ToArray());
            txtCountSif.Text = sifrirano_to_string;
        }

        private void btnFrekRef_Click(object sender, RoutedEventArgs e)
        {
            referencno.Clear();
            for (int i = 0; i < vsebinaReferencno.Length; i++)
            {
                if (Char.IsLetter(vsebinaReferencno[i]))
                {
                    if (referencno.ContainsKey(vsebinaReferencno[i]))
                    {
                        referencno[vsebinaReferencno[i]]++;
                    }
                    else
                    {
                        referencno.Add(vsebinaReferencno[i], 1);
                    }
                }
            }
            string referencno_to_string = string.Join("\n", referencno.Select(x => x.Key + "=" + x.Value + ";").ToArray());
            txtCountRef.Text = referencno_to_string;
        }        

        private void btnSifrirajBesedilo_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<char, char> istoLezeci = new Dictionary<char, char>();
            desifrirano = null;
            desif.Clear();

            for (int i = 0; i < sifrirano.Count; i++) {
                istoLezeci.Add(sifrirano.ElementAt(i).Key, referencno.ElementAt(i).Key);
            }
            for (int i = 0; i < vsebinaSifrirano.Length; i++) {
                if (istoLezeci.ContainsKey(vsebinaSifrirano[i]))
                {
                    desifrirano += istoLezeci[vsebinaSifrirano[i]];
                }
                else {
                    desifrirano += vsebinaSifrirano[i];
                }
            }

            for (int i = 0; i < desifrirano.Length; i++)
            {
                if (Char.IsLetter(desifrirano[i]))
                {
                    if (desif.ContainsKey(desifrirano[i]))
                    {
                        desif[desifrirano[i]]++;
                    }
                    else
                    {
                        desif.Add(desifrirano[i], 1);
                    }
                }
            }
            txtboxDesif.Text = desifrirano;
        }      
        private void btnZamenjaj_Click(object sender, RoutedEventArgs e)
        {
            string a = txt1.Text;
            string b = txt2.Text;
            desifrirano = desifrirano.Replace(a, b);
            txtboxDesif.Text = desifrirano;
            desif.Clear();

            for (int i = 0; i < desifrirano.Length; i++)
            {
                if (Char.IsLetter(desifrirano[i]))
                {
                    if (desif.ContainsKey(desifrirano[i]))
                    {
                        desif[desifrirano[i]]++;
                    }
                    else
                    {
                        desif.Add(desifrirano[i], 1);
                    }
                }
            }
            txt1.Clear();
            txt2.Clear();
        }

        private void btnShrani_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Text File | *.txt",
                    DefaultExt = ".txt",
                    FileName = "Desifrirano.txt"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    using (StreamWriter sW = new StreamWriter(saveFileDialog.FileName))
                    {
                        sW.Write(txtboxDesif.Text);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void btnGraf_Click(object sender, RoutedEventArgs e)
        {
            LoadBarChartData();
        }
        private void LoadBarChartData()
        {
            KeyValuePair<char, int>[] podatki = new KeyValuePair<char, int>[referencno.Count];
            for (int i = 0; i < referencno.Count; i++)
            {
                podatki[i] = new KeyValuePair<char, int>(referencno.ElementAt(i).Key, referencno.ElementAt(i).Value);
            }
            ((BarSeries)mcChart.Series[0]).ItemsSource = podatki;
        }
        private void btnGrafDeSif_Click(object sender, RoutedEventArgs e)
        {
            LoadBarChartData2();
        }
        private void LoadBarChartData2()
        {
            KeyValuePair<char, int>[] podatki = new KeyValuePair<char, int>[desif.Count];
            for (int i = 0; i < referencno.Count; i++)
            {
                podatki[i] = new KeyValuePair<char, int>(desif.ElementAt(i).Key, desif.ElementAt(i).Value);
            }
           ((BarSeries)mcChart.Series[0]).ItemsSource = podatki;
        }
        private void btnGrafSif1_Click(object sender, RoutedEventArgs e)
        {
            LoadBarChartData1();
        }
        private void LoadBarChartData1()
        {
            KeyValuePair<char, int>[] podatki = new KeyValuePair<char, int>[sifrirano.Count];
            for (int i = 0; i < referencno.Count; i++)
            {
                podatki[i] = new KeyValuePair<char, int>(sifrirano.ElementAt(i).Key, sifrirano.ElementAt(i).Value);
            }
           ((BarSeries)mcChart.Series[0]).ItemsSource = podatki;
        }
    }
}
