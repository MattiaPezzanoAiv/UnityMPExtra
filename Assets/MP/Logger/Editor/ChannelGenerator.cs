using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MP.Logging;
using System.IO;

public static class ChannelGenerator
{
   
    [MenuItem("Tools/MP/Export Log Channels")]
    public static void ExportLogChannels()
    {
        var types = Log.GetChannelTypes();

        string classText = "///////////////////////////////////////////////////////////////////\n" +
            "// THIS IS AUTO GENERATED CODE, DO NOT MODIFY THIS FILE\n" +
            "///////////////////////////////////////////////////////////////////\n\n" + 
            "public static class LogChannel\n" +
            "{\n";

        foreach (var t in types)
        {
            if(t.IsInterface)
            {
                continue;
            }

            classText += $"\tprivate static readonly {t.FullName} s_{t.Name} = new {t.FullName}();\n";
            classText += $"\tpublic static {t.FullName} {t.Name} => s_{t.Name};\n";
            classText += "\n";

            Debug.Log(t.Name);
        }

        classText += "}";

        string path = "Assets/LogChannels.cs";
        using (var sw = File.CreateText(path))
        {
            sw.Write(classText);
        }

        AssetDatabase.Refresh();
    }
}
