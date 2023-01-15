using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Cysharp.Threading.Tasks;
using EasyButtons;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;

public class Interdemensionalizer : MonoBehaviour
{
    public Texture2D image;
    [FormerlySerializedAs("url")] public string urlPoopy = "https://framespace.leodastur.com/api/newImage";


    [Button]
    public async UniTask<FileInfo> Interdemensionalize(byte[] bytes, string prompt)
    {
        var urlResponse = await GetUplaodURL();
        string uurl = urlResponse.imageUploadURL;
        /*using (Stream stream = new MemoryStream())
        {
            UploadFileAsync<bool>(uurl, new Dictionary<string, string>(), "", "Image.jpg", stream);
        }*/
        await UploadImageAsync(uurl, bytes, "image.jpg");

        ResponseClass promptClass = new ResponseClass();
        promptClass.prompt = prompt;
        var promptResultJson = await PostAsync($"https://framespace.leodastur.com/api/transformImage/{urlResponse.imageID}", "",
            JsonConvert.SerializeObject(promptClass));

        DownloadUriResponse downloadUriResponse = JsonConvert.DeserializeObject<DownloadUriResponse>(promptResultJson);
        string downloadURL = downloadUriResponse.transformedImageURL;
        string id = urlResponse.imageID;

        string filepath = Path.Combine(Application.persistentDataPath, "Images", $"{id}.jpg");
        FileInfo interDementionalizedFile = new FileInfo(filepath);

        await DownloadFileAsync(downloadURL, interDementionalizedFile.FullName);

        return interDementionalizedFile;
    }
    
    public async UniTask UploadImageAsync(string url, byte[] imageBytes, string fileName)
    {
        using (var client = new HttpClient())
        {
            using (var content = new MultipartFormDataContent())
            {
                var fileContent = new ByteArrayContent(imageBytes);
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                content.Add(fileContent, "file", fileName);

                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException("Failed to upload image: " + response.ReasonPhrase);
                }
            }
        }
    }

    
    [Button]
    public async UniTask<UploadUriResponse> GetUplaodURL()
    {
        var blah = await PostAsync(urlPoopy, "", "");
        if (blah == null)
            return null;
        
        return JsonConvert.DeserializeObject<UploadUriResponse>(blah);
    
    }


    /// <summary>
/// Calls the protected Web API and Posts new data to database
/// </summary>
/// <param name="webApiUrl">Url of the Web API to call</param>
/// <param name="accessToken">Access token used as a security token to call the Web API</param>
/// <param name="serializedObject">List of Dictionaries containing parameters added to the body of the request</param>
/// <returns> A boolean true representing content was posted properly; otherwise, false </returns>
    public async UniTask<string> PostAsync(string requestUrl, string accessToken, string serializedContent)
    {
        string result = String.Empty;
        bool isContentPosted = false;
        var body = new StringContent(serializedContent, Encoding.UTF8, "application/json");
        try
        {
            HttpResponseMessage response = await _client.PostAsync(requestUrl, body);
            if (response.IsSuccessStatusCode)
            {
                isContentPosted = true;
                Debug.Log("Http Request Successful");
                result = await response.Content.ReadAsStringAsync();
                Debug.Log($"Response Content: {result}");
            }
            else
            {
                isContentPosted = false;
                string content = await response.Content.ReadAsStringAsync();

                // It's ok to not have a manager
                if (!content.Contains("Resource 'manager' does not exist"))
                {
                    Debug.LogError($"Failed to call the Web Api: {response.StatusCode}");
                    Debug.LogError($"Content: {content}");
                }
                else
                {
                    Debug.Log("No manager");
                }
            }
        }
        catch (Exception e)
        {
            isContentPosted = false;
            Debug.Log($"EXCEPTION: {e.Message}");
        }

        return result;
    }

    private HttpClient HttpClient = new HttpClient();
    
    
    /// <summary>
    /// Uploads a provided file (Stream) via protect Web API
    /// </summary>
    /// <param name="webApiUrl">Url of the Web API to call</param>
    /// <param name="parameters">Dictionary of parameters added to the request</param>
    /// <param name="accessToken">Access token used as a security token to call the Web API</param>
    /// <param name="fileName">Desired name file being uploaded </param>
    /// <param name="fileStream">Stream for file bein uploaded</param>
    /// <returns> A boolean true representing file was uploaded properly; otherwise, false </returns>
    public async UniTask<bool> UploadFileAsync<Bool>(string requestUrl, Dictionary<string, string> parameters, string accessToken, string fileName, Stream fileStream)
    {
        Boolean isFileUploaded = false;
        //build request

        //Add parameters to request.
        if (parameters.Count > 0)
        {
            requestUrl += '?';
            foreach (var parameter in parameters)
            {
                //defaultRequestHeaders.Add(parameter.Key,parameter.Value);
                requestUrl += $"{parameter.Key}={parameter.Value}&";
            }
        }
        Debug.Log($"request URL: {requestUrl}");

        try
        {
            MultipartFormDataContent form = new MultipartFormDataContent();
            var fileStreamContent = new StreamContent(fileStream);
            form.Add(fileStreamContent, "file", fileName);
            HttpResponseMessage response = await HttpClient.PostAsync(requestUrl, form);
            if (response.IsSuccessStatusCode)
            {
                isFileUploaded = true;
                Debug.Log("Http Request Successful");
                var jsonString = await response.Content.ReadAsStringAsync();
                Debug.Log($"Response Content: {jsonString}");
            }
            else
            {
                isFileUploaded = false;
                string content = await response.Content.ReadAsStringAsync();

                // It's ok to not have a manager
                if (!content.Contains("Resource 'manager' does not exist"))
                {
                    Debug.Log($"Failed to call the Web Api: {response.StatusCode}");
                    Debug.Log($"Content: {content}");
                }
                else
                {
                    Debug.Log("No manager");
                }
            }
        }
        catch (Exception e)
        {
            isFileUploaded = false;
            Debug.Log($"EXCEPTION: {e.Message}");
        }
        
        return isFileUploaded;
    }

    public async UniTask DownloadFileAsync(string url, string filePath)
    {
        using (var client = new HttpClient())
        {
            using (var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();
                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await response.Content.CopyToAsync(fileStream);
                }
            }
        }
    }
    
    private HttpClient _client = new HttpClient();

}

public class UploadUriResponse
{
    
    public bool success { get; set; }
    public string imageUploadURL { get; set; }
    public string imageID { get; set; }

    public UploadUriResponse() { }
}

public class DownloadUriResponse
{
    public bool success { get; set; }
    public string transformedImageURL { get; set; }
    
    public DownloadUriResponse(){}
}

public class ResponseClass
{
    public string prompt { get; set; }
    
    public ResponseClass(){}
}