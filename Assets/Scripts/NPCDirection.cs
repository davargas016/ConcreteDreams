using UnityEngine;

public class NPCDirection : MonoBehaviour
{
    public enum Facing { Down, Right, Up, Left }

    [SerializeField] private Facing startFacing = Facing.Down;
    [SerializeField] private Animator animator;

    private static readonly int MoveXHash = Animator.StringToHash("moveX");
    private static readonly int MoveYHash = Animator.StringToHash("moveY");
    private static readonly int MovingHash = Animator.StringToHash("moving");

    private void OnEnable()
    {
        if (!animator) animator = GetComponent<Animator>();

        // Ensure we're in idle
        animator.SetBool(MovingHash, false);

        // Set blend-tree direction inputs
        Vector2 dir = FacingToVector(startFacing);
        animator.SetFloat(MoveXHash, dir.x);
        animator.SetFloat(MoveYHash, dir.y);

        // Optional: make it visually correct immediately
        animator.Update(0f);
    }

    private Vector2 FacingToVector(Facing f)
    {
        switch (f)
        {
            case Facing.Right: return Vector2.right;
            case Facing.Left:  return Vector2.left;
            case Facing.Up:    return Vector2.up;
            default:           return Vector2.down;
        }
    }
}
