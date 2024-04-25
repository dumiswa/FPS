using UnityEngine;
public class MovementScript : MonoBehaviour
{
    Camera camera;
    Rigidbody rb;

    [Header("Ground movement and jump settings")]
    public float speed;
    public float groundDrag;
    public float airFriction;
    public float jumpForce;
    public LayerMask whatIsGround;
    public bool isGrounded;
    public bool isMoving;

    public Vector3 move;

    public float height;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        camera = Camera.main;
    }

    void Update()
    {
        JumpAndDrag();
        SpeedControl();
    }

    void FixedUpdate()
    {
        GroundMovement();

        if(move.magnitude > 0.1f)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
    }

    void GroundMovement()
    {    

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        move = transform.right * x + transform.forward * z;

        rb.AddForce(move.normalized * speed * 10f, ForceMode.Force);

        if (isGrounded)
        {
            rb.AddForce(move.normalized * speed * 10f, ForceMode.Force);      
        }

        else
        {
            rb.AddForce(move.normalized * speed * 10f * airFriction, ForceMode.Force);          
        }

    }

    void SpeedControl()
    {
        Vector3 XZVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (XZVelocity.magnitude > speed)
        {
            Vector3 normalizedVelocity = XZVelocity.normalized * speed;
            rb.velocity = new Vector3(normalizedVelocity.x, rb.velocity.y, normalizedVelocity.z);
        }
    }

    void JumpAndDrag()
    {

        isGrounded = Physics.Raycast(transform.position, Vector3.down, height, whatIsGround);

        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }
}
