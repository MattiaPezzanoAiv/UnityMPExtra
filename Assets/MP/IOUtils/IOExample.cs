using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IOExample : MonoBehaviour
{
    private void Start()
    {
        IOUtils.SafeWriteFile(Application.persistentDataPath + "/TestFolder/RandomFile.txt", "This is a test file");
        IOUtils.SafeWriteFileAsync(Application.persistentDataPath + "/TestFolder/RandomFile2.txt", "This is a test file", () => Debug.Log("Completed"));
    }
}
