using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private CapsuleCollider collider;
    [SerializeField] private GameObject swordObject;
    [SerializeField] private Animator animator;

    [SerializeField] private AnimationCurve curve;

    private Vector3 MoveVec => new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
    private bool PushedJumpButton => Input.GetKeyDown(KeyCode.Space);
    private bool PushingJumpButton => Input.GetKey(KeyCode.Space);
    private bool PushedWeaponButton => Input.GetKeyDown(KeyCode.E);
    private bool PushingDashButton => Input.GetKey(KeyCode.LeftShift);

    private bool InMidAir => !CheckIsGround();
    private bool UsingWeapon;
    private bool IsJumpAnimation => animator.GetCurrentAnimatorStateInfo(0).IsTag("Jump");

    private const float WalkSpeed = 1f;
    private const float DashSpeed = 1.5f;
    private const float BaseJumpPower = 10f;

    private readonly int groundLayerMask = 1 << 6;

    // Update is called once per frame
    void Update()
    {
        Move();
        PlayMoveAnimation();
        LookAtMoveDirectin();
        if (!PushingDashButton)
            LookAtDefaultDirection();

        if (PushedJumpButton && !IsJumpAnimation)
            StartCoroutine(JumpCoroutine());

        if (PushedWeaponButton)
        {
            if (UsingWeapon)
                RemoveSowrd();
            else
                UseSword();
        }
    }

    private void Move()
    {
        if (Mathf.Approximately(Vector3.Magnitude(MoveVec), 0))
        {
            StopWalkAnimation();
            return;
        }

        var moveSpeed = PushingDashButton ? DashSpeed : WalkSpeed;
        var moveVec = MoveVec * moveSpeed;
        if (InMidAir)
            moveVec.y = rigidbody.velocity.y;
        rigidbody.velocity = moveVec;
        PlayWalkAnimation();
    }

    private void PlayWalkAnimation()
    {
        //animator.SetBool("IsWalking", true);
    }

    private void StopWalkAnimation()
    {
        //animator.SetBool("IsWalking", false);
    }

    private void PlayMoveAnimation()
    {
        animator.SetBool("Dash", PushingDashButton);

        animator.SetFloat("MoveFront", rigidbody.velocity.z);
        animator.SetFloat("MoveRight", rigidbody.velocity.x);
    }

    private void LookAtMoveDirectin()
    {
        if (!PushingDashButton) return;
        var lookDir = rigidbody.velocity;
        lookDir.y = 0;
        transform.LookAt(transform.position + lookDir);
    }

    private void LookAtDefaultDirection()
    {
        transform.LookAt(transform.position+Vector3.forward);
    }

    private IEnumerator JumpCoroutine()
    {
        float time = 0;
        while (true)
        {
            yield return null;
            time += Time.deltaTime;
            if (!PushingJumpButton) break;
            if (time >= 0.5f) break;
        }

        Jump(time);
    }

    private void Jump(float jumpPowerRate)
    {
        if (InMidAir) return;
        rigidbody.AddForce(Vector3.up*jumpPowerRate*BaseJumpPower, ForceMode.Impulse);
        PlayJumpAnimation(jumpPowerRate);
    }

    private void PlayJumpAnimation(float jumpPowerRate)
    {
        animator.SetTrigger("Jump");
        animator.SetFloat("JumpPower", jumpPowerRate);
    }

    private bool CheckIsGround()
    {
        Vector3 centerPos = transform.position + new Vector3(0, collider.radius, 0);
        float radius = collider.radius - 0.01f;
        Ray ray = new Ray(centerPos, Vector3.down);

        return Physics.SphereCast(ray, radius, 0.01f, groundLayerMask);
    }

    private void UseSword()
    {
        swordObject.SetActive(true);
        animator.SetBool("UsingWeapon", true);
        UsingWeapon = true;
    }

    private void RemoveSowrd()
    {
        swordObject.SetActive(false);
        animator.SetBool("UsingWeapon", false);
        UsingWeapon = false;
    }

    public void PrintJump()
    {
        Debug.Log("�W�����v����");
    }
}
