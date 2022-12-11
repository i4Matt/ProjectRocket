using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    //public float sprintSpeed;
    public float slideSpeed;

    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;

    public float speedIncreaseMultiplier;
    public float slopeIncreaseMultiplier;

    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;
    public float crouchForce;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    //public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.C;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;


    public Transform orientation;
    
    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState state;

    public enum MovementState{
        walking,
        sprinting,
        crouching,
        sliding,
        air
    }

    public bool sliding;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        
        readyToJump = true;

        startYScale = transform.localScale.y;
    }

    // Update is called once per frame
    private void Update()
    {
        // Ground Check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();
        StateHandler();

        // Handle Drag
        if(grounded){
            rb.drag = groundDrag;
        }else{
            rb.drag = 0f;
        }
    }

    private void FixedUpdate(){
        MovePlayer();
    }

    private void MyInput(){
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // When to Jump
        if(Input.GetKey(jumpKey) && readyToJump && grounded){
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
        // When to Crouch
        //if(Input.GetKeyDown(crouchKey)){
            //transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            //rb.AddForce(Vector3.down * crouchForce, ForceMode.Impulse);
        //}

        // Stop Crouching
        if (Input.GetKeyUp(crouchKey)){
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    private void StateHandler(){
        // Mode - Sliding
        if (sliding){
            state = MovementState.sliding;

            if(OnSlope() && rb.velocity.y < 0.1f){
                desiredMoveSpeed = slideSpeed;
            }
            else {
                //desiredMoveSpeed = sprintSpeed;
            }
        }

        // Mode - Crouching
        else if(Input.GetKey(crouchKey)){
            state = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;
        }

        // Mode - Sprinting
        //else if(grounded && Input.GetKey(sprintKey)){
        //    state = MovementState.sprinting;
        //    desiredMoveSpeed = sprintSpeed;
        //}

        // Mode - Walking
        else if(grounded){
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }

        // Mode - Air
        else{
            state = MovementState.air;
        }

        // check if desiredMoveSpeed has changed drastically
        if(Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f){
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else{
            moveSpeed = desiredMoveSpeed;
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
    }

    private IEnumerator SmoothlyLerpMoveSpeed(){
        // smoothly lerp movementSpeed to desired value
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference){
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

            if (OnSlope()){
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
            }
            else{
                time += Time.deltaTime * speedIncreaseMultiplier;
            }
            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
    }

    private void MovePlayer(){
        // Calculate Movement Direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        // On a Slope
        if(OnSlope() && !exitingSlope){
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);

            if(rb.velocity.y > 0){
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        // On Gournd
        else if(grounded){
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        // In the Air
        else if(!grounded){
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }

        // Turn off Gravity While on Slope
        rb.useGravity = !OnSlope(); 
    }

    private void SpeedControl(){

        // Limiting Speed on Slopes
        if(OnSlope() && !exitingSlope){
            if(rb.velocity.magnitude > moveSpeed){
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }
        // Limitting Speed In Air or On the Ground
        else{
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // Limit Velocity if Needed
            if(flatVel.magnitude > moveSpeed){
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump(){
        exitingSlope = true;

        // Reset y Velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump(){  
        readyToJump = true;

        exitingSlope = false;
    }

    public bool OnSlope(){
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f)){
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction){
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }
}
