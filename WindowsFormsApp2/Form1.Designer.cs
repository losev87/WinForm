namespace WindowsFormsApp2
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series7 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series8 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series9 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series10 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea6 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series11 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series12 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.button1 = new System.Windows.Forms.Button();
            this.dtStartDate = new System.Windows.Forms.DateTimePicker();
            this.txtLearnLength = new System.Windows.Forms.TextBox();
            this.txtWindowLength = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtLayers = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.chart2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.dtPredictSince = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.trainMethodBox = new System.Windows.Forms.ComboBox();
            this.chart3 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.txtPopulation = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtMutation = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtMate = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.txtMax = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtMin = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.sourceBox = new System.Windows.Forms.ComboBox();
            this.txtQuant = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtStartInd = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtPredictSinceInd = new System.Windows.Forms.TextBox();
            this.nwBox = new System.Windows.Forms.ComboBox();
            this.txtPredictCount = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.wantedInputDataBox = new System.Windows.Forms.ComboBox();
            this.label19 = new System.Windows.Forms.Label();
            this.wantedOutputDataBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(127, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Загрузить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dtStartDate
            // 
            this.dtStartDate.Location = new System.Drawing.Point(12, 175);
            this.dtStartDate.Name = "dtStartDate";
            this.dtStartDate.Size = new System.Drawing.Size(127, 20);
            this.dtStartDate.TabIndex = 2;
            this.dtStartDate.ValueChanged += new System.EventHandler(this.dtStartDate_ValueChanged);
            // 
            // txtLearnLength
            // 
            this.txtLearnLength.Location = new System.Drawing.Point(12, 214);
            this.txtLearnLength.Name = "txtLearnLength";
            this.txtLearnLength.Size = new System.Drawing.Size(127, 20);
            this.txtLearnLength.TabIndex = 3;
            this.txtLearnLength.TextChanged += new System.EventHandler(this.txtLearnLength_TextChanged);
            // 
            // txtWindowLength
            // 
            this.txtWindowLength.Location = new System.Drawing.Point(12, 266);
            this.txtWindowLength.Name = "txtWindowLength";
            this.txtWindowLength.Size = new System.Drawing.Size(127, 20);
            this.txtWindowLength.TabIndex = 3;
            this.txtWindowLength.TextChanged += new System.EventHandler(this.txtWindowLength_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 159);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Взять данные с";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 198);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "В количестве";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 237);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Размер окна";
            // 
            // txtLayers
            // 
            this.txtLayers.Location = new System.Drawing.Point(12, 332);
            this.txtLayers.Name = "txtLayers";
            this.txtLayers.Size = new System.Drawing.Size(127, 20);
            this.txtLayers.TabIndex = 3;
            this.txtLayers.Text = "10,1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 284);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 13);
            this.label4.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 250);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "(кол-во входов)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 316);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(117, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Слои или нейр,сл,вых";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 358);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(127, 23);
            this.button2.TabIndex = 0;
            this.button2.Text = "Создать сеть";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // chart2
            // 
            chartArea4.Name = "ChartArea2";
            this.chart2.ChartAreas.Add(chartArea4);
            this.chart2.Location = new System.Drawing.Point(142, 12);
            this.chart2.Name = "chart2";
            series7.ChartArea = "ChartArea2";
            series7.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series7.Name = "Series1";
            series7.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            series8.ChartArea = "ChartArea2";
            series8.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series8.Name = "Series2";
            series8.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            this.chart2.Series.Add(series7);
            this.chart2.Series.Add(series8);
            this.chart2.Size = new System.Drawing.Size(1026, 272);
            this.chart2.TabIndex = 1;
            this.chart2.Text = "chart2";
            // 
            // dtPredictSince
            // 
            this.dtPredictSince.Location = new System.Drawing.Point(9, 599);
            this.dtPredictSince.Name = "dtPredictSince";
            this.dtPredictSince.Size = new System.Drawing.Size(127, 20);
            this.dtPredictSince.TabIndex = 2;
            this.dtPredictSince.Value = new System.DateTime(2009, 5, 1, 0, 0, 0, 0);
            this.dtPredictSince.ValueChanged += new System.EventHandler(this.dtPredictSince_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 583);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(127, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Предсказать начиная с";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 531);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(99, 23);
            this.button3.TabIndex = 5;
            this.button3.Text = "Обучить";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 557);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(23, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "Err:";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(117, 531);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(22, 23);
            this.button4.TabIndex = 6;
            this.button4.Text = "X";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 570);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(25, 13);
            this.label9.TabIndex = 4;
            this.label9.Text = "Iter:";
            // 
            // trainMethodBox
            // 
            this.trainMethodBox.FormattingEnabled = true;
            this.trainMethodBox.Items.AddRange(new object[] {
            "Specific",
            "Back",
            "Resilient",
            "Genetic"});
            this.trainMethodBox.Location = new System.Drawing.Point(12, 387);
            this.trainMethodBox.Name = "trainMethodBox";
            this.trainMethodBox.Size = new System.Drawing.Size(127, 21);
            this.trainMethodBox.TabIndex = 2;
            this.trainMethodBox.SelectedIndexChanged += new System.EventHandler(this.trainMethodBox_SelectedIndexChanged);
            // 
            // chart3
            // 
            chartArea5.Name = "ChartArea2";
            this.chart3.ChartAreas.Add(chartArea5);
            this.chart3.Location = new System.Drawing.Point(142, 290);
            this.chart3.Name = "chart3";
            series9.ChartArea = "ChartArea2";
            series9.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series9.Name = "Series1";
            series9.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            series10.ChartArea = "ChartArea2";
            series10.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series10.Name = "Series2";
            series10.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            this.chart3.Series.Add(series9);
            this.chart3.Series.Add(series10);
            this.chart3.Size = new System.Drawing.Size(506, 416);
            this.chart3.TabIndex = 1;
            this.chart3.Text = "chart2";
            // 
            // txtPopulation
            // 
            this.txtPopulation.Location = new System.Drawing.Point(12, 427);
            this.txtPopulation.Name = "txtPopulation";
            this.txtPopulation.Size = new System.Drawing.Size(127, 20);
            this.txtPopulation.TabIndex = 3;
            this.txtPopulation.Text = "50";
            this.txtPopulation.TextChanged += new System.EventHandler(this.txtPopulation_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(9, 411);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(102, 13);
            this.label10.TabIndex = 4;
            this.label10.Text = "Размер популяции";
            // 
            // txtMutation
            // 
            this.txtMutation.Location = new System.Drawing.Point(12, 466);
            this.txtMutation.Name = "txtMutation";
            this.txtMutation.Size = new System.Drawing.Size(127, 20);
            this.txtMutation.TabIndex = 3;
            this.txtMutation.Text = "20";
            this.txtMutation.TextChanged += new System.EventHandler(this.txtMutation_TextChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(9, 450);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(64, 13);
            this.label11.TabIndex = 4;
            this.label11.Text = "Мутации, %";
            // 
            // txtMate
            // 
            this.txtMate.Location = new System.Drawing.Point(12, 505);
            this.txtMate.Name = "txtMate";
            this.txtMate.Size = new System.Drawing.Size(127, 20);
            this.txtMate.TabIndex = 3;
            this.txtMate.Text = "40";
            this.txtMate.TextChanged += new System.EventHandler(this.txtMate_TextChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(9, 489);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(91, 13);
            this.label12.TabIndex = 4;
            this.label12.Text = "Скрещивания, %";
            // 
            // chart1
            // 
            chartArea6.Name = "ChartArea2";
            this.chart1.ChartAreas.Add(chartArea6);
            this.chart1.Location = new System.Drawing.Point(654, 290);
            this.chart1.Name = "chart1";
            series11.ChartArea = "ChartArea2";
            series11.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series11.Name = "Series1";
            series11.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            series12.ChartArea = "ChartArea2";
            series12.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series12.Name = "Series2";
            series12.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            this.chart1.Series.Add(series11);
            this.chart1.Series.Add(series12);
            this.chart1.Size = new System.Drawing.Size(511, 416);
            this.chart1.TabIndex = 1;
            this.chart1.Text = "chart2";
            // 
            // txtMax
            // 
            this.txtMax.Location = new System.Drawing.Point(48, 662);
            this.txtMax.Name = "txtMax";
            this.txtMax.Size = new System.Drawing.Size(88, 20);
            this.txtMax.TabIndex = 3;
            this.txtMax.Text = "1,6";
            this.txtMax.TextChanged += new System.EventHandler(this.txtMax_TextChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(12, 665);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(30, 13);
            this.label13.TabIndex = 4;
            this.label13.Text = "Max:";
            // 
            // txtMin
            // 
            this.txtMin.Location = new System.Drawing.Point(48, 688);
            this.txtMin.Name = "txtMin";
            this.txtMin.Size = new System.Drawing.Size(88, 20);
            this.txtMin.TabIndex = 3;
            this.txtMin.Text = "1,0";
            this.txtMin.TextChanged += new System.EventHandler(this.txtMin_TextChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(12, 691);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(27, 13);
            this.label14.TabIndex = 4;
            this.label14.Text = "Min:";
            // 
            // sourceBox
            // 
            this.sourceBox.FormattingEnabled = true;
            this.sourceBox.Items.AddRange(new object[] {
            "Временной ряд",
            "Квантовый вр. ряд",
            "Квантовый ряд",
            "Направление"});
            this.sourceBox.Location = new System.Drawing.Point(12, 54);
            this.sourceBox.Name = "sourceBox";
            this.sourceBox.Size = new System.Drawing.Size(127, 21);
            this.sourceBox.TabIndex = 2;
            this.sourceBox.SelectedIndexChanged += new System.EventHandler(this.sourceBox_SelectedIndexChanged);
            // 
            // txtQuant
            // 
            this.txtQuant.Location = new System.Drawing.Point(12, 136);
            this.txtQuant.Name = "txtQuant";
            this.txtQuant.Size = new System.Drawing.Size(127, 20);
            this.txtQuant.TabIndex = 3;
            this.txtQuant.Text = "0,005";
            this.txtQuant.TextChanged += new System.EventHandler(this.txtQuant_TextChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(9, 120);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(84, 13);
            this.label15.TabIndex = 4;
            this.label15.Text = "Размер кванта";
            // 
            // txtStartInd
            // 
            this.txtStartInd.Location = new System.Drawing.Point(12, 175);
            this.txtStartInd.Name = "txtStartInd";
            this.txtStartInd.Size = new System.Drawing.Size(127, 20);
            this.txtStartInd.TabIndex = 3;
            this.txtStartInd.Text = "0";
            this.txtStartInd.TextChanged += new System.EventHandler(this.txtStartInd_TextChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(12, 38);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(48, 13);
            this.label16.TabIndex = 4;
            this.label16.Text = "Данные";
            // 
            // txtPredictSinceInd
            // 
            this.txtPredictSinceInd.Location = new System.Drawing.Point(9, 599);
            this.txtPredictSinceInd.Name = "txtPredictSinceInd";
            this.txtPredictSinceInd.Size = new System.Drawing.Size(127, 20);
            this.txtPredictSinceInd.TabIndex = 3;
            this.txtPredictSinceInd.Text = "0";
            this.txtPredictSinceInd.TextChanged += new System.EventHandler(this.txtPredictSinceInd_TextChanged);
            // 
            // nwBox
            // 
            this.nwBox.FormattingEnabled = true;
            this.nwBox.Items.AddRange(new object[] {
            "FNN",
            "RNN"});
            this.nwBox.Location = new System.Drawing.Point(12, 292);
            this.nwBox.Name = "nwBox";
            this.nwBox.Size = new System.Drawing.Size(127, 21);
            this.nwBox.TabIndex = 2;
            this.nwBox.SelectedIndexChanged += new System.EventHandler(this.nwBox_SelectedIndexChanged);
            // 
            // txtPredictCount
            // 
            this.txtPredictCount.Location = new System.Drawing.Point(9, 638);
            this.txtPredictCount.Name = "txtPredictCount";
            this.txtPredictCount.Size = new System.Drawing.Size(127, 20);
            this.txtPredictCount.TabIndex = 3;
            this.txtPredictCount.Text = "56";
            this.txtPredictCount.TextChanged += new System.EventHandler(this.txtPredictCount_TextChanged);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(6, 622);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(75, 13);
            this.label18.TabIndex = 4;
            this.label18.Text = "В количестве";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(12, 80);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(58, 13);
            this.label17.TabIndex = 4;
            this.label17.Text = "Исходные";
            // 
            // wantedInputDataBox
            // 
            this.wantedInputDataBox.FormattingEnabled = true;
            this.wantedInputDataBox.Items.AddRange(new object[] {
            "Цена",
            "Разница",
            "Направление"});
            this.wantedInputDataBox.Location = new System.Drawing.Point(12, 96);
            this.wantedInputDataBox.Name = "wantedInputDataBox";
            this.wantedInputDataBox.Size = new System.Drawing.Size(61, 21);
            this.wantedInputDataBox.TabIndex = 2;
            this.wantedInputDataBox.SelectedIndexChanged += new System.EventHandler(this.wantedInputDataBox_SelectedIndexChanged);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(78, 80);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(53, 13);
            this.label19.TabIndex = 4;
            this.label19.Text = "Целевые";
            // 
            // wantedOutputDataBox
            // 
            this.wantedOutputDataBox.FormattingEnabled = true;
            this.wantedOutputDataBox.Items.AddRange(new object[] {
            "Цена",
            "Разница",
            "Направление"});
            this.wantedOutputDataBox.Location = new System.Drawing.Point(78, 96);
            this.wantedOutputDataBox.Name = "wantedOutputDataBox";
            this.wantedOutputDataBox.Size = new System.Drawing.Size(61, 21);
            this.wantedOutputDataBox.TabIndex = 2;
            this.wantedOutputDataBox.SelectedIndexChanged += new System.EventHandler(this.wantedOutputDataBox_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1183, 715);
            this.Controls.Add(this.nwBox);
            this.Controls.Add(this.wantedOutputDataBox);
            this.Controls.Add(this.wantedInputDataBox);
            this.Controls.Add(this.sourceBox);
            this.Controls.Add(this.trainMethodBox);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtMate);
            this.Controls.Add(this.txtMin);
            this.Controls.Add(this.txtMax);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtMutation);
            this.Controls.Add(this.txtLayers);
            this.Controls.Add(this.txtPredictCount);
            this.Controls.Add(this.txtPopulation);
            this.Controls.Add(this.txtPredictSinceInd);
            this.Controls.Add(this.txtStartInd);
            this.Controls.Add(this.txtQuant);
            this.Controls.Add(this.txtWindowLength);
            this.Controls.Add(this.txtLearnLength);
            this.Controls.Add(this.dtPredictSince);
            this.Controls.Add(this.dtStartDate);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.chart3);
            this.Controls.Add(this.chart2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DateTimePicker dtStartDate;
        private System.Windows.Forms.TextBox txtLearnLength;
        private System.Windows.Forms.TextBox txtWindowLength;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtLayers;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart2;
        private System.Windows.Forms.DateTimePicker dtPredictSince;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox trainMethodBox;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart3;
        private System.Windows.Forms.TextBox txtPopulation;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtMutation;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtMate;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.TextBox txtMax;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtMin;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox sourceBox;
        private System.Windows.Forms.TextBox txtQuant;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtStartInd;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtPredictSinceInd;
        private System.Windows.Forms.ComboBox nwBox;
        private System.Windows.Forms.TextBox txtPredictCount;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ComboBox wantedInputDataBox;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.ComboBox wantedOutputDataBox;
    }
}

