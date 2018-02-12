using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2.Service
{
    partial class NNService
    {
        public void SetPredictCount(int c)
        {
            _predictLength = c;
        }
        public void SetWindowSize(int s)
        {
            _windowSize = s;
        }
        public void SetPredictStartInd(int i)
        {
            _predictStartInd = i;
        }
        public void SetPredictStartDate(DateTime d)
        {
            _predictStartDate = d;
        }
        public void SetTrainStartInd(int i)
        {
            _trainStartInd = i;
        }
        public void SetTrainStartDate(DateTime d)
        {
            _trainStartDate = d;
        }

        public void SetTrainLength(int l)
        {
            _learnLength = l;
        }
        public void SetQuant(double q)
        {
            _quant = q;
        }
        public void SetWantedSource(SourceType s)
        {
            _wantedSource = s;
            if (s == SourceType.QuantDirection)
            {
                _minInput = _minOutput = -1;
                _maxInput = _maxOutput = 1;
            }
            else
            {
                _minInput = _archive.Min(a => a.Val[(int)_wantedInput]);
                _maxInput = _archive.Max(a => a.Val[(int)_wantedInput]);
                _minOutput = _archive.Min(a => a.Val[(int)_wantedOutput]);
                _maxOutput = _archive.Max(a => a.Val[(int)_wantedOutput]);
            }
        }
        public void SetWantedNnType(NetworkType t)
        {
            _wantedNnType = t;
        }
        public void SetTrainMethod(TrainMethod t)
        {
            _trainMethod = t;
        }
        public void SetPopulation(int p)
        {
            _population = p;
        }
        public void SetMatationPercent(int p)
        {
            _mutationPercent = p;
        }
        public void SetMatePercent(int p)
        {
            _matePercent = p;
        }

        public void SetWantedInputResultType(ResultType t)
        {
            _wantedInput = t;
            _minInput = _archive.Min(a => a.Val[(int)_wantedInput]);
            _maxInput = _archive.Max(a => a.Val[(int)_wantedInput]);
        }
        public void SetWantedOutputResultType(ResultType t)
        {
            _wantedOutput = t;
            _minOutput = _archive.Min(a => a.Val[(int)_wantedOutput]);
            _maxOutput = _archive.Max(a => a.Val[(int)_wantedOutput]);
        }
        public double GetMax() => _maxOutput;
        public double GetMin() => _minOutput;
        public DateTime GetTrainStartDate() => _trainStartDate.Date;
        public DateTime GetPredictStartDate() => _predictStartDate.Date;
        public int GetTrainLength() => _learnLength;
        public int GetWindowSize() => _windowSize;
        public int GetPredictStartInd() => _predictStartInd;
        public double GetQuant() => _quant;
        public int GetTrainStartInd() => _trainStartInd;
        public int GetPopulation() => _population;
        public int GetMutationPercent() => _mutationPercent;
        public int GetMatePercent() => _matePercent;
        public int GetPredictLength() => _predictLength;
        public ResultType GetWantedOutput() => _wantedOutput;
    }
}
