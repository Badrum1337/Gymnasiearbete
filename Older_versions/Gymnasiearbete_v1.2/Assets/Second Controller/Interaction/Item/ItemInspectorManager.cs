using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using TMPro;
using System;

public class ItemInspectorManager : MonoBehaviour
{

    [SerializeField] private GameObject objectNameBG;
    [SerializeField] private GameObject itemObject;
    [SerializeField] private TextMeshProUGUI objectName;
    [SerializeField] private TextMeshProUGUI objectDescription;
    [SerializeField] private FirstPersonController player;
    private static ItemInspectorManager instance;
    public Animator anim;
    private Vector3 originalPosition;
    private Vector3 originalScale;
    private Quaternion originalRotation;
    private GameObject sent;
    private Coroutine rotation;
    

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

    public static void StartInspect(GameObject sender, string name, string desc)
    {
        print(sender.name);
        instance.sent = Instantiate(sender);
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
        instance.anim.SetBool("isOpen", true);
        instance.originalPosition = sender.transform.position;
        instance.originalScale = sender.transform.localScale;
        instance.originalRotation = sender.transform.rotation;
        instance.sent.transform.position = instance.itemObject.transform.position;
        instance.sent.layer = 9;
        instance.itemObject.SetActive(false);
        instance.objectName.text = name;
        instance.objectDescription.text = desc;
        instance.RotateItem(instance.sent);
           
    }

    private void RotateItem(GameObject origin)
    {
        if (rotation == null)
        {
            rotation = instance.StartCoroutine(Rotation(origin));
        }
    }

    public void Exit()
    {
        
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        player.SetFlags("ItemInteract");
        player.CanMove = true;
        anim.SetBool("isOpen", false);

    }

    private IEnumerator Rotation(GameObject origin, float speed = 0.025f)
    {
        float originY = origin.transform.eulerAngles.y;

        while (player.GetFlags("ItemInteract"))
        {
            originY += 2f;
            origin.transform.eulerAngles = new Vector3(origin.transform.eulerAngles.x, originY, origin.transform.eulerAngles.z);
            yield return new WaitForSeconds(speed);
        }

        rotation = null;
    }
}
