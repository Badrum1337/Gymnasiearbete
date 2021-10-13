using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] FirstPersonController controller;
    [SerializeField] DialogueManager manager ;
    PlayerControls playerControls;
    PlayerControls.GroundMovementActions groundMovement;

    private Vector2 horizontalInput;
    private Vector2 mouseInput;

    private void Awake()
    {
        playerControls = new PlayerControls();
        groundMovement = playerControls.GroundMovement;

        groundMovement.HorizontalMovement.performed += ctx => horizontalInput = ctx.ReadValue<Vector2>();

        groundMovement.Jump.performed += ctx => controller.KeyPressed(groundMovement.Jump.name);

        groundMovement.Interact.performed += ctx => controller.KeyPressed(groundMovement.Interact.name);
        groundMovement.Interact.canceled += ctx => controller.KeyReleased(groundMovement.Interact.name);

        groundMovement.Sprint.performed += ctx => controller.KeyPressed(groundMovement.Sprint.name);
        groundMovement.Sprint.canceled += ctx => controller.KeyReleased(groundMovement.Sprint.name);

        groundMovement.Crouch.performed += ctx => controller.KeyPressed(groundMovement.Crouch.name);
        groundMovement.Crouch.canceled += ctx => controller.KeyReleased(groundMovement.Crouch.name);

        groundMovement.Zoom.performed += ctx => controller.KeyPressed(groundMovement.Zoom.name);
        groundMovement.Zoom.canceled += ctx => controller.KeyReleased(groundMovement.Zoom.name);

        groundMovement.Continue.performed += ctx => this.UpdateText();

        groundMovement.MouseX.performed += ctx => mouseInput.x = ctx.ReadValue<float>();
        groundMovement.MouseY.performed += ctx => mouseInput.y = ctx.ReadValue<float>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDestroy()
    {
        playerControls.Disable();
    }
    // Update is called once per frame
    void Update()
    {
        controller.GetInput(horizontalInput, mouseInput);
    }

    private void UpdateText()
    {
        if (controller.GetFlags("Interact"))
        {
            manager.ReadNext();
        }
    }
}
