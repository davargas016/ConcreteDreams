using UnityEngine;

[ExecuteAlways]
public class LockUIScale : MonoBehaviour
{
    [SerializeField] private Vector3 lockedScale = Vector3.one;

    void Awake()
    {
        transform.localScale = lockedScale;
    }

    void OnEnable()
    {
        transform.localScale = lockedScale;
    }

    void LateUpdate()
    {
        if (transform.localScale != lockedScale)
        {
            transform.localScale = lockedScale;
        }
    }

#if UNITY_EDITOR
    void Update()
    {
        if (!Application.isPlaying)
        {
            if (transform.localScale != lockedScale)
                transform.localScale = lockedScale;
        }
    }
#endif
}