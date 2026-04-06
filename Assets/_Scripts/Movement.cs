using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float sprintSpeed = 15f;
    [SerializeField] private float maxVelocityChange = 10f;

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float extraGravity = 10f;

    private Vector2 input;
    private Rigidbody rb;
    private bool isGrounded;
    private bool isSprinting;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        isSprinting = Input.GetKey(KeyCode.LeftShift);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(Vector3.down * extraGravity, ForceMode.Acceleration);
        }
    }

    void FixedUpdate()
    {
        rb.AddForce(CalculateMovement(), ForceMode.VelocityChange);

        isGrounded = false;
    }

    private Vector3 CalculateMovement()
    {
        float _speedToUse = isSprinting ? sprintSpeed : walkSpeed;

        Vector3 targetVelocity = transform.TransformDirection(new Vector3(input.x, 0f, input.y)) * _speedToUse;
        Vector3 velocityChange = targetVelocity - rb.linearVelocity;

        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0f;

        if (input.magnitude < 0.5f)
        {
            return new Vector3(-rb.linearVelocity.x, 0f, -rb.linearVelocity.z);
        }
        else
        {
            return velocityChange;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }
}
