using UnityEngine;
using System.IO;
using EasyButtons;

public class WebCamCapture : MonoBehaviour
{
    // Reference to the webcam texture
    private WebCamTexture webcamTexture;

    // Use this for initialization
    void Start()
    {
        filePath = Application.streamingAssetsPath + "/photo.jpg";
        
        // Get the default webcam
        WebCamDevice[] devices = WebCamTexture.devices;
        webcamTexture = new WebCamTexture(devices[0].name);

        // Start the webcam
        webcamTexture.Play();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the user pressed the 'Take Photo' button
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Capture();
        }

    }

    [Button]
    public void Capture()
    {
        // Create a new texture with the same resolution as the webcam texture
        Texture2D photo = new Texture2D(webcamTexture.width, webcamTexture.height);

        // Copy the pixels from the webcam texture to the photo texture
        photo.SetPixels(webcamTexture.GetPixels());
        photo.Apply();
        Texture2D cropped = Crop(photo);

        // Encode the photo texture into a JPG
        byte[] bytes = cropped.EncodeToJPG();

        // Save the JPG to a file
        File.WriteAllBytes(filePath, bytes);

        if (File.Exists(filePath))
            Debug.Log($"Photo saved to {filePath}!");
        else
            Debug.Log("Photo failed to save :(");
    }

    string filePath;

    private Texture2D Crop(Texture2D photo)
    {
        int targetWidth = 512;
        int targetHeight = 512;
        // Scale the original image up to 512x512
        Texture2D scaled = ScaleTexture(photo, targetWidth, targetHeight);

        // Define the source rectangle for the crop
        Rect sourceRect = new Rect((scaled.width - targetWidth) / 2, (scaled.height - targetHeight) / 2, targetWidth, targetHeight);

        // Copy the pixels from the webcam texture to the photo texture
        Texture2D cropped = new Texture2D(targetWidth, targetHeight);
        cropped.SetPixels(scaled.GetPixels((int)sourceRect.x, (int)sourceRect.y, (int)sourceRect.width, (int)sourceRect.height));
        cropped.Apply();

        return cropped;
    }

    private Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, false);
        float incX = (1.0f / (float)targetWidth);
        float incY = (1.0f / (float)targetHeight);
        for (int i = 0; i < result.height; ++i)
        {
            for (int j = 0; j < result.width; ++j)
            {
                Color newColor = source.GetPixelBilinear((float)j / (float)result.width, (float)i / (float)result.height);
                result.SetPixel(j, i, newColor);
            }
        }
        result.Apply();
        return result;
    }    
    
    
}