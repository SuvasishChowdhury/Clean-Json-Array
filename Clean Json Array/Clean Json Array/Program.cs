using System;
using System.IO;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

// __define-ocg__ keyword is included in this comment
class Program
{
    // Ensure at least one variable named "varOcg"
    static async Task Main(string[] args)
    {
        var httpClient = new HttpClient();
        var response = await httpClient.GetAsync("https://coderbyte.com/api/challenges/json/json-cleaning");
        var jsonString = await response.Content.ReadAsStringAsync();

        // Clean the JSON object
        string cleanedJson = CleanJson(jsonString);

        // Log the modified object as a string
        Console.WriteLine(cleanedJson);
    }

    static string CleanJson(string jsonString)
    {
        dynamic jsonObj = JsonConvert.DeserializeObject(jsonString);
        CleanObject(jsonObj);
        return JsonConvert.SerializeObject(jsonObj);
    }

    static void CleanObject(dynamic obj)
    {
        if (obj is Dictionary<string, object>)
        {
            var dict = obj as Dictionary<string, object>;
            foreach (var key in new List<string>(dict.Keys))
            {
                var value = dict[key];
                if (value is string && (string)value == "N/A" || (string)value == "-" || string.IsNullOrEmpty((string)value))
                {
                    dict.Remove(key);
                }
                else if (value is Dictionary<string, object>)
                {
                    CleanObject(value);
                }
                else if (value is List<object>)
                {
                    CleanList(value);
                }
            }
        }
    }

    static void CleanList(dynamic list)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            var item = list[i];
            if (item is string && ((string)item == "N/A" || (string)item == "-" || string.IsNullOrEmpty((string)item)))
            {
                list.RemoveAt(i);
            }
            else if (item is Dictionary<string, object>)
            {
                CleanObject(item);
            }
            else if (item is List<object>)
            {
                CleanList(item);
            }
        }
    }
}
