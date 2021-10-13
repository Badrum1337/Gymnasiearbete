using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInteraction : Interactable
{
    [SerializeField] ConversationHandler test;
    public override void OnFocus()
    {
        print("Looking at char " + gameObject.name);
    }

    public override void OnInteract()
    {
        print("Interacting with char " + gameObject.name);
        test.StartConvo();
    }

    public override void OnLoseFocus()
    {
        print("Stopped looking at char" + gameObject.name);
    }
}
