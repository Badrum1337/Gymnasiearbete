using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Variables
    [Header("Player Flags")]
    public bool isGrounded;
    public bool isJumping;
    public bool isCrouching;
    public bool shouldUpdate;

    [Header("Player Speeds")]
    public float crouchSpeed = 6f;
    public float normalSpeed = 11f;
    public float timeToCrouch = 0.25f;

    [Header("Player Heigths")]
    public float crouchHeight = 4f;
    public float normalHeight;

    [Header("Player Centers")]
    public Vector3 crouchCenter = new Vector3(0, 0.5f, 0);
    public Vector3 normalCenter = new Vector3(0, 0, 0);
    #endregion
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}