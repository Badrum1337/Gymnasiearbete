using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI speakerName, dialogue;
    // public Image speakerSprite;

    private int currentIndex;
    private static DialogueManager instance;
    private Conversation currentConversation;
    [SerializeField] FirstPersonController player;
    private Animator anim;
    private Coroutine typing;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            anim = GetComponent<Animator>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void StartConversation(Conversation convo)
    {
        instance.anim.SetBool("isOpen", true);
        instance.currentIndex = 0;
        instance.currentConversation = convo;
        instance.speakerName.text = "";
        instance.dialogue.text = "";

        instance.ReadNext();
    }

    public void ReadNext()
    {
        if (currentIndex > currentConversation.GetLength())
        {
            player.SetFlags("Interact");
            player.CanMove = true;
            instance.anim.SetBool("isOpen", false);
            return;
        }
        speakerName.text = currentConversation.GetLineByIndex(currentIndex).speaker.GetName();

        if (typing == null)
        {
            typing = instance.StartCoroutine(TypeText(currentConversation.GetLineByIndex(currentIndex).dialogue));
        }
        else
        {
            instance.StopCoroutine(typing);
            typing = null;
            typing = instance.StartCoroutine(TypeText(currentConversation.GetLineByIndex(currentIndex).dialogue));
        }


        // speakerSprite = currentConversation.GetLineByIndex(currentIndex).speaker.GetSprite();
        currentIndex++;

    }

    private IEnumerator TypeText(string text)
    {
        dialogue.text = "";
        bool complete = false;
        int index = 0;

        while (!complete)
        {
            dialogue.text += text[index];
            index++;
            yield return new WaitForSeconds(0.02f);
            if (index == text.Length)
            {
                complete = true;
            }
        }

        typing = null;
    }
}
