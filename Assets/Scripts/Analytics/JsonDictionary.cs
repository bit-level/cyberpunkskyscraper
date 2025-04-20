using System.Collections.Generic;
using System.Text;

namespace BitLevel.Core.Analytics
{
    public class JsonDictionary
    {
        private readonly Dictionary<string, object> data = new();

        public JsonDictionary Add(string key, string value) { data.Add(key, value); return this; }
        public JsonDictionary Add(string key, int value) { data.Add(key, value); return this; }
        public JsonDictionary Add(string key, float value) { data.Add(key, value); return this; }
        public JsonDictionary Add(string key, JsonDictionary value) { data.Add(key, value); return this; }
        public JsonDictionary Add(string key, string[] value) { data.Add(key, value); return this; }

        public string ToJson()
        {
            StringBuilder sb = new();
            sb.Append("{");

            foreach (KeyValuePair<string, object> kvp in data)
            {
                object v = kvp.Value;
                string value = null;

                if (v is string) value = $"\"{v}\"";
                else if (v is float || v is int) value = v.ToString();
                else if (v is JsonDictionary jsonDict) value = jsonDict.ToJson();
                else if (v is string[]) value = $"[\"{string.Join("\",\"", (string[])v)}\"]";

                sb.Append(string.Format("\"{0}\": {1},", kvp.Key, value));
            }
            
            if (data.Count > 0)
                sb.Remove(sb.Length - 1, 1);

            sb.Append("}");
            return sb.ToString();
        }
    }
}