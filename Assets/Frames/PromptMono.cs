using System.Collections.Generic;
using Argyle.UnclesToolkit.Base;
using EasyButtons;
using UnityEngine;

namespace Frames
{
    public class PromptMono : ArgyleComponent
    {
        public string themeName;
        public List<string> prompts = new List<string>();
        public Theme theme;
        
        [Button]
        public void SendPrompt()
        {
            int i = Random.Range(0, prompts.Count);
            Debug.Log($"Sending Prompt {i}: {prompts[i]}");
            PortalManager.Instance.SetPrompt(prompts[i]);
        }

        [Button]
        public void TransferTemp()
        {
            theme.Name = themeName;
            theme.Prompts = prompts;
        }
    }
}