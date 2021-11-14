using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DS
{
    using DS.Handlers;
    using ScriptableObjects;
    public class DSDialogue : MonoBehaviour
    {
        // Scriptalbe Objects
        [SerializeField] private DSDialogueContainerSO dialogueContianer;
        [SerializeField] private DSDialogueGroupSO dialogueGroup;
        [SerializeField] private DSDialogueSO dialogue;

        // Filters
        [SerializeField] private bool groupedDialogues;
        [SerializeField] private bool startingDialogueOnly;

        // Indexes
        [SerializeField] private int selectedDialoguesGroupIndex;
        [SerializeField] private int selectedDialogueIndex;

        [SerializeField] private FirstPersonController player;

        [SerializeField] private DSDialogueSO startingDialogue;

        [SerializeField] private DSDialogueManager manager;

        public void StartConvo()
        {
            player.SetFlags("Interact");
            player.CanMove = false;
            DSDialogueManager.StartConversation(dialogue, name);
        }
    }
}