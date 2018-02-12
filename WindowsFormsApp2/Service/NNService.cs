using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Encog.Engine.Network.Activation;
using Encog.ML.Data.Basic;
using Encog.ML.Train;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Layers;
using Encog.Neural.Networks.Training;
using Encog.Neural.Networks.Training.Genetic;
using Encog.Neural.Networks.Training.Lma;
using Encog.Neural.Networks.Training.Propagation.Back;
using Encog.Neural.Networks.Training.Propagation.Resilient;
using SharpML.Recurrent.Activations;
using SharpML.Recurrent.DataStructs;
using SharpML.Recurrent.Loss;
using SharpML.Recurrent.Models;
using SharpML.Recurrent.Networks;
using SharpML.Recurrent.Trainer;
using SharpML.Recurrent.Util;

namespace WindowsFormsApp2.Service
{
    partial class NNService
    {

        private double _maxInput = 1.6, _minInput = 1.0;
        private double _maxOutput = 1.6, _minOutput = 1.0;
        private List<IndValueDto> _archive = new List<IndValueDto>();
        private List<IndValueDto> _quants = new List<IndValueDto>();
        private List<IndValueDto> _quantsDate = new List<IndValueDto>();
        private List<IndValueDto> _quantsDirection = new List<IndValueDto>();

        private double _quant = 0.005;

        private DateTime _trainStartDate;
        private int _trainStartInd;
        private DateTime _predictStartDate;
        private int _predictStartInd;

        private NetworkType _wantedNnType = NetworkType.FNN;
        private SourceType _wantedSource = SourceType.Raw;
        private ResultType _wantedInput = ResultType.Pure;
        private ResultType _wantedOutput = ResultType.Pure;
        private TrainMethod _trainMethod = TrainMethod.Resilent;

        private int _population = 500;
        private int _matePercent = 40;
        private int _mutationPercent = 20;

        private INetwork _rnetwork;
        private BasicNetwork _network;

        private int[] _layers;

        private int _learnLength = 200;//2920;
        private int _windowSize = 30;//56;
        private int _predictLength = 30;//56;
        private bool _stop;
        private int _epoch;

        private double[] NormalizeInput(double[] source)
        {
            var dif = _maxInput - _minInput;
            return source.Select(s => 2 * (s - _minInput) / dif - 1).ToArray();
        }
        private double[] DenormalizeInput(double[] source)
        {
            var dif = _maxOutput - _minInput;
            return source.Select(s => (s + 1) / 2 * dif + _minOutput).ToArray();
        }
        private double[] NormalizeOutput(double[] source)
        {
            var dif = _maxOutput - _minOutput;
            return source.Select(s => 2 * (s - _minOutput) / dif - 1).ToArray();
        }
        private double[] DenormalizeOutput(double[] source)
        {
            var dif = _maxOutput - _minOutput;
            return source.Select(s => (s + 1) / 2 * dif + _minOutput).ToArray();
        }
        private IndValueDto[] GetSource()
        {
            switch (_wantedSource)
            {
                case SourceType.Raw: return _archive.ToArray();
                case SourceType.QuantDated: return _quantsDate.ToArray();
                case SourceType.Quant: return _quants.ToArray();
                case SourceType.QuantDirection: return _quantsDirection.ToArray();
            }

            MessageBox.Show("WrongSourceType");
            return new IndValueDto[]{};
        }

        private int TrainStartIndex()
        {
            switch (_wantedSource)
            {
                case SourceType.Raw: return _archive.FirstOrDefault(d => (DateTime)d.GetArg >= _trainStartDate)?.Ind ?? 0;
                case SourceType.QuantDated: return _quantsDate.FirstOrDefault(d => (DateTime)d.GetArg >= _trainStartDate)?.Ind ?? 0;
                case SourceType.Quant: return _trainStartInd;
                case SourceType.QuantDirection: return _trainStartInd;
            }

            MessageBox.Show("WrongSourceType");
            return 0;
        }

        private int PredictStartIndex()
        {
            int ind;
            switch (_wantedSource)
            {
                case SourceType.Raw: ind = _archive.FirstOrDefault(d => (DateTime)d.GetArg >= _predictStartDate)?.Ind ?? 0; return ind < _windowSize ? _windowSize : ind;
                case SourceType.QuantDated: ind = _quantsDate.FirstOrDefault(d => (DateTime)d.GetArg >= _predictStartDate)?.Ind ?? 0; return ind < _windowSize ? _windowSize : ind;
                case SourceType.Quant: return _predictStartInd < _windowSize ? _windowSize : _predictStartInd;
                case SourceType.QuantDirection: return _predictStartInd < _windowSize ? _windowSize : _predictStartInd;
            }

            MessageBox.Show("WrongSourceType");
            return 0;
        }

        private IndValueDto[] PredictPeriodByOnlyFirstNextVal(int indexSince, int periodLength)
        {
            var result = new List<IndValueDto>();

            var sourceArray = GetSource();
            if(_network==null && _rnetwork == null) return result.ToArray();
            for (int i = 0; i < periodLength; i++)
            {
                var source = new double[_windowSize];
                for (int j = 0; j < _windowSize; j++)
                {
                    source[j] = sourceArray[indexSince - _windowSize + i + j].Val[(byte)_wantedInput];
                }

                var output = ProcessByNetwork(source);
                if (output.Any())
                {
                    var clone = sourceArray[indexSince + i].Clone();
                    clone.Val[(int)_wantedOutput] = DenormalizeOutput(output)[0];
                    result.Add(clone);
                }
            }

            return result.ToArray();
        }

        private IndValueDto[] PredictWholePeriodByNn(int indexSince)
        {
            var source = new double[_windowSize];
            var sourceArray = GetSource();
            for (int j = 0; j < _windowSize; j++)
            {
                source[j] = sourceArray[indexSince - _windowSize + j].Val[(byte)_wantedInput];
            }

            var output = ProcessByNetwork(source);
            if (output.Any())
            {
                var denormOutput = DenormalizeOutput(output);
                return denormOutput.Select((o, i) =>
                {
                    var clone = sourceArray[indexSince + i].Clone();
                    clone.Val[(int)_wantedOutput] = o;
                    return clone;
                }).ToArray();
            }
            return new IndValueDto[]{};
        }

        private double[] ProcessByNetwork(double[] source)
        {
            var normSource = NormalizeInput(source);

            if (_network != null)
            {
                var output = new double[_layers.Last()];
                _network.Compute(normSource, output);

                return output;
            }

            if (_rnetwork != null)
            {
                Matrix output = new Matrix(_layers.Last());
                Matrix input = new Matrix(normSource);
                Graph g = new Graph(true);
                output = _rnetwork.Activate(input, g);
                return output.W;
            }

            //MessageBox.Show("NN not created");
            return new double[0];
        }

        public void UploadData(string fileName)
        {
            _archive.Clear();
            using (System.IO.StreamReader file = new System.IO.StreamReader(fileName, Encoding.UTF8))
            {
                string line;
                //file.ReadLine();
                var ind = 0;
                while ((line = file.ReadLine()) != null)
                {
                    if (line.Length > 0)
                    {
                        var cols = line.Split(';');
                        for (var i = 0; i < cols.Count(); i++)
                        {
                            var col = cols[i];
                            if (col.Length > 0)
                            {
                                if (col[0] == '"' && col[col.Length - 1] == '"')
                                {
                                    cols[i] = col.Substring(1, col.Length - 2);
                                }
                            }
                        }

                        var val = double.Parse(cols[1].Replace(".", ","), new CultureInfo("ru-RU"));

                        var wd = new ArgValueDto<DateTime>
                        {
                            Ind = ind,
                            Arg = DateTime.ParseExact(cols[0], "dd.MM.yyyy HH:mm", null, DateTimeStyles.None),
                            Val = new double[3]
                            {
                                cols[1].Length == 0 ? _archive.Last().Val[0] : val,
                                _archive.Any() ? val - _archive.Last().Val[0] : 0,
                                _archive.Any() ? (val > _archive.Last().Val[0]) ? 1 : (val < _archive.Last().Val[0]) ? -1 : 0 : 0
                            }
                        };

                        _archive.Add(wd);
                        ind++;
                    }
                }
            }

            _archive = _archive.OrderBy(d => d.GetArg).ToList();

            _minInput = _archive.Min(a => a.Val[(byte)_wantedInput]);
            _maxInput = _archive.Max(a => a.Val[(byte)_wantedInput]);
            _minOutput = _archive.Min(a => a.Val[(byte)_wantedOutput]);
            _maxOutput = _archive.Max(a => a.Val[(byte)_wantedOutput]);

            _trainStartDate = (DateTime)_archive.Min(e => e.GetArg);
            _predictStartDate = _trainStartDate.AddDays(2);

            RecalcQuants();
        }

        public void CreateNetwork(int[] layersOrNeuronsLayersOutput)
        {
            _layers = layersOrNeuronsLayersOutput;
            if (_wantedNnType == NetworkType.FNN)
            {
                // create neural network
                _network = new BasicNetwork();

                //_network.AddLayer(new BasicLayer(null, true, _windowSize));
                //foreach (var neuronCount in _layers)
                //{
                //    _network.AddLayer(new BasicLayer(new ActivationTANH(), false, neuronCount));
                //}

                var input = new BasicLayer(null, true, _windowSize);
                var input1 = new BasicLayer(new ActivationTANH(), false, _layers[0]);
                var hidden = new BasicLayer(new ActivationTANH(), false, _layers[1]);
                var output = new BasicLayer(new ActivationTANH(), false, _layers[2]);

                input1.ContextFedBy = hidden;

                _network.AddLayer(input);
                _network.AddLayer(input1);
                _network.AddLayer(hidden);
                _network.AddLayer(output);

                _network.Structure.FinalizeStructure();
                _network.Reset();
                _rnetwork = null;
            }

            if (_wantedNnType == NetworkType.RNN)
            {
                _rnetwork = NetworkBuilder.MakeLstm(_windowSize,
                    _layers[0],
                    _layers[1],
                    _layers[2],
                    new TanhUnit(),
                    0.08, new Random());
                _rnetwork.ResetState();
                _network = null;
            }
        }

        public IndValueDto[] GetKnownSourceData()
        {
            return GetSource().Skip(TrainStartIndex()).Take(_learnLength + _windowSize).ToArray();
        }

        public IndValueDto[] GetTrainSourceData()
        {
            return PredictPeriodByOnlyFirstNextVal(TrainStartIndex() + _windowSize, _learnLength);
        }

        public IndValueDto[] GetKnownPerionOneByOneData()
        {
            return GetSource().Skip(PredictStartIndex()).Take(_predictLength).ToArray();
        }

        public IndValueDto[] GetTrainPeriodOneByOneData()
        {
            return PredictPeriodByOnlyFirstNextVal(PredictStartIndex(), _predictLength);
        }

        public IndValueDto[] GetKnownNnResultData()
        {
            return GetSource().Skip(PredictStartIndex()).Take(_layers?.Last() ?? 0).ToArray();
        }

        public IndValueDto[] GetTrainNnResultData()
        {
            return PredictWholePeriodByNn(PredictStartIndex());
        }

        public void StopTrain()
        {
            _stop = true;
            _epoch = 0;
        }

        public void StartTrain(Action<double, int> trainCallback = null, int reportEach = 5)
        {
            // initialize input and output values
            var inputs = new double[_learnLength][];
            var outputs = new double[_learnLength][];
            var i0 = TrainStartIndex();
            var sourceArray = GetSource().ToArray();
            var window = new Queue<double>(sourceArray.Skip(i0).Take(_windowSize).Select(d => d.Val[(byte)_wantedInput]));
            var trainingDataSet = new List<DataSequence>();

            for (var i = 0; i < _learnLength; i++)
            {
                // берем _windowSize предыдущих значений для _learnLength значений начиная с выбранной даты
                var innerArray = window.ToArray();
                inputs[i] = NormalizeInput(innerArray);

                window.Dequeue();
                window.Enqueue(sourceArray[i0 + i + _windowSize].Val[(byte)_wantedInput]);

                var ouputArray = sourceArray.Skip(i + _windowSize).Take(_layers.Last()).Select(d => d.Val[(byte)_wantedOutput]).ToArray();
                outputs[i] = NormalizeOutput(ouputArray);

                trainingDataSet.Add(new DataSequence(new List<DataStep>() { new DataStep(inputs[i], outputs[i]) }));
            }

            if (_network != null)
            {
                var trainingSet = new BasicMLDataSet(inputs, outputs);
                IMLTrain teacher =
                    _trainMethod == TrainMethod.Specific ? (IMLTrain)new LevenbergMarquardtTraining(_network, trainingSet) :
                    _trainMethod == TrainMethod.BackProp ? (IMLTrain)new Backpropagation(_network, trainingSet) :
                    _trainMethod == TrainMethod.Resilent ? (IMLTrain)new ResilientPropagation(_network, trainingSet) :
                    _trainMethod == TrainMethod.Genetic ? (IMLTrain)new NeuralGeneticAlgorithm(
                        _network,
                        new Encog.MathUtil.Randomize.NguyenWidrowRandomizer(),
                        new TrainingSetScore(trainingSet),
                        _population,
                        _mutationPercent / 100.0,
                        _matePercent / 100.0) : null;
                if (teacher == null) return;

                _stop = false;
                for (int i = 1; !_stop; i++)
                {
                    teacher.Iteration();
                    if (teacher is NeuralGeneticAlgorithm)
                        _network = (teacher as NeuralGeneticAlgorithm).Genetic.Population.Best.Organism as BasicNetwork;
                    if (i % reportEach == 0 && trainCallback != null)
                    {
                        trainCallback(teacher.Error, i);
                    }
                }
            }

            if (_rnetwork != null)
            {
                _epoch = int.MaxValue;
                Trainer.train<NeuralNetwork>(ref _epoch, 0.001, _rnetwork, new DataSet()
                {
                    Training = trainingDataSet,
                    InputDimension = _windowSize,
                    LossReporting = new LossSumOfSquares(),
                    LossTraining = new LossSumOfSquares(),
                    OutputDimension = _layers[2],
                    Testing = trainingDataSet,
                    Validation = trainingDataSet
                }, reportEach, new Random(), trainCallback);
            }
        }

        public void RecalcQuants()
        {
            if (!_archive.Any()) return;

            var previous = _archive.First().Val[(int)ResultType.Pure];
            //if (double.TryParse(txtQuant.Text.Replace(".", ","), NumberStyles.AllowDecimalPoint, new CultureInfo("ru-RU"), out var quant))
            if(_quant > 0)
            {
                _quantsDate.Clear();
                _quants.Clear();
                _quantsDirection.Clear();

                var quantInd = 0;
                var previousInd = 0;

                _quantsDate.Add(new ArgValueDto<DateTime> { Arg = (DateTime)_archive.First().GetArg, Val = new double[3] { previous, 0, 0 }, Ind = 0 });
                _quants.Add(new IndValueDto() {Ind = quantInd, Val = new double[3] {previous, 0, 0}});
                _quantsDirection.Add(new IndValueDto() { Ind = quantInd, Val = new double[3] { 0, 0, 0 } });
                quantInd++;

                for (int index = 0; index < _archive.Count; index++)
                {
                    var item = _archive[index];
                    var dif = Math.Abs(item.Val[(int)ResultType.Pure] - previous);
                    if (dif > _quant)
                    {
                        var sign = item.Val[(int)ResultType.Pure] < previous ? -1 : 1;
                        var steps = (int)(dif / _quant);
                        for (int i = 1; i <= steps; i++)
                        {
                            var newQuantVal = previous + _quant * sign * i;
                            _quants.Add(new IndValueDto()
                            {
                                Ind = quantInd,
                                Val = new double[3]
                                {
                                    newQuantVal,
                                    newQuantVal - _quants.Last().Val[(int) ResultType.Pure],
                                    newQuantVal > _quants.Last().Val[(int) ResultType.Pure] ? 1 : -1
                                }
                            });
                            _quantsDirection.Add(new IndValueDto()
                            {
                                Ind = quantInd,
                                Val = new double[]
                                {
                                    sign,
                                    sign - _quantsDirection.Last().Val[(int) ResultType.Pure],
                                    sign > _quantsDirection.Last().Val[(int) ResultType.Pure] ? 1 : -1
                                }
                            });
                            quantInd++;
                        }
                        var total = _quant * steps * sign;
                        var totalStep = total / (index - previousInd);
                        for (int j = previousInd + 1, c = 1; j <= index; j++, c++)
                        {
                            var val = previous + totalStep * c;
                            _quantsDate.Add(new ArgValueDto<DateTime>
                            {
                                Arg = (DateTime) _archive[j].GetArg,
                                Val = new double[3]
                                {
                                    val, val - _quantsDate.Last().Val[(int) ResultType.Pure],
                                    val > _quantsDate.Last().Val[(int) ResultType.Pure] ? 1 : -1
                                },
                                Ind = j
                            });
                        }
                        previous = previous + total;
                        previousInd = index;
                    }
                }
            }
        }
    }
}
