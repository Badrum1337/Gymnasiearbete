using DS;
using DS.Handlers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInteraction : Interactable
{
    [SerializeField] DSDialogue Handler;
    public override void OnFocus()
    {
        print("Looking at char " + gameObject.name);
    }

    public override void OnInteract()
    {
        print("Interacting with char " + gameObject.name);
        Handler.StartConvo();
    }

    public override void OnLoseFocus()
    {
        print("Stopped looking at char" + gameObject.name);
    }
}
