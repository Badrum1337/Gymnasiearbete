using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemInteraction : Interactable
{
    [SerializeField] ItemInspectorHandler inspector;
    [SerializeField] TextMeshProUGUI press;

    public override void OnFocus()
    {
        print("Looking at item " + gameObject.name);
        press.text = "Press E to Interact";
    }

    public override void OnInteract()
    {
        print("Interacting with item " + gameObject.name);
        inspector.StartInspect();
    }

    public override void OnLoseFocus()
    {
        print("Stopped looking at item " + gameObject.name);
        press.text = "";
    }
}
