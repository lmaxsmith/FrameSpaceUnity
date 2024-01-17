using System.Collections;
using System.Collections.Generic;
using Argyle.UnclesToolkit.Base;
using Argyle.UnclesToolkit.SceneStuff;
using EasyButtons;
using UnityEngine;
using System.IO;
using UnityEngine.Events;

public class PortalManager : Manager<PortalManager>
{
    private HashSet<Portal> Portals = new HashSet<Portal>();
    public Portal _portalPrefab;

    public Interdemensionalizer Interdemensionalizer;
    public WebCamCapture WebCamCapture;
    public string prompt;

    public UnityEvent NewPortalEvent = new UnityEvent();
    
    
    [Button]
    public async void NewPortal()
    {
        var imageData = WebCamCapture.Capture();
        var fileInfo = WebCamCapture.SaveImage(imageData);

        Portal portal = Instantiate(_portalPrefab, TForm);
        portal.TForm.position = Reference.MainCameraTransform.position;
        portal.TForm.rotation = Reference.MainCameraTransform.rotation;

        portal.photoFile = fileInfo;
        portal.photoTexture = imageData;
        portal.currentTexture = imageData;
        portal.Display();
        Portals.Add(portal);
        
        FileInfo changedFile = await Interdemensionalizer.Interdemensionalize(imageData.EncodeToJPG(), prompt);

        portal.currentTexture = ReadImageFile(changedFile);
        portal.Display();
        
        NewPortalEvent.Invoke();
    }

    public Texture2D ReadImageFile(FileInfo file)
    {
        Texture2D tex = new Texture2D(512, 512);
        tex.LoadImage(File.ReadAllBytes(file.FullName));
        return tex;
    }

    public void SetPrompt(string p)
    {
        prompt = p;
    }

    public void SetTheme()
    {
        
    }
    
    public void ClearPortals()
    {
        foreach (var portal in Portals)
        {
            Destroy(portal.GO);
            Portals = new HashSet<Portal>();
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
