using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private CapsuleCollider collider;
    [SerializeField] private GameObject swordObject;

    private Vector3 MoveVec => new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
    private bool PushedJumpButton => Input.GetKeyDown(KeyCode.Space);
    private bool PushedWeaponButton => Input.GetKeyDown(KeyCode.E);
    private bool PushingDashButton => Input.GetKey(KeyCode.LeftShift);

    private bool InMidAre => !CheckIsGround();
    private bool UsingSword;

    private const float WalkSpeed = 1f;
    private const float DashSpeed = 1.5f;

    private readonly Vector3 JumpPower = new Vector3(0, 10f, 0);
    private readonly int groundLayerMask = 1 << 6;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        LookAtMoveDirectin();
        if (PushedJumpButton)
            Jump();

        if (PushedWeaponButton)
        {
            if (UsingSword)
                RemoveSowrd();
            else
                UseSword();
        }
    }

    private void Move()
    {
        var moveSpeed = PushingDashButton ? DashSpeed : WalkSpeed;
        rigidbody.velocity = MoveVec * moveSpeed;
    }

    private void LookAtMoveDirectin()
    {
        if (!PushingDashButton) return;
        transform.LookAt(transform.position + rigidbody.velocity);
    }

    private void Jump()
    {
        if (InMidAre) return;
        rigidbody.AddForce(JumpPower, ForceMode.Impulse);
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
        UsingSword = true;
    }

    private void RemoveSowrd()
    {
        swordObject.SetActive(false);
        UsingSword = false;
    }
}
