using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
#pragma warning disable 649

    [SerializeField] PlayerManager manager;

    [SerializeField] float sensetivtyX = 8f;
    [SerializeField] float sensetivityY = 0.5f;
    float mouseX, mouseY;

    [SerializeField] Transform playerCamera;
    [SerializeField] float xClamp = 85f;
    float xRotation = 0f;

    public void RecieveInput(Vector2 mouseInput)
    {
        mouseX = mouseInput.x * sensetivtyX;
        mouseY = mouseInput.y * sensetivityY;
    }
    private void Update()
    {
        transform.Rotate(Vector3.up, mouseX * Time.deltaTime);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -xClamp, xClamp);
        Vector3 targetRotation = transform.eulerAngles;
        targetRotation.x = xRotation;
        playerCamera.eulerAngles = targetRotation;
    }
}
