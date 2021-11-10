using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public SpeakerScript speaker;
    [TextArea]
    public string dialogue;
}
