


using System.Collections.Generic;
using Argyle.UnclesToolkit.Scriptable;
using UnityEngine;

[CreateAssetMenu(fileName = "New themem", menuName = "FrameSpace/Theme")]
public class Theme : ArgyleScriptableObject
{
    public string Name;
    public List<string> Prompts = new List<string>();


    GetPrompt()
    {
        
    }
}