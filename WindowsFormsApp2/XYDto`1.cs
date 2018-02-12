using System.Linq;

namespace WindowsFormsApp2
{
    public class ArgValueDto<TArg>:IndValueDto
    {
        public TArg Arg { get; set; }
        public override object GetArg => Arg;
        public override IndValueDto Clone()
        {
            return new ArgValueDto<TArg>() { Ind = this.Ind, Val = this.Val.ToArray(), Arg = this.Arg};
        }
    }

    public class IndValueDto
    {
        public int Ind { get; set; }
        public double[] Val { get; set; }
        public virtual object GetArg => Ind;

        public virtual IndValueDto Clone()
        {
            return new IndValueDto() {Ind = this.Ind, Val = this.Val.ToArray()};
        }
    }
}