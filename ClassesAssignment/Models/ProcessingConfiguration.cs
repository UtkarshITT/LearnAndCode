namespace SolidDataProcessor.Models
{
    public class ProcessingConfiguration
    {
        public string DateFormat { get; set; } = "yyyy-MM-dd";
        public int BatchSize { get; set; } = 100;
        public bool ShouldValidate { get; set; } = true;
        public bool ShouldTransform { get; set; } = true;
    }
}
