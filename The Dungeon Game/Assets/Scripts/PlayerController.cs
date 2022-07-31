using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // VARIABLES

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float sprintSpeed;

    private Vector3 moveDir;
    private Vector3 velocity;
    [SerializeField]
    private float gravity;

    [SerializeField]
    private bool isGrounded;
    [SerializeField]
    private float groundCheckDistance;
    [SerializeField]
    private LayerMask groundMask;

    [SerializeField]
    private float jumpHeight;

    // REFERENCES

    private CharacterController charController;
    private Animator anim;
    
    

    // Start is called before the first frame update
    void Start()
    {
        charController = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        moveSpeed = runSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if (Input.GetMouseButtonDown(0))
            Attack();
        else if (Input.GetMouseButtonDown(1))
            Debug.Log("Pressed secondary button."); // shield
        else if (Input.GetMouseButtonDown(2))
            Debug.Log("Pressed middle click."); // switch to staff
    }

    void Move()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if (isGrounded)
        {
            float moveVertical = Input.GetAxis("Vertical"); // Z axis
            float moveHorizontal = Input.GetAxis("Horizontal"); // X axis

            moveDir = new Vector3(moveHorizontal, 0, moveVertical);
        
            if (moveDir != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
                Walk();

            else if (moveDir != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
                Sprint();

            else if (moveDir == Vector3.zero)
                Idle();

            if (Input.GetKey(KeyCode.Space))
                Jump();

            moveDir *= moveSpeed;
        }

        charController.Move(moveDir * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        charController.Move(velocity * Time.deltaTime);
    }
    
    void Idle()
    {
        anim.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
    }

    void Walk()
    {
        moveSpeed = runSpeed;
        anim.SetFloat("Speed", 0.667f, 0.1f, Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            moveSpeed = walkSpeed;
            anim.SetFloat("Speed", 0.334f, 0.1f, Time.deltaTime);
        }
    }

    void Sprint()
    {
        moveSpeed = sprintSpeed;
        anim.SetFloat("Speed", 1, 0.1f, Time.deltaTime);
    }

    void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
    }

    void Attack()
    {
        anim.SetTrigger("Attack");
    }

}
