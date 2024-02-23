using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace new_pr_2._24
{
    class Json
    {
            public static T Des<T>(string filename)
            {
                string json = File.ReadAllText(filename);
                T types = JsonConvert.DeserializeObject<T>(json);
                return types;
            }
            public static void Ser<T>(T types, string filename)
            {
                string json = JsonConvert.SerializeObject(types);
                File.WriteAllText(filename, json);
            }
    }
}


