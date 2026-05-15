using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class OllamaTest : MonoBehaviour
{
    [SerializeField]
    private string modelName = "phi4-mini";

    private void Start()
    {
        StartCoroutine(TestOllama());
    }

    IEnumerator TestOllama()
    {
        Debug.Log("Sending request to Ollama...");

        string prompt =
            "Generate a short creepy fortune cookie fortune.";

        string json =
            "{\"model\":\"" + modelName + "\"," +
            "\"prompt\":\"" + prompt + "\"," +
            "\"stream\":false}";

        UnityWebRequest request =
            new UnityWebRequest(
                "http://localhost:11434/api/generate",
                "POST"
            );

        byte[] bodyRaw =
            Encoding.UTF8.GetBytes(json);

        request.uploadHandler =
            new UploadHandlerRaw(bodyRaw);

        request.downloadHandler =
            new DownloadHandlerBuffer();

        request.SetRequestHeader(
            "Content-Type",
            "application/json"
        );

        yield return request.SendWebRequest();

        if (request.result ==
            UnityWebRequest.Result.Success)
        {
            Debug.Log("CONNECTED!");

            Debug.Log(
                "RAW RESPONSE:\n" +
                request.downloadHandler.text
            );

            OllamaResponse response =
                JsonUtility.FromJson<OllamaResponse>(
                    request.downloadHandler.text
                );

            Debug.Log(
                "FORTUNE:\n" +
                response.response
            );
        }
        else
        {
            Debug.LogError(
                "FAILED:\n" +
                request.error
            );
        }
    }
}

