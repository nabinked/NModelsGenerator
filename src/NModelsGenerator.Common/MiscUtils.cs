using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace NModelsGenerator.Common
{
    public class MiscUtils
    {
        public static Config GetConfig(string configFileFullPath)
        {
            if (configFileFullPath == null) return null;
            if (!File.Exists(configFileFullPath))
            {
                return null;
            }

            using (var reader = new StreamReader(configFileFullPath))
            {
                var d = reader.ReadToEnd();

                var config = JsonConvert.DeserializeObject<Config>(d);
                if (config != null)
                {
                    return config;

                }
            }

            return null;
        }
    }
}
