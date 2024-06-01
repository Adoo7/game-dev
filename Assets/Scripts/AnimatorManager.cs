using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    private Animator animator;
    private int horizontal;
    private int vertical;

    private void Awake()
    {
        animator   = GetComponent<Animator>();
        horizontal = Animator.StringToHash("Horizontal");
        vertical   = Animator.StringToHash("Vertical");
    }

    public void UpdateAnimationVelocity(float x, float y)
    {
        animator.SetFloat(horizontal, x, 0.1f, Time.deltaTime);
        animator.SetFloat(vertical,   y, 0.1f, Time.deltaTime);
    }
}