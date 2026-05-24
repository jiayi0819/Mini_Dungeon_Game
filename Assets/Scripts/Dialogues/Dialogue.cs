using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public DialogueLine[] dialogueLines;
    //public string name;
    //[TextArea(3, 10)]
    //public string[] sentences;
}
[System.Serializable]
public class DialogueLine
{
    public string speakerName; // Who is talking for this specific line?
    [TextArea(3, 10)]
    public string sentence;    // What are they saying?
}