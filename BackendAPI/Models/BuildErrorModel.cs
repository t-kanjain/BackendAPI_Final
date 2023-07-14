namespace BackendAPI.Models
{
    public class BuildErrorModel
    {
        public string inputFile { get; set; }
        public List<Error> ErrorList { get; set; }
    }

    public class Error
    {
        public string LineNumber { get; set; }
        public string ErrorMessage { get; set; }
        public string PotentialFix { get; set; }
    }
}
