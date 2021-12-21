
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController1 : MonoBehaviour
{
    //Serialized field gör variabeln privat men så att den fortfarande kan visas upp i editorn
    [SerializeField] //Change them while in playmode
    private float playerSpeed = 3.0f;

    [SerializeField]
    private float jumpHeight = 1.0f;

    [SerializeField]
    private float gravityValue = -9.81f;

    [SerializeField]
    private float rotationSpeed = 5f;

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private Transform barrelTransform;

    [SerializeField]
    private Transform bulletParent;

    [SerializeField]
    private float bulletHitMissDistance = 25f;

    [SerializeField]
    private float animationSmoothTime = 0.1f;

    [SerializeField]
    private float animationPlayTransition = .15f;

    private CharacterController controller;
    private PlayerInput playerInput;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform cameraTransform;


    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction shootAction;

    private Animator animator;
    int MoveXAnimationParamId;
    int MoveZAnimationParamId;
    int jumpAnimation;

    Vector2 currentAnimationBlendVector;
    Vector2 animationVelocity;



    //Funktion för de olika rörelserna 
    private void Awake() //Awake happens b4 OnEn method
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        shootAction = playerInput.actions["Shoot"];

        Cursor.lockState = CursorLockMode.Locked; //Lockar cursor i mitten av spelet


        //Animations
        animator = GetComponent<Animator>();
        MoveXAnimationParamId = Animator.StringToHash("MoveX");
        MoveZAnimationParamId = Animator.StringToHash("MoveZ");
        jumpAnimation = Animator.StringToHash("Pistol Jump");




    }

    //Funktioner som startas o avslutas när pistolen avfyras
    private void OnEnable()
    {
        shootAction.performed += _ => ShootGun();
    }
    private void OnDisable()
    {
        shootAction.performed -= _ => ShootGun();
    }

    private void ShootGun()
    {
        RaycastHit hit;
        GameObject bullet = GameObject.Instantiate(bulletPrefab, barrelTransform.position, Quaternion.identity, bulletParent);
        BulletController bulletController = bullet.GetComponent<BulletController>();
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity))
        {

            bulletController.target = hit.point;
            bulletController.hit = true;

        }
        else
        {

            bulletController.target = cameraTransform.position + cameraTransform.forward * bulletHitMissDistance;
            bulletController.hit = false;
        }
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 input = moveAction.ReadValue<Vector2>();
        currentAnimationBlendVector = Vector2.SmoothDamp(currentAnimationBlendVector, input, ref animationVelocity, animationSmoothTime) ;

        Vector3 move = new Vector3(currentAnimationBlendVector.x, 0, currentAnimationBlendVector.y);

        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y = 0f;

        controller.Move(move * Time.deltaTime * playerSpeed);


        animator.SetFloat(MoveXAnimationParamId, currentAnimationBlendVector.x);
        animator.SetFloat(MoveZAnimationParamId, currentAnimationBlendVector.y);




        // Changes the height position of the player..
        if (jumpAction.triggered && groundedPlayer) // Will return true if the jump is pressed
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            animator.CrossFade(jumpAnimation,animationPlayTransition); //Transitionerar till jump animationen 
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);


        //Rotate towards camera direction

      //returns camera current y rotation
        Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

    }
}