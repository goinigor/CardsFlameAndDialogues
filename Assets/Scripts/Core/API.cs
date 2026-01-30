using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using System.Threading;
using CFD.Core;

public class API : IService
{
    private float timeoutSeconds = 30f;

    private CancellationTokenSource cancellationTokenSource;

    /// <summary>
    /// Download dialogue data from the specified URL (async)
    /// </summary>
    public async UniTask Get<T>(string url, Action<T> onSuccess, Action<string> onFail, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(url))
        {
            onFail?.Invoke("[API] url is empty");
            return;
        }
        
        try
        {
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                request.timeout = (int)timeoutSeconds;

                await request.SendWebRequest().WithCancellation(cancellationToken);

                // Check for errors
                if (request.result != UnityWebRequest.Result.Success)
                {
                    var errorMsg = $"[API] Failed to download data: {request.error}";
                    Debug.LogError(errorMsg);
                    onFail?.Invoke(errorMsg);
                    throw new Exception(errorMsg);
                }

                // Parse JSON
                var jsonText = request.downloadHandler.text;
                var data = JsonUtility.FromJson<T>(jsonText);

                Debug.Log($"[API] Successfully loaded");
                onSuccess?.Invoke(data);
            }
        }
        catch (OperationCanceledException)
        {
            onFail("[API] Download operation was cancelled");
            throw;
        }
        catch (Exception e)
        {
            string errorMsg = $"[API] Failed to download or parse data: {e.Message}";
            Debug.LogError(errorMsg);
            onFail?.Invoke(errorMsg);
            throw;
        }
    }

    /// <summary>
    /// Download texture from URL
    /// </summary>
    public async UniTask<Texture2D> DownloadTexture2D(string url, CancellationToken cancellationToken = default)
    {
        try
        {
            using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
            {
                request.timeout = (int)timeoutSeconds;

                Debug.Log($"[API] Downloading texture from {url}");
                
                await request.SendWebRequest().WithCancellation(cancellationToken);

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"[API] Failed to download texture from {url}: {request.error}");
                    return null;
                }

                var texture = DownloadHandlerTexture.GetContent(request);
                return texture;
            }
        }
        catch (OperationCanceledException)
        {
            Debug.Log($"[API] Texture download cancelled: {url}");
            return null;
        }
        catch (Exception e)
        {
            Debug.LogError($"[API] Error downloading Texture: {e.Message}");
            return null;
        }
    }
}