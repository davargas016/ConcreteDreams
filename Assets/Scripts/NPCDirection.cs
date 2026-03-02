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

        animator.SetBool(MovingHash, false);

        Vector2 dir = FacingToVector(startFacing);
        animator.SetFloat(MoveXHash, dir.x);
        animator.SetFloat(MoveYHash, dir.y);

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
