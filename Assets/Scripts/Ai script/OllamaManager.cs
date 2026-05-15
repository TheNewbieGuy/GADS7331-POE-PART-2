using System;
using System.Collections;
using System.Diagnostics;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;

public class OllamaManager : MonoBehaviour
{
    public static OllamaManager Instance;

    [Header("Model")]
    [SerializeField] private string modelName = "phi4-mini";

    private const string apiURL =
        "http://localhost:11434/api/generate";

    private bool connected;

    private void Awake()
    {
        Instance = this;
    }

    private IEnumerator Start()
    {
        yield return CheckConnection();

        if (!connected)
        {
            LaunchOllama();

            Debug.Log("Launching Ollama...");

            yield return new WaitForSeconds(5f);

            yield return CheckConnection();
        }

        if (connected)
        {
            Debug.Log("Ollama connected!");

            // Warm up model
            StartCoroutine(
                GenerateFortune((x) =>
                {
                    Debug.Log("Model warmed up.");
                })
            );
        }
        else
        {
            Debug.LogError("Could not connect to Ollama.");
        }
    }

    IEnumerator CheckConnection()
    {
        UnityWebRequest request =
            UnityWebRequest.Get(
                "http://localhost:11434/api/tags"
            );

        yield return request.SendWebRequest();

        connected =
            request.result ==
            UnityWebRequest.Result.Success;
    }

    void LaunchOllama()
    {
        try
        {
            Process.Start(
                new ProcessStartInfo
                {
                    FileName = "ollama",
                    Arguments = "serve",
                    CreateNoWindow = true,
                    UseShellExecute = false
                }
            );
        }
        catch
        {
            Debug.LogError(
                "Ollama is not installed."
            );
        }
    }

    public IEnumerator GenerateFortune(
        Action<string> callback)
    {
        string prompt =
            "Generate ONE short fortune cookie fortune or quote. " +
            "Maximum 5 words. In quotation marks."+
            "Positive, negative, or neutral. (do not write Positive, negative, or neutral, just the fortune)";

        string json =
            "{\"model\":\"" + modelName + "\"," +
            "\"prompt\":\"" + prompt + "\"," +
            "\"stream\":false}";

        UnityWebRequest request =
            new UnityWebRequest(apiURL, "POST");

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
            OllamaResponse response =
                JsonUtility.FromJson<OllamaResponse>(
                    request.downloadHandler.text
                );

            string fortune =
                response.response.Trim();

            callback?.Invoke(fortune);
        }
        else
        {
            Debug.LogError(request.error);

            callback?.Invoke(
                "Your fate is uncertain."
            );
        }
    }
}
