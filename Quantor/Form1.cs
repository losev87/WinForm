using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quantor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog myDialog = new OpenFileDialog();
            myDialog.Filter = "CSV|*.csv";
            myDialog.CheckFileExists = true;
            myDialog.Multiselect = false;
            if (myDialog.ShowDialog() == DialogResult.OK)
            {
                Create(myDialog.FileName);
            }
        }

        class DTO
        {
            public DateTime Date;
            public double Value;
        }

        private void Create(string fileName)
        {
            List<DTO> result;
            using (System.IO.StreamReader file = new System.IO.StreamReader(fileName, Encoding.UTF8))
            {
                string line;
                //file.ReadLine();
                var ind = 0;
                var quant = double.Parse(textBox1.Text.Replace(".", ","), new CultureInfo("ru-RU"));
                var firstLine = file.ReadLine();
                result = new[] {new DTO {Date = new DateTime(2000, 1, 1),Value= double.Parse(firstLine.Split(',')[5].Replace(".", ","), new CultureInfo("ru-RU")) } }.ToList();
                while ((line = file.ReadLine()) != null)
                {
                    if (line.Length <= 0) continue;
                    
                    
                        var cols = line.Split(',');
                        var val = double.Parse(cols[5].Replace(".", ","), new CultureInfo("ru-RU"));

                    var dif = Math.Abs(val - result.Last().Value);
                    if (dif > quant)
                    {
                        var sign = val < result.Last().Value ? -1 : 1;
                        var steps = (int)(dif / quant);
                        for (int i = 1; i <= steps; i++)
                        {
                            var newQuantVal = result.Last().Value + quant * sign;
                            result.Add(new DTO()
                            {
                                Date = result.Last().Date.AddDays(1),
                                Value = newQuantVal
                            });
                        }
                    }
                }
            }

            Directory.CreateDirectory(Path.Combine(
                Path.GetDirectoryName(fileName),
                "quants"));
            File.WriteAllLines(
                Path.Combine(
                    Path.GetDirectoryName(fileName),
                    "quants",
                    Path.GetFileName(fileName)
                ),
                result.Select(r => $"{r.Date:yyyy.MM.dd HH:mm};{r.Value:0.00000}")
            );
        }
    }
}
