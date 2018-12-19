using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private Text staminaBar;
    [SerializeField] private Text healthBar;

    public CharacterController charController;
    private PlayerLook playerL;
    private PlayerHitCeil playerHit;

    [SerializeField] private float normalSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float crouchSpeed;
    private float playerSpeed;
    
    [Header("Player Stats")]
    [SerializeField] private int health;
    [SerializeField] private float stress;
    [SerializeField] private float stamina;
    
    private float vertInput;
    private float horizInput;

    [Header("Player Status")]
    #region PlayerStatus
    public bool isHurting = false;
    public bool isJumping = false;
    public bool isCrouching = false;
    public bool isRunning = false;
    public bool isClimbing = false;
    public bool isSliding = false;
    public bool isAnimating = false;
    #endregion


    #region Player Camera
    [Header("Player ParkourAnim")]
    public Animation ParAnim;
    public Camera parkourCam;

    [Header("Player RegularAnim")]
    public Animation RegAnim;
    public Camera regularCam;
    #endregion

    [Header("Player Climb Collision")]
    #region Player Climb Collision
    public bool hitCeil = false;
    public bool canClimb = false;
    public bool canHalfClimbForwad = false;
    #endregion


    private bool isStressed;
    private bool isAlive;
    [SerializeField] private bool isExhausted = false;

    private float maxStamina = 100f;
    private int maxHealth = 3;
    private float jumpStamina = 10f;

    private float freakTime = 0f;

    private float slideTimer = 0.0f;
    [SerializeField] private AnimationCurve jumpFallOff;
    [SerializeField] private float jumpMultiplier;


    void Awake()
    {
        playerSpeed = normalSpeed;
        health = maxHealth;
        stamina = maxStamina;
        playerL = GetComponentInChildren<PlayerLook>();
        charController = GetComponent<CharacterController>();
        playerHit = GetComponentInChildren<PlayerHitCeil>();
    }

    private void FixedUpdate()
    {
        PlayerMovement();
        InterfaceUpdate();
    }

    private void PlayerMovement()
    {
        vertInput = Input.GetAxis("Vertical");
        horizInput = Input.GetAxis("Horizontal");

        if (!isAnimating)
        {
            Move();
            Jump();
            Crouch();

            if (isIdle())
            {
                playerL.breathingTime += Time.deltaTime;
                if (playerL.breathingTime > 0.3f)
                {
                    playerL.Breathing();
                }
            }
        }
    }

    private void Move()
    {
        Vector3 forwardMovement = transform.forward * vertInput;
        Vector3 rightMovement = transform.right * horizInput;
        if (Input.GetButton("Run") && !isExhausted) 
        {
            if(isCrouching)
            {
                playerSpeed = crouchSpeed + 1f;
            }
            else
            {
                if (stamina <= 0)
                {
                    playerSpeed = normalSpeed;
                    isRunning = false;
                    PlayerExhausted();
                }
                if (playerSpeed != runSpeed)
                {
                    playerSpeed = runSpeed;
                    playerL.bobbingSpeed = 0.2f;
                    isRunning = true;
                }
                if(!isIdle())
                {
                    stamina -= Time.deltaTime * 5;
                    slideTimer += Time.deltaTime;
                }
            }
        }
        else if(Input.GetButtonUp("Run"))
        {
            slideTimer = 0f;
            if(!isCrouching)
            {
                playerSpeed = normalSpeed;
                playerL.bobbingSpeed = 0.1f;
            }
            else
            {
                playerSpeed = crouchSpeed;
                playerL.bobbingSpeed = 0.1f;
            }
            isRunning = false;
        }
        charController.SimpleMove(Vector3.ClampMagnitude(forwardMovement + rightMovement, 1.0f) * playerSpeed);
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && charController.isGrounded && !isExhausted)
        {
            isJumping = true;
            StartCoroutine(JumpEvent());
        }
    }

    #region JumpEvents

    private IEnumerator JumpEvent()
    {
        if (isCrouching)
        {
            if (playerHit.isHitCeil) { yield break; }
            charController.height = 2.0f;
            isCrouching = false;
            isJumping = false;
            yield break;
        }

        charController.slopeLimit = 80.0f;
        float timeInAir = 0.0f;

        if(stamina >= jumpStamina)
        {
            stamina -= jumpStamina;
        }
        else
        {
            stamina = 0f;
            PlayerExhausted();
        }

        do
        {
            float jumpforce = jumpFallOff.Evaluate(timeInAir);
            charController.Move(Vector3.up * jumpforce * jumpMultiplier * Time.deltaTime);
            timeInAir += Time.deltaTime;
            yield return null;

        }
        while (!isAnimating && !charController.isGrounded && charController.collisionFlags != CollisionFlags.Above);
        charController.slopeLimit = 60.0f;
        isJumping = false;
    }

    #endregion


    private void Crouch()
    {
        if (Input.GetButtonDown("Crouch"))
        {
            if(slideTimer >= 1.5f)
            {
                isSliding = true;
                StartCoroutine(AnimationEvent("Sliding", 1.4f));
                slideTimer = 0f;
                playerL.bobbingSpeed = 0.1f;
            }

            if (!isCrouching && charController.isGrounded)
            {
                playerL.crouch = isCrouching = true;
                playerL.CrouchSet(true);
                playerSpeed = crouchSpeed;
            }
            else if (isCrouching && !playerHit.isHitCeil)
            {
                playerL.crouch = isCrouching = false;
                playerL.CrouchSet(false);
                playerSpeed = normalSpeed;
            }
        }

        if (isCrouching && charController.height != 1.2f)
        {
            charController.height = Mathf.Lerp(charController.height, 1.2f, Time.deltaTime * 5);

        }
        else if (charController.height != 2f)
        {
            charController.height = Mathf.Lerp(charController.height, 2f, Time.deltaTime * 20);
        }
    }
    

    public IEnumerator AnimationEvent(string animName, float waitTime)
    {
        regularCam.enabled = false;
        parkourCam.enabled = true;
        isJumping = false;
        isAnimating = true;
        regularCam.depth = 0;
        parkourCam.depth = 1;
        playerL.canLook = false;
        ParAnim.Play(animName);
        yield return new WaitForSeconds(waitTime);
        Vector3 climbVector = parkourCam.transform.position;
        climbVector.y -= 0.5f;
        Quaternion rotationVec = parkourCam.transform.localRotation;
        rotationVec.y = 0f;
        rotationVec.z = 0f;
        regularCam.transform.localRotation = rotationVec;
        transform.position = climbVector;
        playerL.canLook = true;
        regularCam.enabled = true;
        parkourCam.enabled = false;
        regularCam.depth = 1;
        parkourCam.depth = 0;
        parkourCam.transform.localPosition = Vector3.zero;
        isClimbing = false;
        isAnimating = false;
        isJumping = false;
        isRunning = false;
        isSliding = false;
        Vector3 temp = regularCam.transform.localPosition;
        parkourCam.transform.parent.localPosition = temp;

    }

    public IEnumerator TerminalEvent(Transform pos,bool isExiting)
    {
        parkourCam.enabled = true;
        regularCam.enabled = false;
        if (!isExiting)
        {
            regularCam.depth = 0;
            parkourCam.depth = 1;
        }
        do
        {
                parkourCam.transform.position = Vector3.Lerp(parkourCam.transform.position, pos.position, Time.deltaTime * 4);
                parkourCam.transform.rotation = Quaternion.Slerp(parkourCam.transform.rotation, pos.rotation, Time.deltaTime * 4);
            if(isExiting)
            {
                freakTime += Time.deltaTime;
                if (freakTime >= 1f)
                {
                    freakTime = 0f;
                    break;
                }
            }
                yield return null;

        } while (isAnimating && parkourCam.transform.position != pos.position && parkourCam.transform.eulerAngles != pos.eulerAngles);

        if (isExiting)
        {
            isAnimating = false;
            regularCam.depth = 1;
            parkourCam.depth = 0;
            Vector3 temp = regularCam.transform.localPosition;
            temp.y -= 0.7f;
            parkourCam.transform.localPosition = temp;
            regularCam.enabled = true;
            parkourCam.enabled = false;
        }

        yield return new WaitForSeconds(1.5f);
    }

    private bool isIdle()
    {
        if (charController.velocity.magnitude == 0.0f)
        {
            if(!isExhausted && stamina <= maxStamina)
            {
                StaminaUpdate();
            }
            return true;
        }

        else
        {
            playerL.breathingTime = 0f;
            return false;
        }
    }
    
    private void InterfaceUpdate()
    {
        if(isHurting)
        {
            health -= 1;
            isHurting = false;
        }
        staminaBar.text = (Mathf.RoundToInt(stamina)).ToString();
        healthBar.text = (Mathf.RoundToInt(health)).ToString();
    }

    private void StaminaUpdate()
    {
        if(isCrouching)
        {
            stamina += Time.deltaTime * 4;
        }
        else
        {
            stamina += Time.deltaTime * 2;
        }
    }

    private void PlayerExhausted()
    {
        if (stamina <= 0.0f)
        {
            isExhausted = true;
            StartCoroutine(ExhaustedTime());
        }
    }

    private IEnumerator ExhaustedTime()
    {
        stamina = 0.0f;

        playerL.freakness = 2.25f;
        yield return new WaitForSeconds(5.0f);
        isExhausted = false;
    }

    public void SetTerminal(bool isTextDisabled)
    {
        if(isTextDisabled == false)
        {
            staminaBar.enabled = false;
            healthBar.enabled = false;
            isAnimating = true;
            playerL.canLook = false;
        }
        else
        {
            playerL.canLook = true;
            staminaBar.enabled = true;
            healthBar.enabled = true;
        }

    }
}
