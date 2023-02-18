namespace Application
{
    public class Meta
    {
        public int parsed_files {get; set;} = 0;
        public int parsed_lines {get; set;} = 0;
        public int found_errors {get; set;} = 0;
        public List<int> invalid_files {get; set;} = new List<int>();

    }
}