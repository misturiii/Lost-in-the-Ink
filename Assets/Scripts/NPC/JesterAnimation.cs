using UnityEngine;

public delegate void AnimationTrigger();

public class JesterAnimation : MonoBehaviour
{
    public static AnimationTrigger exit;
    public static AnimationTrigger enter;
    public static AnimationTrigger talk;
    public static AnimationTrigger clap;

    Animator animator;
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();   
        exit = Exit;
        enter = Enter;
        talk = Talk;
        clap = Clap;
    }

    void Exit () {
        animator.ResetTrigger("Enter");
        animator.ResetTrigger("Talk");
        animator.ResetTrigger("Clap");
        animator.SetTrigger("Exit");
    }

    void Enter () {
        animator.ResetTrigger("Exit");
        animator.SetTrigger("Enter");
    }
    
    void Clap () {
        animator.ResetTrigger("Talk");
        animator.SetTrigger("Clap");
    }

    void Talk () {
        animator.SetTrigger("Talk");
        animator.ResetTrigger("Enter");
    }

    void OnDestory () {
        exit -= Exit;
        enter -= Enter;
        talk -= Talk;
        clap -= Clap;
    }
}
