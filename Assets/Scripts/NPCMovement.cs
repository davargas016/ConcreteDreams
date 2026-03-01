using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class NPCMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 2f;
    public float arriveDistance = 0.06f;

    [Header("Patrol Points")]
    public Transform[] patrolPoints;
    public Transform pointA; // fallback if patrolPoints empty
    public Transform pointB;

    [Header("Patrol Behaviour")]
    public bool pingPong = true;
    public bool waitAtPoints = true;
    public float waitMin = 0.5f;
    public float waitMax = 2.0f;

    [Header("Animator Parameter Names")]
    public string paramMoveX = "moveX";
    public string paramMoveY = "moveY";
    public string paramMoving = "moving";

    [Header("Waypoint Options")]
    public bool cacheWaypointPositions = true; // set true if waypoints are children of NPC

    private Rigidbody2D rb;
    private Animator animator;
    private int moveXHash, moveYHash, movingHash;

    private Vector2[] cachedPositions;
    private Vector2 destination;
    private bool hasDestination;
    private Vector2 lastFacing = Vector2.down;
    private Vector2 currentVelocity;

    private int currentIndex;
    private int direction = 1;
    private Coroutine waitCoroutine;

    private float arriveDistanceSqr => arriveDistance * arriveDistance;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        moveXHash = Animator.StringToHash(paramMoveX);
        moveYHash = Animator.StringToHash(paramMoveY);
        movingHash = Animator.StringToHash(paramMoving);

        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    void Start()
    {
        if ((patrolPoints == null || patrolPoints.Length == 0) && pointA != null && pointB != null)
        {
            patrolPoints = new Transform[] { pointA, pointB };
        }

        if (cacheWaypointPositions && patrolPoints != null && patrolPoints.Length > 0)
        {
            cachedPositions = new Vector2[patrolPoints.Length];
            for (int i = 0; i < patrolPoints.Length; i++)
                cachedPositions[i] = patrolPoints[i] ? (Vector2)patrolPoints[i].position : (Vector2)transform.position;
        }

        SetupInitialPatrol();
    }

    void FixedUpdate()
    {
        if (!hasDestination)
        {
            currentVelocity = Vector2.zero;
            UpdateAnimator(Vector2.zero);
            return;
        }

        Vector2 pos = rb.position;
        Vector2 toTarget = destination - pos;
        float distSqr = toTarget.sqrMagnitude;

        if (distSqr <= arriveDistanceSqr)
        {
            rb.MovePosition(destination);
            hasDestination = false;
            currentVelocity = Vector2.zero;
            UpdateAnimator(Vector2.zero);

            if (waitAtPoints)
            {
                if (waitCoroutine != null) StopCoroutine(waitCoroutine);
                waitCoroutine = StartCoroutine(WaitThenGoNext());
            }
            else
            {
                AdvanceToNextPoint();
            }
            return;
        }

        Vector2 dir = toTarget.normalized;
        currentVelocity = dir * speed;
        Vector2 nextPos = pos + currentVelocity * Time.fixedDeltaTime;
        rb.MovePosition(nextPos);
        UpdateAnimator(currentVelocity);
    }

    public void SetDestination(Vector2 worldPos)
    {
        destination = worldPos;
        hasDestination = true;
        if (waitCoroutine != null) { StopCoroutine(waitCoroutine); waitCoroutine = null; }
    }

    public void Stop()
    {
        hasDestination = false;
        currentVelocity = Vector2.zero;
        UpdateAnimator(Vector2.zero);
        if (waitCoroutine != null) { StopCoroutine(waitCoroutine); waitCoroutine = null; }
    }

    public void StartPatrol()
    {
        SetupInitialPatrol();
    }

    public void SetPatrolPoints(Transform[] points, bool usePingPong = true)
    {
        patrolPoints = points;
        pingPong = usePingPong;
        if (cacheWaypointPositions && patrolPoints != null && patrolPoints.Length > 0)
        {
            cachedPositions = new Vector2[patrolPoints.Length];
            for (int i = 0; i < patrolPoints.Length; i++)
                cachedPositions[i] = patrolPoints[i] ? (Vector2)patrolPoints[i].position : (Vector2)transform.position;
        }
        SetupInitialPatrol();
    }

    private IEnumerator WaitThenGoNext()
    {
        UpdateAnimator(Vector2.zero);
        yield return new WaitForSeconds(Random.Range(waitMin, waitMax));
        waitCoroutine = null;
        AdvanceToNextPoint();
    }

    private void SetupInitialPatrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            hasDestination = false;
            return;
        }

        currentIndex = 0;
        direction = 1;
        destination = GetPointWorldPos(currentIndex);
        hasDestination = true;
    }

    private void AdvanceToNextPoint()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            hasDestination = false;
            return;
        }

        int lastIndex = patrolPoints.Length - 1;

        if (pingPong)
        {
            if (currentIndex == 0) direction = 1;
            else if (currentIndex == lastIndex) direction = -1;

            currentIndex += direction;
            currentIndex = Mathf.Clamp(currentIndex, 0, lastIndex);
        }
        else
        {
            currentIndex = (currentIndex + 1) % patrolPoints.Length;
        }

        destination = GetPointWorldPos(currentIndex);
        hasDestination = true;
    }

    private Vector2 GetPointWorldPos(int index)
    {
        if (cachedPositions != null && index >= 0 && index < cachedPositions.Length)
            return cachedPositions[index];

        if (patrolPoints != null && index >= 0 && index < patrolPoints.Length && patrolPoints[index] != null)
            return (Vector2)patrolPoints[index].position;

        return rb.position;
    }

    private void UpdateAnimator(Vector2 velocity)
    {
        if (animator == null) return;

        float speedVal = velocity.magnitude;
        bool moving = speedVal > 0.001f;
        animator.SetBool(movingHash, moving);

        if (moving)
        {
            Vector2 face = Pick4Dir(velocity);
            lastFacing = face;
            animator.SetFloat(moveXHash, face.x);
            animator.SetFloat(moveYHash, face.y);
        }
        else
        {
            animator.SetFloat(moveXHash, lastFacing.x);
            animator.SetFloat(moveYHash, lastFacing.y);
        }
    }

    private Vector2 Pick4Dir(Vector2 v)
    {
        if (Mathf.Abs(v.x) > Mathf.Abs(v.y)) return (v.x >= 0f) ? Vector2.right : Vector2.left;
        return (v.y >= 0f) ? Vector2.up : Vector2.down;
    }
}
