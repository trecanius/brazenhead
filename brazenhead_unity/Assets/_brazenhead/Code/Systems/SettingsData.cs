using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace brazenhead
{
    internal class SettingsData
    {
        private readonly Dictionary<string, string> _valueByKey = new();
        //private readonly Dictionary<string, List<string>> _keysBySection = new();

        internal bool HasValue(in string key) => _valueByKey.ContainsKey(key);

        internal string GetValue(in string key, in string defaultValue = "") => _valueByKey.TryGetValue(key, out string value) ? value : defaultValue;

        internal string SetValue(in string key, in string value) => _valueByKey[key] = value;

        internal bool ReadFromFile(in string path)
        {
            try
            {
                var lines = File.ReadAllLines(path);
                //var currentSectionKeys = new List<string>();
                //_keysBySection.Add("", currentSectionKeys);

                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    int j = line.IndexOf('#');
                    if (j >= 0)
                        line = line[..j];
                    line = line.Trim();
                    if (line.StartsWith('[') && line.EndsWith(']'))
                    {
                        //currentSectionKeys = new();
                        //_keysBySection.Add(line[1..^1], currentSectionKeys);
                    }
                    else
                    {
                        j = line.IndexOf('=');
                        if (j >= 1)
                        {
                            var key = line[..j].TrimEnd();
                            var value = line[(j + 1)..].TrimStart();
                            //currentSectionKeys.Add(key);
                            _valueByKey.Add(key, value);
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
            var sb = new StringBuilder();
            foreach ((var key, var value) in _valueByKey)
                sb.AppendLine($"{key}={value}");
            try
            {
                File.WriteAllText(path, sb.ToString());
                return true;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return false;
            }
        }
    }
}
