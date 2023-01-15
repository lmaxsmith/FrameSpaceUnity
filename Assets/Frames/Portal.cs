using System.Collections;
using System.Collections.Generic;
using System.IO;
using Argyle.UnclesToolkit.Base;
using UnityEngine;

public class Portal : ArgyleComponent
{
    public FileInfo photoFile;
    public Texture2D photoTexture;
    public Texture2D currentTexture;
    public MeshRenderer photoRenderer;

    public void Display()
    {
        photoRenderer.material.mainTexture = currentTexture;
    }
    
    public void GetChange()
    {
        
    }
}
