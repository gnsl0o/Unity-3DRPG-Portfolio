using System;
using UnityEngine;

public interface IInteraction
{
    string name { get; set; }
    int dialogueIndex { get; set; }
    void Interact();
}

