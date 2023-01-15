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
        filePath = Application.dataPath + "/photo.jpg";
        
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
        Texture2D cropped = new Texture2D(512, 512);
        cropped.Apply();

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

    private void CropSquare(int sdf )
    {
        // Create a new texture with the desired crop dimensions
        int cropWidth = 300;
        int cropHeight = 200;
        Texture2D photo = new Texture2D(cropWidth, cropHeight);

        // Define the source rectangle for the crop
        Rect sourceRect = new Rect((webcamTexture.width - cropWidth) / 2, (webcamTexture.height - cropHeight) / 2, cropWidth, cropHeight);

        // Copy the pixels from the webcam texture to the photo texture
        photo.SetPixels(webcamTexture.GetPixels((int)sourceRect.x, (int)sourceRect.y, (int)sourceRect.width, (int)sourceRect.height));
        photo.Apply();

        // Encode the photo texture into a JPG
        byte[] bytes = photo.EncodeToJPG();

        // Save the JPG to a file
        File.WriteAllBytes(Application.dataPath + "/photo.jpg", bytes);

        Debug.Log("Photo saved!");
    }
    
    
    
}