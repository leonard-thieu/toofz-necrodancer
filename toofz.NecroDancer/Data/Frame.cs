namespace toofz.NecroDancer.Data
{
    public sealed class Frame
    {
        public int InSheet { get; set; }
        public int InAnim { get; set; }
        public string AnimType { get; set; }
        public double OnFraction { get; set; }
        public double OffFraction { get; set; }
        public bool SingleFrame { get; set; }
    }
}
