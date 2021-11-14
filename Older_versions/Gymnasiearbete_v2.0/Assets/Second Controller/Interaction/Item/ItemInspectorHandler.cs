using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInspectorHandler : MonoBehaviour
{
    [SerializeField] string Name;
    [SerializeField] [TextArea] string description;
    [SerializeField] FirstPersonController player;
    [SerializeField] GameObject sender;

    [SerializeField] ItemInspectorManager manager;

    public void StartInspect()
    {
        player.SetFlags("ItemInteract");
        player.CanMove = false;
        ItemInspectorManager.StartInspect(sender, sender.name, description);
    }

}
