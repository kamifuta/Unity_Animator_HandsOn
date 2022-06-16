using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbody;

    private Vector3 MoveVec => new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
    private bool PushedJumpButton => Input.GetKeyDown(KeyCode.Space);

    private readonly Vector3 JumpPower = new Vector3(0, 10f, 0);

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
    }

    private void Move()
    {
        rigidbody.velocity = MoveVec;
    }

    private void LookAtMoveDirectin()
    {
        transform.LookAt(transform.position + rigidbody.velocity);
    }

    private void Jump()
    {
        rigidbody.AddForce(JumpPower, ForceMode.Impulse);
    }
}
