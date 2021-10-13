using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
#pragma warning disable 649

    [SerializeField] PlayerManager manager;
    [SerializeField] CharacterController controller;
    [SerializeField] Camera mainCamera;
    private Vector2 horizontalInput;

    [SerializeField] float jumpHeight = 3.5f;

    [SerializeField] float gravity = -30f;
    Vector3 verticalVelocity = Vector3.zero;

    [SerializeField] LayerMask groundMask;

    private void Start()
    {
        manager.normalHeight = controller.height;
    }
    public void RecieveInput(Vector2 _horizontalInput)
    {
        horizontalInput = _horizontalInput;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 horizontalVelocity;
        manager.isGrounded = Physics.CheckSphere(transform.position, 0.1f, groundMask);

        if (manager.isGrounded)
        {
            verticalVelocity.y = 0;
        }

        #region movement
        if (!manager.isCrouching)
        {
            horizontalVelocity = (transform.right * horizontalInput.x + transform.forward * horizontalInput.y) * manager.normalSpeed;
        }
        else
        {
            horizontalVelocity = (transform.right * horizontalInput.x + transform.forward * horizontalInput.y) * manager.crouchSpeed;
        }

        controller.Move(horizontalVelocity * Time.deltaTime);
        #endregion
        #region crouching
        if (manager.isCrouching)
        {
            if (manager.isGrounded)
            {
                StartCoroutine(CrouchStand());
            }
        }
        else
        {
            controller.height = manager.normalHeight;
        }
        if (manager.shouldUpdate)
        {
            OnCrouchReleased();
        }
        #endregion
        #region jumping
        if (manager.isJumping)
        {
            if (manager.isGrounded)
            {
                verticalVelocity.y = Mathf.Sqrt(-2f * jumpHeight * gravity);
                manager.isJumping = false;
            }
        }
        verticalVelocity.y += gravity * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);
        #endregion
    }

    private IEnumerator CrouchStand()
    {
        // isInAnim
        float timeElapsed = 0;
        float targetHeight = manager.isCrouching ? manager.normalHeight : manager.crouchHeight;
        float currentHeight = controller.height;
        Vector3 targetCenter = manager.isCrouching ? manager.normalCenter : manager.crouchCenter;
        Vector3 currentCenter = controller.center;

        while (timeElapsed < manager.timeToCrouch)
        {
            controller.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / manager.timeToCrouch);
            controller.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / manager.timeToCrouch);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        controller.height = targetHeight;
        controller.center = targetCenter;

        manager.isCrouching = !manager.isCrouching;

        //isOutOfAnim
    }
    public void OnJumpPressed()
    {
        manager.isJumping = true;
    }

    public void OnCrouchPressed()
    {
        manager.isCrouching = true;
    }
    public void OnCrouchReleased()
    {
        Vector3 position = new Vector3(transform.position.x, transform.position.y + manager.crouchHeight, transform.position.z);
        bool canUncrouch = Physics.CheckSphere(position, 1.5f, groundMask);
        if (!canUncrouch)
        {
            manager.shouldUpdate = false;
        }
        else
        {
            manager.shouldUpdate = true;
        }
    }
}
