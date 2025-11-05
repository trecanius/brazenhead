using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace brazenhead
{
    [Serializable]
    internal class FileConfigData : ConfigData
    {
        internal bool ReadFromFile(in ConfigSettings settings, in string path)
        {
            if (!File.Exists(path))
                return false;

            try
            {
                var lines = File.ReadAllLines(path);
                var valueTypeByKey = new Dictionary<string, Type>();
                var keyProperty = typeof(ConfigSetting).GetProperty("Key", BindingFlags.Instance | BindingFlags.NonPublic);
                foreach (var propertyInfo in typeof(ConfigSettings).GetProperties(BindingFlags.Instance | BindingFlags.NonPublic))
                {
                    var configSetting = propertyInfo.GetValue(settings) as ConfigSetting;
                    valueTypeByKey.Add(keyProperty.GetValue(configSetting) as string, propertyInfo.PropertyType.BaseType.GenericTypeArguments.FirstOrDefault());
                }

                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    int j = line.IndexOf('#');
                    if (j >= 0)
                        line = line[..j];
                    line = line.Trim();
                    j = line.IndexOf('=');
                    if (j >= 1)
                    {
                        var key = line[..j].TrimEnd();
                        if (valueTypeByKey.TryGetValue(key, out var type))
                        {
                            var value = line[(j + 1)..].TrimStart();
                            if (type == typeof(bool) && bool.TryParse(value, out var boolValue))
                                BoolValues[key] = boolValue;
                            if (type == typeof(int) && int.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var intValue))
                                IntValues[key] = intValue;
                            if (type == typeof(float) && float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var floatValue))
                                FloatValues[key] = floatValue;
                            else if (type == typeof(string))
                                StringValues[key] = value;
                        }
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return false;
            }
        }

        internal bool WriteToFile(in string path)
        {
            if (!IsDirty)
                return true;

            var sb = new StringBuilder();
            foreach ((var key, var value) in BoolValues)
                sb.AppendLine($"{key}={ValueToString(value)}");
            foreach ((var key, var value) in IntValues)
                sb.AppendLine($"{key}={ValueToString(value)}");
            foreach ((var key, var value) in FloatValues)
                sb.AppendLine($"{key}={ValueToString(value)}");
            foreach ((var key, var value) in StringValues)
                sb.AppendLine($"{key}={ValueToString(value)}");

            try
            {
                File.WriteAllText(path, sb.ToString());
                IsDirty = false;
                return true;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return false;
            }

            static string ValueToString(in object value)
            {
                return value switch
                {
                    bool boolValue => boolValue.ToString(CultureInfo.InvariantCulture).ToLowerInvariant(),
                    int intValue => intValue.ToString(CultureInfo.InvariantCulture),
                    float intValue => intValue.ToString(CultureInfo.InvariantCulture),
                    _ => value.ToString()
                };
            }
        }
    }
}
