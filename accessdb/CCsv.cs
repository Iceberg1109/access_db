using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace accessdb
{
    class CCsv
    {
        public class Table
        {
            public string ID_Number { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string MobilePhoneNumber { get; set; }
            public string Email { get; set; }
            public string Street { get; set; }
            public string City { get; set; }
            public string APP { get; set; }
            public string PostalCode { get; set; }
            public string AptNumber { get; set; }
            public string CivicNumber { get; set; }
        }
        public List<T> ReadCsv<T>(string path)
        {
            List<T> list = new List<T>();
            try
            {
                using (var textReader = File.OpenText(path))
                {
                    var csv = new CsvReader(textReader);
                    while (csv.Read())
                    {
                        var record = csv.GetRecord<T>();
                        list.Add(record);
                    }
                    textReader.Close();
                }
            }
            catch (Exception)
            {
            }
            return list;
        }
        public void AppendCsv<T>(T val, string path)
        {
            List<T> list = new List<T>();
            list.AddRange(ReadCsv<T>(path));
            list.Add(val);
            SaveCsv<T>(list, path);
        }
        public void SaveCsv<T>(List<T> list, string path)
        {
            using (StreamWriter sw = new StreamWriter(path))
            using (CsvWriter cw = new CsvWriter(sw))
            {
                cw.WriteHeader<T>();
                cw.NextRecord();
                foreach (T item in list)
                {
                    cw.WriteRecord<T>(item);
                    cw.NextRecord();
                }
            }
        }
    }
}
