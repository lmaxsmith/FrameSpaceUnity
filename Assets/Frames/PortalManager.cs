using System.Collections;
using System.Collections.Generic;
using Argyle.UnclesToolkit.Base;
using Argyle.UnclesToolkit.SceneStuff;
using EasyButtons;
using UnityEngine;

public class PortalManager : Manager<PortalManager>
{
    private HashSet<Portal> Portals = new HashSet<Portal>();
    public Portal _portalPrefab;
    
    public WebCamCapture WebCamCapture;
    
    
    [Button]
    public void NewPortal()
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
