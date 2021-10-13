using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteraction : Interactable
{
    [SerializeField] ConversationHandler test;
    public override void OnFocus()
    {
        print("Looking at item " + gameObject.name);
    }

    public override void OnInteract()
    {
        print("Interacting with item " + gameObject.name);
        test.StartConvo();
    }

    public override void OnLoseFocus()
    {
        print("Stopped looking at item " + gameObject.name);
    }
}
