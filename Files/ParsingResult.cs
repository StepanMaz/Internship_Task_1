using Entities;

namespace Files
{
    public class ParsingResult
    {
        public bool isInvalid => failed_lines != 0;

        public List<PaymentDetails> details;
        public int lines;
        public int failed_lines;
        public string file_path;
    }
}