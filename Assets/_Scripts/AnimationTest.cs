using UnityEngine;

public class AnimationTest : MonoBehaviour
{
    private Animator animator;
    [Range(-2, 2)]
    public float horizontal, vertical;
    public bool jump;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);

        if (jump)
        {
            jump = false;
            animator.SetTrigger("Jump");
        }
    }
}
