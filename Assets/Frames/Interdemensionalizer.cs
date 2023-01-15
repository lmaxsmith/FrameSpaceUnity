using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ImageUploader : MonoBehaviour
{
    public Texture2D image;
    public string url = "http://yourserver.com/upload";

    public void UploadImage()
    {
        byte[] imageBytes = image.EncodeToJPG();

        UnityWebRequest www = UnityWebRequest.Put(url, imageBytes);
        www.SetRequestHeader("Content-Type", "image/jpeg");
        www.SetRequestHeader("Content-Length", imageBytes.Length.ToString());
        www.method = "POST";
        www.uploadHandler = new UploadHandlerRaw(imageBytes);
        www.downloadHandler = new DownloadHandlerBuffer();

        StartCoroutine(SendRequest(www));
    }

   /* private bool SendRequestAsync()
    {
        
    }*/
    private IEnumerator SendRequest(UnityWebRequest www)
    {
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError(www.error);
        }
        else
        {
            Debug.Log("Image uploaded successfully!");
        }
    }
}