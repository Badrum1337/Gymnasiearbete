using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Handlers
{
    using ScriptableObjects;
    using Data;
    using System;
    using System.Linq;

    public class DSDialogueManager : MonoBehaviour
    {
        public TextMeshProUGUI speakerName, dialogue;
        [SerializeField]public GameObject option1, option2, option3;
        private GameObject[] buttons;
        private List<bool> allNull;
        private bool allIsNull;
        // public Image speakerSprite;
        private string speaker;
        private static DSDialogueManager instance;
        private DSDialogueSO currentConversation;
        [SerializeField] FirstPersonController player;
        private Animator anim;
        private Coroutine typing;
        private bool isMultiple;
        private bool nextIsEmpty;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                anim = GetComponent<Animator>();
                buttons = new GameObject[] { option1, option2, option3 };
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public static void StartConversation(DSDialogueSO convo, string speaker)
        {
            instance.anim.SetBool("isOpen", true);
            instance.currentConversation = convo;
            instance.speakerName.text = "";
            instance.dialogue.text = "";
            instance.speaker = speaker;
            instance.ReadNext();
        }

        public void ReadNext()
        {
            if (instance.currentConversation == null)
            {
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                UnityEngine.Cursor.visible = false;
                player.SetFlags("Interact");
                player.CanMove = true;
                anim.SetBool("isOpen", false);
                return;
            }

            if (instance.currentConversation.DialogueType == Enumerations.DSDialogueType.SingleChoice)
            {
                speakerName.text = speaker == null ? "" : speaker;

                if (typing == null)
                {
                    typing = instance.StartCoroutine(TypeText(instance.currentConversation.Text));
                }
                else
                {
                    instance.StopCoroutine(typing);
                    typing = null;
                    typing = instance.StartCoroutine(TypeText(instance.currentConversation.Text));
                }

                foreach (DSDialogueChoiceData choiceData in instance.currentConversation.Choices)
                {
                    if (choiceData == instance.currentConversation.Choices[0])
                    {
                        instance.currentConversation = choiceData.NextDialogue;
                    }
                }
            }
            else
            { 
                UnityEngine.Cursor.lockState = CursorLockMode.None;
                UnityEngine.Cursor.visible = true;
                speakerName.text = speaker == null ? "" : speaker;

                if (typing == null)
                {
                    typing = instance.StartCoroutine(TypeText(instance.currentConversation.Text));
                }
                else
                {
                    instance.StopCoroutine(typing);
                    typing = null;
                    typing = instance.StartCoroutine(TypeText(instance.currentConversation.Text));
                }
                isMultiple = true;

            }
        }

        private void ShowButtons()
        {
            allNull = new List<bool>();
            allIsNull = false;
            player.SetFlags("Choosing");
            for (int i = 0; i < instance.currentConversation.Choices.Count; i++)
            { 
                if (instance.currentConversation.Choices[i].NextDialogue != null)
                {
                    buttons[i].SetActive(true);
                    allNull.Add(false);
                }
                else
                {
                    allNull.Add(true);
                }
            }

            TextMeshProUGUI[] texts = new TextMeshProUGUI[] {
                    option1.GetComponentInChildren<TextMeshProUGUI>(),
                    option2.GetComponentInChildren<TextMeshProUGUI>(),
                    option3.GetComponentInChildren<TextMeshProUGUI>(),
                    };
            for (int j = 0; j < instance.currentConversation.Choices.Count; j++)
            {
                texts[j].text = instance.currentConversation.Choices[j].Text;
            }
            if (allNull.All(c => c == true))
            {
                player.SetFlags("Choosing");
                instance.currentConversation = instance.currentConversation.Choices[0].NextDialogue;
            }

        }

        private void HideButtons()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].SetActive(false);
            }
        }

        public void PrintButtonData(GameObject button)
        {
            string whichChoice = button.GetComponentInChildren<TextMeshProUGUI>().text;
            for (int i = 0; i < instance.currentConversation.Choices.Count; i++)
            {
                if (whichChoice == instance.currentConversation.Choices[i].Text)
                {
                    instance.currentConversation = instance.currentConversation.Choices[i].NextDialogue;
                }
            }
            player.SetFlags("Choosing");
            isMultiple = false;
            HideButtons();
            instance.ReadNext();
        }

        private IEnumerator TypeText(string text, float length = 0.02f)
        {
            dialogue.text = "";
            bool complete = false;
            int index = 0;

            while (!complete)
            {
                dialogue.text += text[index];
                index++;
                yield return new WaitForSeconds(length);
                if (index == text.Length)
                {
                    complete = true;
                }
            }

            if (isMultiple)
            {
                ShowButtons();
            }

            typing = null;
        }
    }
}
