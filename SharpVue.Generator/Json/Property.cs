namespace SharpVue.Generator.Json
{
    public class Property
    {
        public string? Name { get; set; }
        public string? ReturnType { get; set; }
        public bool Getter { get; set; }
        public bool Setter { get; set; }
        public string? Summary { get; set; }
        public string? Remarks { get; set; }
        public string? Returns { get; set; }
    }
}
