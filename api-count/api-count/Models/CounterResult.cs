namespace api_count.Models
{
    public class CounterResult
    {
        public int CurrentValue { get; set; }
        public string Locale { get; set; }
        public string Kernel { get; set; }
        public string TargetFwk { get; set; }
        public string FixeMessage { get; set; }
        public object VariableMessage { get; set; }
    }
}
