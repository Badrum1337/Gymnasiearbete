using DS;
using DS.Handlers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterInteraction : Interactable
{
    [SerializeField] DSDialogue Handler;
    [SerializeField] TextMeshProUGUI press;
    public override void OnFocus()
    {
        print("Looking at char " + gameObject.name);
        press.text = "Press E to Interact";
    }

    public override void OnInteract()
    {
        print("Interacting with char " + gameObject.name);
        Handler.StartConvo();
        press.text = "";
    }

    public override void OnLoseFocus()
    {
        print("Stopped looking at char" + gameObject.name);
        press.text = "";
    }
}
