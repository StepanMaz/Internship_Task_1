using Entities;
using System.Text.RegularExpressions;
using System.Collections.Concurrent;
using System.Globalization;

namespace Files
{
    public class TXTFileReader : FileReader
    {
        Regex regex = new Regex(@"[“""]{1}[\w\s,\\]+[”""]{1}|[\w\-\.]+");

        public override async Task<ParsingResult> ReadData(string path, CancellationToken cancellationToken)
        {
            ParsingResult pr = new ParsingResult();
            pr.details = new List<PaymentDetails>();
            pr.file_path = path;

            ConcurrentQueue<string> queue = new ();
            StreamReader reader = new StreamReader(path);

            await QueueLinesForProcessing(queue, reader);

            while(!cancellationToken.IsCancellationRequested && queue.Count != 0) 
            {
                string line;
                queue.TryDequeue(out line);

                try{
                    var pd = ParseLine(line);

                    if(!ValidatePaymentDetails(pd))
                    {
                        pr.failed_lines++;
                    }
                    else
                    {
                        pr.details.Add(pd);
                    }
                }
                catch {
                    pr.failed_lines++;
                }

                pr.lines++;
            }

            return pr;
        }

        private async Task<ConcurrentQueue<string>> QueueLinesForProcessing(ConcurrentQueue<string> queue, StreamReader reader)
        {
            while(!reader.EndOfStream)
            {
                queue.Enqueue(await reader.ReadLineAsync());
            }
            return queue;
        }

        private PaymentDetails ParseLine(string line)
        {
            string[] array = regex.Matches(line).Select(t => t.Value).ToArray();

            if(array.Length != 7)
            {
                throw new Exception("Invalid line format");
            }

            return new PaymentDetails() {
                First_name = array[0],
                Last_name = array[1],
                Address = array[2].Trim('"', '”', '“'),
                Payment = decimal.Parse(array[3], CultureInfo.InvariantCulture),
                Date = DateTime.ParseExact(array[4], "yyyy-dd-MM", CultureInfo.InvariantCulture),
                Account_number = long.Parse(array[5]),
                Service = array[6]
            };
        }
    }
}