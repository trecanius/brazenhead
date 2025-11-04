using System;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEngine;

namespace brazenhead
{
    [Serializable]
    internal class FileConfigData : ConfigData
    {
        private ValueMap _valueByKey = new();
        private bool _isDirty;

        internal override bool TryGetValue<T>(in string key, out T value)
        {
            value = default;
            return _valueByKey.TryGetValue(key, out string valueString) && StringToValue(valueString, out value);
        }

        internal override T GetValue<T>(in string key, in T defaultValue = default)
        {
            return _valueByKey.TryGetValue(key, out string valueString) && StringToValue<T>(valueString, out var value) ? value : defaultValue;
        }

        internal override void SetValue<T>(in string key, in T value)
        {
            _isDirty = true;
            _valueByKey[key] = ValueToString(value);
        }

        internal bool ReadFromFile(in string path)
        {
            if (!File.Exists(path))
                return false;

            try
            {
                var lines = File.ReadAllLines(path);
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
                        var value = line[(j + 1)..].TrimStart();
                        _valueByKey.Add(key, value);
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
            if (!_isDirty)
                return true;

            var sb = new StringBuilder();
            foreach ((var key, var value) in _valueByKey)
                sb.AppendLine($"{key}={ValueToString(value)}");

            try
            {
                File.WriteAllText(path, sb.ToString());
                _isDirty = false;
                return true;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return false;
            }
        }

        private bool StringToValue<T>(in string valueString, out T value)
        {
            value = default;
            bool success = false;
            if (typeof(T) == typeof(bool))
            {
                success = bool.TryParse(valueString, out bool boolValue);
                value = success ? (T)(object)boolValue : default;
            }
            else if (typeof(T) == typeof(int))
            {
                success = int.TryParse(valueString, NumberStyles.Any, CultureInfo.InvariantCulture, out var intValue);
                value = success ? (T)(object)intValue : default;
            }
            else if (typeof(T) == typeof(float))
            {
                success = float.TryParse(valueString, NumberStyles.Any, CultureInfo.InvariantCulture, out var floatValue);
                value = success ? (T)(object)floatValue : default;
            }
            else if (typeof(T) == typeof(string))
            {
                success = true;
                value = (T)(object)valueString;
            }
            return success;
        }

        private string ValueToString(in object value)
        {
            return value switch
            {
                bool boolValue => boolValue.ToString(CultureInfo.InvariantCulture).ToLowerInvariant(),
                int intValue => intValue.ToString(CultureInfo.InvariantCulture),
                float intValue => intValue.ToString(CultureInfo.InvariantCulture),
                _ => value.ToString()
            };
        }

        [Serializable]
        private class ValueMap : SerializedDictionary<string, string> { }
    }
}
