using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationHandler : MonoBehaviour
{
    public Conversation convo;
    public FirstPersonController player;
    public void StartConvo()
    {
        player.SetFlags("Interact");
        player.CanMove = false;
        print(player.GetFlags("Interact"));
        DialogueManager.StartConversation(convo);
    }
}
