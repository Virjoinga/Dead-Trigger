using System.Collections.Generic;
using System.IO;
using SimpleJSON;
using UnityEngine;

public static class PlayerPrefs
{
    static string savePath
    {
        get
        {
            return Path.Combine(Application.persistentDataPath, "PlayerPrefs.json");
        }
    }

    public static Dictionary<string, string> strings = new Dictionary<string, string>();
    public static Dictionary<string, int> ints = new Dictionary<string, int>();
    public static Dictionary<string, float> floats = new Dictionary<string, float>();

    public static void SetString(string key, string value)
    {
        strings[key] = value;
        Save();
    }

    public static string GetString(string key, string defaultValue = "")
    {
        if (strings.ContainsKey(key)) return strings[key];

        strings[key] = defaultValue;
        return defaultValue;
    }

    public static void SetInt(string key, int value)
    {
        ints[key] = value;
        Save();
    }

    public static int GetInt(string key, int defaultValue = 0)
    {
        if (ints.ContainsKey(key)) return ints[key];

        ints[key] = defaultValue;
        return defaultValue;
    }

    public static void SetFloat(string key, float value)
    {
        floats[key] = value;
        Save();
    }

    public static float GetFloat(string key, float defaultValue = 0)
    {
        if (floats.ContainsKey(key)) return floats[key];

        floats[key] = defaultValue;
        return defaultValue;
    }

    public static void Save()
    {
        JSONObject jsonStrings = new JSONObject();
        JSONObject jsonInts = new JSONObject();
        JSONObject jsonFloats = new JSONObject();

        foreach (var v in strings) jsonStrings[v.Key] = v.Value;
        foreach (var v in ints) jsonInts[v.Key] = v.Value;
        foreach (var v in floats) jsonFloats[v.Key] = v.Value;

        JSONObject output = new JSONObject();

        output["strings"] = jsonStrings;
        output["ints"] = jsonInts;
        output["floats"] = jsonFloats;

        File.WriteAllText(savePath, output.ToString(1));
    }

    [RuntimeInitializeOnLoadMethod]
    public static void Load()
    {
        if (!File.Exists(savePath)) return;

        JSONNode index = JSON.Parse(File.ReadAllText(savePath));

        JSONNode jsonStrings = index["strings"];
        JSONNode jsonInts = index["ints"];
        JSONNode jsonFloats = index["floats"];

        foreach (var v in jsonStrings) strings[v.Key] = v.Value;
        foreach (var v in jsonInts) ints[v.Key] = v.Value;
        foreach (var v in jsonFloats) floats[v.Key] = v.Value;
    }

    public static void DeleteKey(string key)
    {
        if (strings.ContainsKey(key)) strings.Remove(key);
        if (ints.ContainsKey(key)) ints.Remove(key);
        if (floats.ContainsKey(key)) floats.Remove(key);

        Save();
    }

    public static bool HasKey(string key)
    {
        return strings.ContainsKey(key) || ints.ContainsKey(key) || floats.ContainsKey(key);
    }
}
