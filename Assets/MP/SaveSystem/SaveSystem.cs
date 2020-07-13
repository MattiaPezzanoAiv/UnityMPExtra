using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public interface ISaveCallbackReceiver
{
    void OnSaveData(ISaveStream saveStream);
}

public interface ISaveStream
{
    void Write(string key, object data);
}

// todo create example file to test this
public class SaveSystem : MonoBehaviour, ISaveStream
{
    public static SaveSystem Instance { get; private set; }

    private string SaveFilePath => $"{Application.persistentDataPath}/SaveData/SaveFile.json";

    private JObject m_jsonObject;

    private readonly List<ISaveCallbackReceiver> m_toSaveItems = new List<ISaveCallbackReceiver>();

    public event System.Action OnSaveCompleted;

    private void OnApplicationQuit()
    {
        SaveAll();
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void RegisterItem(ISaveCallbackReceiver item)
    {
        m_toSaveItems.Add(item);
    }

    public void UnRegisterItem(ISaveCallbackReceiver item)
    {
        m_toSaveItems.Remove(item);
    }

    public void SaveAll()
    {
        m_jsonObject = new JObject();
        foreach (var item in m_toSaveItems)
        {
            if (item == null)
            {
                Debug.LogError("Null item found. Did you forget to unsubscribe an ISaveCallbackReceiver?");
                continue;
            }

            item.OnSaveData(this);
        }

        string json = m_jsonObject.ToString();
        IOUtils.SafeWriteFileAsync(SaveFilePath, json, () => OnSaveCompleted?.Invoke());
    }

    public void Write(string key, object data)
    {
        m_jsonObject[key] = (JToken)data;
    }

    public T Read<T>(string key)
    {
        var token = m_jsonObject[key];
        return token.ToObject<T>();
    }
}
