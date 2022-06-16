using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbody;

    private Vector3 MoveVec => new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        LookAtMoveDirectin();
    }

    private void Move()
    {
        rigidbody.velocity = MoveVec;
    }

    private void LookAtMoveDirectin()
    {
        transform.LookAt(transform.position + rigidbody.velocity);
    }
}
