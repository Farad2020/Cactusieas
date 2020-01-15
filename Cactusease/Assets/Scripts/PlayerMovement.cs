using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    Rigidbody2D rb;
    public Animator animator;
    public float runSpeed;

    [Range(1,20)]
    public float jumpVelocity;
    public float fallMultiplier;
    public float lowJumpMultiplier;

    [SerializeField]
    private Transform[] groundPoints;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private LayerMask whatIsGround;




    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // When falling down
        if (rb.velocity.y < 0) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
            animator.SetBool("Landing",true);
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    private void FixedUpdate()
    {

        //Walking
        float move = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(runSpeed * move * Time.fixedDeltaTime, 0f, 0f);
        transform.position += runSpeed * movement * Time.fixedDeltaTime;
        //rb.velocity = new Vector2(runSpeed * move * Time.fixedDeltaTime, rb.velocity.y);

        animator.SetFloat("Speed", Mathf.Abs(move));

        //Jumping
        if (Input.GetButtonDown("Jump") && IsGrounded()) 
        {
            rb.velocity = Vector2.up * jumpVelocity;
            animator.SetTrigger("Jump");
        }

        HandleLayers();

        //Flip

        Vector2 characterScale = transform.localScale;
        if (Input.GetAxis("Horizontal") < 0) {
            characterScale.x = -1 * Mathf.Abs(transform.localScale.x);
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            characterScale.x = Mathf.Abs(transform.localScale.x);
        }

        transform.localScale = characterScale;
    }

    private bool IsGrounded() {
        if (rb.velocity.y <= 0) {
            foreach (Transform point in groundPoints) {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);
                for (int i = 0; i < colliders.Length; i++) {
                    if (colliders[i].gameObject != gameObject)
                    {
                        animator.ResetTrigger("Jump");
                        animator.SetBool("Landing", false);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void HandleLayers() {
        if (!IsGrounded())
        {
            animator.SetLayerWeight(1, 1);
        }
        else
        {
            animator.SetLayerWeight(1, 0);
        }
    }
}