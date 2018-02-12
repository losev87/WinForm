namespace WindowsFormsApp2.Service
{
    public enum NetworkType : byte
    {
        FNN = 0,
        RNN = 1
    }

    public enum SourceType : byte
    {
        Raw = 0,
        QuantDated = 1,
        Quant = 2,
        QuantDirection = 3
    }

    public enum TrainMethod : byte
    {
        BackProp = 0,
        Resilent = 1,
        Genetic = 2,
        Specific = 3,
    }

    public enum ResultType : byte
    {
        Pure = 0,
        Difference = 1,
        Direction = 2,
    }
}
