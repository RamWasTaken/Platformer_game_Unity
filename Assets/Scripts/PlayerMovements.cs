using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;
    private SpriteRenderer sprite;

    public Joystick joystick;

    [SerializeField] private LayerMask jumpableGround;
    private float dirX = 0f;
    private float dirY = 0f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;

    private enum MovementState { idle, running, jumping, falling}

    [SerializeField] private AudioSource JumpsoundEffect;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        //dirX = Input.GetAxisRaw("Horizontal");
        dirX = joystick.Horizontal;
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        /* if (Input.GetButtonDown("Jump") && isGrounded())
          {
              JumpsoundEffect.Play();
              anim.SetInteger("state", 2);
              rb.velocity = new Vector2(rb.velocity.x, jumpForce);
          }*/
        dirY = joystick.Vertical;
        if (dirY > 0.5f && isGrounded())
        {
            JumpsoundEffect.Play();
            anim.SetInteger("state", 2);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        UpdateAnimationState();
    }
    private void UpdateAnimationState()
    {
        MovementState state;
        if (dirX > 0f) //we can use X velocity , but if any other means changges our X velocity then it will also show us running.
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }
        if(rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if(rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);
    }
    private bool isGrounded()
    {
        return  Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}