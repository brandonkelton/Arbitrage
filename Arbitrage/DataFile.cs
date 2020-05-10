using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Arbitrage
{
    public class DataFile
    {
        private readonly string fileName;

        public readonly List<ExchangeEntry> Entries = new List<ExchangeEntry>();

        public DataFile(string fileName)
        {
            this.fileName = fileName;
        }

        public void LoadFile()
        {
            Entries.Clear();

            var lines = File.ReadAllLines(fileName);
            var lineEnumerator = lines.GetEnumerator();
            var isReading = false;

            while (lineEnumerator.MoveNext())
            {
                if (isReading)
                {
                    try { Entries.Add(new ExchangeEntry(lineEnumerator.Current as string)); }
                    catch (Exception) { }
                    
                }

                if ((lineEnumerator.Current as string).StartsWith("****"))
                {
                    isReading = true;
                }
            }
        }
    }
}
