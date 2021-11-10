using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonController : MonoBehaviour
{
#pragma warning disable 0414
    #region Properties
    public bool CanMove { get; set; } = true;
    private bool IsSprinting => canSprint && sprintPressed;
    private bool ShouldJump => jumpPressed && isGrounded;
    private bool ShouldCrouch => crouchPressed && !duringCrouchAnim && isGrounded;
    private bool IsSliding
    {
        get
        {
            Debug.DrawRay(transform.position, Vector3.down, Color.red);
            if (controller.isGrounded && Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 100f))
            {
                hitPointNormal = slopeHit.normal;
                return Vector3.Angle(hitPointNormal, Vector3.up) > controller.slopeLimit;
            }
            else
            {
                return false;
            }
        }
    }
    #endregion

    #region Displayed Varaibles
    [Header("Functionalities")]
    [SerializeField] private bool canSprint = true;
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool canCrouch = true;
    [SerializeField] private bool canHeadbob = true;
    [SerializeField] private bool canZoom = true;
    [SerializeField] private bool willSlideOnSlopes = true;
    [SerializeField] private bool canInteract = true;

    [Header("Player Flags")]
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isCrouching;
    [SerializeField] private bool isSprinting;
    [SerializeField] private bool isFallng;
    [SerializeField] private bool isSliding;
    [SerializeField] private bool isZooming;
    [SerializeField] private bool isInteracting;
    
    [Header("Controls")]
    [SerializeField] private readonly KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private readonly KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private readonly KeyCode crouchKey = KeyCode.LeftControl;
    [SerializeField] private readonly KeyCode zoomKey = KeyCode.Mouse1;
    [SerializeField] private readonly KeyCode interactKey = KeyCode.E;

    [Header("Movement")]
    [SerializeField] private float crouchSpeed = 1.5f;
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float sprintSpeed = 6.0f;
    [SerializeField] private float slopeSpeed = 8.0f;

    [Header("Look")]
    [SerializeField, Range(0.01f, 1.0f)] private float lookSpeedX = 2.0f; 
    [SerializeField, Range(0.01f, 1.0f)] private float lookSpeedY = 2.0f; 
    [SerializeField, Range(1, 180)] private float upperLookLimit = 80.0f; 
    [SerializeField, Range(1, 180)] private float lowerLookLimit = 80.0f;

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 8.0f;
    [SerializeField] private float gravity = 30.0f;

    [Header ("Crouching")]
    [SerializeField] private float crouchHeight = 0.5f;
    [SerializeField] private float standingHeight = 2.0f;
    [SerializeField] private float timeToCrouch = 0.25f;
    [SerializeField] private Vector3 crouchingCenter = new Vector3(0, 0.5f, 0);
    [SerializeField] private Vector3 standingCenter = new Vector3(0, 0, 0);
    private bool duringCrouchAnim;

    [Header("Headbob")]
    [SerializeField] private float crouchbobSpeed = 8.0f;
    [SerializeField] private float crouchbobAmount = 0.025f;
    [SerializeField] private float walkbobSpeed = 14.0f;
    [SerializeField] private float walkbobAmount = 0.1f;
    [SerializeField] private float sprintbobSpeed = 18.0f;
    [SerializeField] private float sprintbobAmount = 0.05f;
    private float defaultYPos = 0;
    private float timer;

    [Header ("Zoom")]
    [SerializeField] private float timeToZoom = 0.3f;
    [SerializeField] private float zoomFOV = 30.0f;
    private float defaultFOV;
    private Coroutine zoomRoutine;

    [Header("Interaction")]
    [SerializeField] private Vector3 interactionRaypoint = default;
    [SerializeField] private float interactionDistance = default;
    [SerializeField] private LayerMask interactionLayer = default;
    [SerializeField] private int interactableLayer = 7;
    private Interactable currentInteractable;

    [Header("Layer Mask")]
    [SerializeField] private LayerMask mask;
    #endregion

    #region Private Variables
    [Header("Keys pressed")]
    private bool sprintPressed;
    private bool crouchPressed;
    private bool jumpPressed;
    private bool zoomInPressed;
    private bool zoomOutPressed;
    private bool interactPressed;

    [Header("Sliding")]
    private Vector3 hitPointNormal;

    private Camera playerCamera;
    private CharacterController controller;
    [SerializeField] PlayerControls controls;

    private Vector3 moveDirection;
    private Vector2 currentInput;

    private Vector2 mouseInput;
    private float rotationX = 0;
    #endregion

    void Awake()
    {
        playerCamera = GetComponentInChildren<Camera>();
        controller = GetComponent<CharacterController>();
        defaultYPos = playerCamera.transform.localPosition.y;
        defaultFOV = playerCamera.fieldOfView;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (CanMove)
        {
            HandleMovementInput();
            HandleMouseLook();
            if (canJump)
            {
                HandleJump();
            }
            if (canCrouch)
            {
                HandleCrouch();
            }
            if (canHeadbob)
            {
                HandleHeadbob();
            }
            if (canZoom)
            {
                HandleZoom();
            }
            if (canInteract)
            {
                HandleInteracionCheck();
                HandleInteractionInput();
            }
            ApplayFinalMovemens();
        }
    }

    private void HandleMovementInput()
    {
        isSprinting = IsSprinting;
        currentInput = new Vector2(currentInput.y * (isCrouching ? crouchSpeed: IsSprinting ? sprintSpeed:walkSpeed), currentInput.x * (isCrouching ? crouchSpeed : IsSprinting ? sprintSpeed : walkSpeed));

        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);
        moveDirection.y = moveDirectionY;
    }

    private void HandleMouseLook()
    {
        rotationX -= mouseInput.y * lookSpeedY;
        rotationX = Mathf.Clamp(rotationX, -upperLookLimit, lowerLookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, mouseInput.x * lookSpeedX, 0);
    }

    private void HandleJump()
    {
        if (ShouldJump)
        {
            moveDirection.y = jumpForce;
            jumpPressed = false;
        }
    }

    private void HandleCrouch()
    {
        if (ShouldCrouch)
        {
            StartCoroutine(CrouchStand());
        }
    }

    private void HandleHeadbob()
    {
        if (!isGrounded)
        {
            return;
        }
        if (Mathf.Abs(moveDirection.x) > 0.1f || Mathf.Abs(moveDirection.z) > 0.1f)
        {
            timer += Time.deltaTime * (isCrouching ? crouchbobSpeed : IsSprinting ? sprintbobSpeed:walkbobSpeed);
            playerCamera.transform.localPosition = new Vector3(
                playerCamera.transform.localPosition.x,
                defaultYPos + Mathf.Sin(timer) * (isCrouching ? crouchbobAmount : IsSprinting ? sprintbobAmount : walkbobAmount),
                playerCamera.transform.localPosition.z);
        }
    }

    private void HandleZoom()
    {
        if (zoomInPressed)
        {
            if (zoomRoutine != null)
            {
                StopCoroutine(zoomRoutine);
                zoomRoutine = null;
            }
            isZooming = true;
            zoomInPressed = false;
            zoomRoutine = StartCoroutine(ToggleZoom(true));
        }
        if (zoomOutPressed)
        {
            if (zoomRoutine != null)
            {
                StopCoroutine(zoomRoutine);
                zoomRoutine = null;
            }
            zoomOutPressed = false;
            isZooming = false;
            zoomRoutine = StartCoroutine(ToggleZoom(false));
        }
    }

    private void HandleInteracionCheck()
    {
        if (Physics.Raycast(playerCamera.ViewportPointToRay(interactionRaypoint), out RaycastHit hit, interactionDistance))
        {
            if (hit.collider.gameObject.layer == interactableLayer && (currentInteractable == null || hit.collider.gameObject.GetInstanceID() != currentInteractable.GetInstanceID()))
            {
                hit.collider.TryGetComponent(out currentInteractable);

                if (currentInteractable)
                {
                    currentInteractable.OnFocus();
                }
            }
        }
        else if (currentInteractable)
        {
            currentInteractable.OnLoseFocus();
            currentInteractable = null;
        }
    }

    private void HandleInteractionInput()
    {
        if (interactPressed && currentInteractable != null && Physics.Raycast(playerCamera.ViewportPointToRay(interactionRaypoint), out RaycastHit hit, interactionDistance, interactionLayer))
        {
            currentInteractable.OnInteract();
        }
    }

    private void ApplayFinalMovemens()
    {
        // isGrounded = Physics.CheckSphere(transform.position, 0.1f, mask);
        isGrounded = controller.isGrounded;
        isSliding = IsSliding;
        if (!isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
            isFallng = true;
        }
        else
        {
            isFallng = false;
        }
        if (controller.velocity.y < -1 && isGrounded)
        {
            moveDirection.y = 0;
        }
        if (willSlideOnSlopes && IsSliding)
        {
            moveDirection += new Vector3(hitPointNormal.x, -hitPointNormal.y, hitPointNormal.z) * slopeSpeed;
        }
        controller.Move(moveDirection * Time.deltaTime);

    }

    public void GetInput(Vector2 input, Vector2 mouse)
    {
        currentInput = input;
        mouseInput = mouse;
    }

    public void SetFlags(string flag)
    {
        switch (flag)
        {
            case "Interact":
                isInteracting = !isInteracting;
                break;
            default:
                break;
        }
    }

    public bool GetFlags(string flag)
    {

        switch (flag)
        {
            case "Interact":
                return isInteracting;
            default:
                return false;
        }
    }

    public void KeyPressed(string action)
    {
        switch (action)
        {
            case "Sprint":
                sprintPressed = true;
                break;
            case "Crouch":
                crouchPressed = true;
                break;
            case "Jump":
                jumpPressed = true;
                break;
            case "Zoom":
                zoomInPressed = true;
                break;
            case "Interact":
                interactPressed = true;
                break;
            default:
                print("Error");
                break;
        }
    }

    public void KeyReleased(string action)
    {
        switch (action)
        {
            case "Sprint":
                sprintPressed = false;
                break;
            case "Crouch":
                crouchPressed = false;
                break;
            case "Zoom":
                zoomOutPressed = true;
                break;
            case "Interact":
                interactPressed = false;
                break;
            default:
                print("Error");
                break;
        }
    }

    private IEnumerator CrouchStand()
    {
        if (isCrouching && Physics.Raycast(playerCamera.transform.position, Vector3.up, 3f))
        {
            yield break;
        }
        duringCrouchAnim = true;
        float timeElapsed = 0;
        float targetHeight = isCrouching ? standingHeight : crouchHeight;
        float currentHeight = controller.height;
        Vector3 targetCenter = isCrouching ? standingCenter : crouchingCenter;
        Vector3 currentCenter = controller.center;

        while (timeElapsed < timeToCrouch)
        {
            controller.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / timeToCrouch);
            controller.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrouch);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        controller.height = targetHeight;
        controller.center = targetCenter;

        isCrouching = !isCrouching;

        duringCrouchAnim = false;
    }

    private IEnumerator ToggleZoom(bool isEnter)
    {
        float targetFOV = isEnter ? zoomFOV : defaultFOV;
        float startingFOV = playerCamera.fieldOfView;
        float timeElapsed = 0;

        while (timeElapsed < timeToZoom)
        {
            playerCamera.fieldOfView = Mathf.Lerp(startingFOV, targetFOV, timeElapsed / timeToZoom);
            timeElapsed += Time.deltaTime;
            print(timeElapsed);
            yield return null;
        }
        playerCamera.fieldOfView = targetFOV;
        zoomRoutine = null;
    }


}
