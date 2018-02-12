using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using WindowsFormsApp2.Service;

namespace WindowsFormsApp2
{
    partial class Form1
    {
        private void Draw()
        {
            DrawSource();
            DrawPeriodOneByOne();
            DrawNnResult();
        }

        private void DrawSource()
        {
            DrawKnownSource();
            DrawTrainSource();
        }

        private void DrawKnownSource()
        {
            var data = _service.GetKnownSourceData();
            SetAsixX(chart2,data);
            DrawChart(chart2.Series[0], data);
        }

        private void DrawTrainSource()
        {
            var data = _service.GetTrainSourceData();
            SetAsixX(chart2, data);
            DrawChart(chart2.Series[1], data);
        }

        private void DrawPeriodOneByOne()
        {
            DrawKnownPerionOneByOne();
            DrawTrainPerionOneByOne();
        }

        private void DrawKnownPerionOneByOne()
        {
            var data = _service.GetKnownPerionOneByOneData();
            SetAsixX(chart3, data);
            DrawChart(chart3.Series[0], data);
        }

        private void DrawTrainPerionOneByOne()
        {
            var data = _service.GetTrainPeriodOneByOneData();
            SetAsixX(chart3, data);
            DrawChart(chart3.Series[1], data);
        }

        private void DrawNnResult()
        {
            DrawKnownNnResult();
            DrawTrainNnResult();
        }

        private void DrawKnownNnResult()
        {
            var data = _service.GetKnownNnResultData();
            SetAsixX(chart1, data);
            DrawChart(chart1.Series[0], data);
        }

        private void DrawTrainNnResult()
        {
            var data = _service.GetTrainNnResultData();
            SetAsixX(chart1, data);
            DrawChart(chart1.Series[1], data);
        }

        private void DrawChart(Series series, IndValueDto[] data, int digits = 5)
        {
            series.Points.Clear();

            if (data == null || data.Count() < 2) return;

            var divider = Math.Pow(10, digits);

            var resultType = _service.GetWantedOutput();

            foreach (var dto in data)
            {
                series.Points.AddXY(dto.GetArg, ((int)(dto.Val[(int)resultType] * divider)) / divider);
            }
        }

        private void SetAsixX(Chart chart, IndValueDto[] data)
        {
            if (data.Any())
            {
                if (data.First().GetArg is int)
                {
                    chart.ChartAreas[0].AxisX.Minimum =
                        data.Select(d => (double) (int) d.GetArg).Min();
                    chart.ChartAreas[0].AxisX.Maximum =
                        data.Select(d => (double)(int)d.GetArg).Max() + 1;
                }
                else
                {
                    chart.ChartAreas[0].AxisX.Minimum =
                        data.Select(d => (DateTime)d.GetArg).Min().ToOADate();
                    chart.ChartAreas[0].AxisX.Maximum =
                        data.Select(d => (DateTime) d.GetArg).Max().AddHours(4).ToOADate();
                }
            }
        }
    }
}
