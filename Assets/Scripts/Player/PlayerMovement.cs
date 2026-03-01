using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    private Rigidbody2D myRigidBody;
    private Vector2 change;
    private Animator animator;

    [HideInInspector] public bool canMove = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!canMove)
        {
            change = Vector2.zero;
            animator.SetBool("moving", false);
            return;
        }

        change = Vector2.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");

        UpdateAnimation();
    }

    void FixedUpdate()
    {
        MoveCharacter();
    }

    void UpdateAnimation()
    {
        if (change != Vector2.zero)
        {
            animator.SetFloat("moveX", change.x);
            animator.SetFloat("moveY", change.y);
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }
    }

    void MoveCharacter()
    {
        if (!canMove)
        {
            myRigidBody.linearVelocity = Vector2.zero;
            return;
        }

        myRigidBody.linearVelocity = change.normalized * speed;
    }
}