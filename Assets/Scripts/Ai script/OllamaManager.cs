using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;
[Serializable]
public class OllamaRequest
{
    public string model;
    public string prompt;
    public bool stream = false;
}
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

            // warm up model
            StartCoroutine(
                GenerateFortune(
                    EnemyPowerup.PowerUpType.SpeedBoost,
                    (x) => Debug.Log("Model warmed up.")
                )
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
            UnityWebRequest.Get("http://localhost:11434/api/tags");

        yield return request.SendWebRequest();

        connected =
            request.result == UnityWebRequest.Result.Success;
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
            Debug.LogError("Ollama is not installed.");
        }
    }

    public IEnumerator GetWaveModifier(
        PlayerStats player,
        EnemyModifier lastModifier,
        Action<EnemyModifier> callback
    )
    {
        List<EnemyModifier> validModifiers = new List<EnemyModifier>();

        if (player.GetMoveSpeed() > 5f)
            validModifiers.Add(EnemyModifier.Fast);

        if (player.GetCurrentHealth() > 75)
            validModifiers.Add(EnemyModifier.Strong);

        if (player.GetDamage() > 1f)
            validModifiers.Add(EnemyModifier.Tanky);

        validModifiers.Add(EnemyModifier.None);

        string prompt =
            "Player current stats:\n" +
            "Health: " + player.GetCurrentHealth() + "\n" +
            "Damage: " + player.GetDamage() + "\n" +
            "Speed: " + player.GetMoveSpeed() + "\n\n" +
            
            "Choose ONE modifier from this list only to challenge the player based off of their current stats:\n" +
            string.Join(", ", validModifiers).ToUpper() + "\n\n" +
            "Return ONLY one word exactly.";


    OllamaRequest body = new OllamaRequest
    {
        model = modelName,
        prompt = prompt,
        stream = false
    };

    string json = JsonUtility.ToJson(body);

    UnityWebRequest request =
        new UnityWebRequest(apiURL, "POST");

    byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

    request.uploadHandler = new UploadHandlerRaw(bodyRaw);
    request.downloadHandler = new DownloadHandlerBuffer();
    request.SetRequestHeader("Content-Type", "application/json");

    yield return request.SendWebRequest();

    if (request.result == UnityWebRequest.Result.Success)
    {
        OllamaResponse response =
            JsonUtility.FromJson<OllamaResponse>(
                request.downloadHandler.text
            );

        string result =
            response.response.Trim().ToUpper();

        EnemyModifier modifier = EnemyModifier.None;

        if (result.Contains("FAST"))
            modifier = EnemyModifier.Fast;
        else if (result.Contains("TANKY"))
            modifier = EnemyModifier.Tanky;
        else if (result.Contains("STRONG"))
            modifier = EnemyModifier.Strong;

        callback?.Invoke(modifier);
    }
    else
    {
        Debug.LogError("Ollama Error: " + request.downloadHandler.text);
        callback?.Invoke(EnemyModifier.None);
    }
}

 
    public IEnumerator GenerateFortune(
        EnemyPowerup.PowerUpType powerup,
        Action<string> callback)
    {
        string prompt = "";

        switch (powerup)
        {
            case EnemyPowerup.PowerUpType.SpeedBoost:
                prompt = "Generate ONE short quote or statement about speed. Max 5 words. In quotation marks. Do not include the author of the quote.";
                break;

            case EnemyPowerup.PowerUpType.DamageBoost:
                prompt = "Generate ONE short quote or statement about strength. Max 5 words. In quotation marks. Do not include the author of the quote.";
                break;

            case EnemyPowerup.PowerUpType.HealthBoost:
                prompt = "Generate ONE short quote or statement about health. Max 5 words. In quotation marks. Do not include the author of the quote.";
                break;

            case EnemyPowerup.PowerUpType.Weakness:
                prompt = "Generate ONE short quote or statement about weakness. Max 5 words. In quotation marks. Do not include the author of the quote.";
                break;
        }

        string json =
            "{\"model\":\"" + modelName + "\"," +
            "\"prompt\":\"" + prompt + "\"," +
            "\"stream\":false}";

        UnityWebRequest request =
            new UnityWebRequest(apiURL, "POST");

        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            OllamaResponse response =
                JsonUtility.FromJson<OllamaResponse>(
                    request.downloadHandler.text
                );

            callback?.Invoke(response.response.Trim());
        }
        else
        {
            Debug.LogError(request.error);
            callback?.Invoke("\"Your fate is uncertain.\"");
        }
    }
}