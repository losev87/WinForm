using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using WindowsFormsApp2.Service;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        private readonly NNService _service;

        public Form1()
        {
            InitializeComponent();

            _service = new NNService();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog myDialog = new OpenFileDialog();
            myDialog.Filter = "CSV|*.csv";
            myDialog.CheckFileExists = true;
            myDialog.Multiselect = false;
            if (myDialog.ShowDialog() == DialogResult.OK)
            {
                _service.UploadData(myDialog.FileName);

                SetComponentsDefault();

                Draw();
            }
        }


        private void SetComponentsDefault()
        {
            sourceBox.SelectedIndex = 0;
            nwBox.SelectedIndex = 0;
            trainMethodBox.SelectedIndex = 0;
            wantedInputDataBox.SelectedIndex = 0;
            wantedOutputDataBox.SelectedIndex = 0;

            txtQuant.Text = _service.GetQuant().ToString(new CultureInfo("ru-RU"));

            dtPredictSince.MinDate = dtStartDate.MinDate = dtStartDate.Value = _service.GetTrainStartDate();
            txtStartInd.Text = _service.GetTrainStartInd().ToString();
            dtPredictSince.Value = _service.GetPredictStartDate();
            txtPredictSinceInd.Text = _service.GetPredictStartInd().ToString();

            txtLearnLength.Text = _service.GetTrainLength().ToString();
            txtWindowLength.Text = _service.GetWindowSize().ToString();
            txtLayers.Text = "10,1";
            txtPopulation.Text = _service.GetPopulation().ToString();
            txtMutation.Text = _service.GetMutationPercent().ToString();
            txtMate.Text = _service.GetMatePercent().ToString();
            txtPredictCount.Text = _service.GetPredictLength().ToString();

            txtMax.Text = _service.GetMax().ToString(new CultureInfo("ru-Ru"));
            txtMin.Text = _service.GetMin().ToString(new CultureInfo("ru-Ru"));

            chart1.ChartAreas[0].AxisY.Maximum = _service.GetMax();
            chart1.ChartAreas[0].AxisY.Minimum = _service.GetMin();
            chart1.ChartAreas[0].AxisX.Enabled = AxisEnabled.False;
            chart1.Series[0].XValueType = ChartValueType.DateTime;
            chart1.Series[1].XValueType = ChartValueType.DateTime;

            chart2.ChartAreas[0].AxisY.Maximum = _service.GetMax();
            chart2.ChartAreas[0].AxisY.Minimum = _service.GetMin();
            chart2.ChartAreas[0].AxisX.Enabled = AxisEnabled.False;
            chart2.Series[0].XValueType = ChartValueType.DateTime;
            chart2.Series[1].XValueType = ChartValueType.DateTime;

            chart3.ChartAreas[0].AxisY.Maximum = _service.GetMax();
            chart3.ChartAreas[0].AxisY.Minimum = _service.GetMin();
            chart3.ChartAreas[0].AxisX.Enabled = AxisEnabled.False;
            chart3.Series[0].XValueType = ChartValueType.DateTime;
            chart3.Series[1].XValueType = ChartValueType.DateTime;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var layersStr = txtLayers.Text.Split(',');
            var layers = new List<int>();

            foreach (var l in layersStr)
            {
                var n = 0;
                int.TryParse(l.Trim(), out n);
                if (n != 0)
                {
                    layers.Add(n);
                }
            }

            _service.CreateNetwork(layers.ToArray());

            Draw();
        }

        private void TrainCallback(double e, int iter)
        {
            Draw();
            label8.Text = $"Err: {e}";
            label9.Text = $"Iter: {iter}";
            Application.DoEvents();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _service.StartTrain(TrainCallback);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            _service.StopTrain();
        }

        private void txtMax_TextChanged(object sender, EventArgs e)
        {
            if (double.TryParse(txtMax.Text, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, new CultureInfo("ru-RU"), out var v))
            {
                if (v < chart1.ChartAreas[0].AxisY.Minimum + 0.001) v = chart1.ChartAreas[0].AxisY.Minimum + 0.001;
                chart1.ChartAreas[0].AxisY.Maximum = v;
                chart2.ChartAreas[0].AxisY.Maximum = v;
                chart3.ChartAreas[0].AxisY.Maximum = v;
            }
        }

        private void txtMin_TextChanged(object sender, EventArgs e)
        {
            if (double.TryParse(txtMin.Text, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, new CultureInfo("ru-RU"), out var v))
            {
                if (v > chart1.ChartAreas[0].AxisY.Maximum - 0.001) v = chart1.ChartAreas[0].AxisY.Maximum - 0.001;
                chart1.ChartAreas[0].AxisY.Minimum = v;
                chart2.ChartAreas[0].AxisY.Minimum = v;
                chart3.ChartAreas[0].AxisY.Minimum = v;
            }
        }

        private void txtStartInd_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(txtStartInd.Text, out var ind))
            {
                _service.SetTrainStartInd(ind);
                DrawSource();
            }
        }

        private void txtPredictSinceInd_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(txtPredictSinceInd.Text, out var ind))
            {
                _service.SetPredictStartInd(ind);
                DrawPeriodOneByOne();
                DrawNnResult();
            }
        }

        private void txtWindowLength_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(txtWindowLength.Text, out var ws))
            {
                _service.SetWindowSize(ws);
                DrawKnownSource();
            }
        }

        private void txtPredictCount_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(txtPredictCount.Text, out var pc))
            {
                _service.SetPredictCount(pc);
                DrawPeriodOneByOne();
                DrawNnResult();
            }
        }

        private void txtLearnLength_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(txtLearnLength.Text, out int l))
            {
                _service.SetTrainLength(l);
                DrawSource();
            }
        }

        private void dtStartDate_ValueChanged(object sender, EventArgs e)
        {
            dtStartDate.Value = dtStartDate.Value.Date;
            _service.SetTrainStartDate(dtStartDate.Value);

            DrawSource();
        }

        private void txtQuant_TextChanged(object sender, EventArgs e)
        {
            if (double.TryParse(txtQuant.Text, NumberStyles.AllowDecimalPoint, new CultureInfo("ru-RU"), out var q))
                _service.SetQuant(q);
            _service.RecalcQuants();
            Draw();
        }

        private void dtPredictSince_ValueChanged(object sender, EventArgs e)
        {
            dtPredictSince.Value = dtPredictSince.Value.Date;
            _service.SetPredictStartDate(dtPredictSince.Value.Date);
            DrawPeriodOneByOne();
            DrawNnResult();
        }

        private void sourceBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (sourceBox.SelectedIndex)
            {
                case 0:
                    _service.SetWantedSource(SourceType.Raw);
                    txtStartInd.Visible = false;
                    dtStartDate.Visible = true;
                    txtPredictSinceInd.Visible = false;
                    dtPredictSince.Visible = true;
                    chart1.Series[0].XValueType = ChartValueType.DateTime;
                    chart1.Series[1].XValueType = ChartValueType.DateTime;
                    chart2.Series[0].XValueType = ChartValueType.DateTime;
                    chart2.Series[1].XValueType = ChartValueType.DateTime;
                    chart3.Series[0].XValueType = ChartValueType.DateTime;
                    chart3.Series[1].XValueType = ChartValueType.DateTime;
                    txtMax.Text = _service.GetMax().ToString(new CultureInfo("ru-Ru"));
                    txtMin.Text = _service.GetMin().ToString(new CultureInfo("ru-Ru"));
                    break;
                case 1:
                    _service.SetWantedSource(SourceType.QuantDated);
                    txtStartInd.Visible = false;
                    dtStartDate.Visible = true;
                    txtPredictSinceInd.Visible = false;
                    dtPredictSince.Visible = true;
                    chart1.Series[0].XValueType = ChartValueType.DateTime;
                    chart1.Series[1].XValueType = ChartValueType.DateTime;
                    chart2.Series[0].XValueType = ChartValueType.DateTime;
                    chart2.Series[1].XValueType = ChartValueType.DateTime;
                    chart3.Series[0].XValueType = ChartValueType.DateTime;
                    chart3.Series[1].XValueType = ChartValueType.DateTime;
                    txtMax.Text = _service.GetMax().ToString(new CultureInfo("ru-Ru"));
                    txtMin.Text = _service.GetMin().ToString(new CultureInfo("ru-Ru"));
                    break;
                case 2:
                    _service.SetWantedSource(SourceType.Quant);
                    txtStartInd.Visible = true;
                    dtStartDate.Visible = false;
                    txtPredictSinceInd.Visible = true;
                    dtPredictSince.Visible = false;
                    chart1.Series[0].XValueType = ChartValueType.Int32;
                    chart1.Series[1].XValueType = ChartValueType.Int32;
                    chart2.Series[0].XValueType = ChartValueType.Int32;
                    chart2.Series[1].XValueType = ChartValueType.Int32;
                    chart3.Series[0].XValueType = ChartValueType.Int32;
                    chart3.Series[1].XValueType = ChartValueType.Int32;
                    txtMax.Text = _service.GetMax().ToString(new CultureInfo("ru-Ru"));
                    txtMin.Text = _service.GetMin().ToString(new CultureInfo("ru-Ru"));
                    break;
                case 3:
                    _service.SetWantedSource(SourceType.QuantDirection);
                    txtStartInd.Visible = true;
                    dtStartDate.Visible = false;
                    txtPredictSinceInd.Visible = true;
                    dtPredictSince.Visible = false;
                    chart1.Series[0].XValueType = ChartValueType.Int32;
                    chart1.Series[1].XValueType = ChartValueType.Int32;
                    chart2.Series[0].XValueType = ChartValueType.Int32;
                    chart2.Series[1].XValueType = ChartValueType.Int32;
                    chart3.Series[0].XValueType = ChartValueType.Int32;
                    chart3.Series[1].XValueType = ChartValueType.Int32;
                    txtMax.Text = (1.1).ToString(new CultureInfo("ru-RU"));
                    txtMin.Text = (-1.1).ToString(new CultureInfo("ru-RU"));
                    break;
            }

            DrawSource();
        }

        private void nwBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (nwBox.SelectedIndex)
            {
                case 0:
                    _service.SetWantedNnType(NetworkType.FNN);
                    break;
                case 1:
                    _service.SetWantedNnType(NetworkType.RNN);
                    break;
            }
        }

        private void trainMethodBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (trainMethodBox.SelectedIndex)
            {
                case 0:
                    _service.SetTrainMethod(TrainMethod.Specific);
                    break;
                case 1:
                    _service.SetTrainMethod(TrainMethod.BackProp);
                    break;
                case 2:
                    _service.SetTrainMethod(TrainMethod.Resilent);
                    break;
                case 3:
                    _service.SetTrainMethod(TrainMethod.Genetic);
                    break;
            }
        }

        private void txtPopulation_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(txtPopulation.Text, out var p))
                _service.SetPopulation(p);
        }

        private void txtMutation_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(txtMutation.Text, out var p))
                _service.SetMatationPercent(p);
        }

        private void txtMate_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(txtMate.Text, out var p))
                _service.SetMatePercent(p);
        }

        private void wantedOutputDataBox_SelectedIndexChanged(object sender, EventArgs e)
        {

            switch (wantedOutputDataBox.SelectedIndex)
            {
                case 0:
                    _service.SetWantedOutputResultType(ResultType.Pure);
                    break;
                case 1:
                    _service.SetWantedOutputResultType(ResultType.Difference);
                    break;
                case 2:
                    _service.SetWantedOutputResultType(ResultType.Direction);
                    break;
            }
            txtMax.Text = _service.GetMax().ToString(new CultureInfo("ru-Ru"));
            txtMin.Text = _service.GetMin().ToString(new CultureInfo("ru-Ru"));
            Draw();
        }

        private void wantedInputDataBox_SelectedIndexChanged(object sender, EventArgs e)
        {

            switch (wantedInputDataBox.SelectedIndex)
            {
                case 0:
                    _service.SetWantedInputResultType(ResultType.Pure);
                    break;
                case 1:
                    _service.SetWantedInputResultType(ResultType.Difference);
                    break;
                case 2:
                    _service.SetWantedInputResultType(ResultType.Direction);
                    break;
            }
            txtMax.Text = _service.GetMax().ToString(new CultureInfo("ru-Ru"));
            txtMin.Text = _service.GetMin().ToString(new CultureInfo("ru-Ru"));
            Draw();
        }
    }
}
