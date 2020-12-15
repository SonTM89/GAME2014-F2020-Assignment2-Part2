using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualGuyBehaviour : MonoBehaviour
{
    public float runForce;
    public Rigidbody2D rigidbody;
    public Transform lookInFrontPoint;
    public Transform lookAheadPoint;
    public LayerMask collisionWallLayer;
    public LayerMask collisionGroundLayer;
    public bool isGroundAhead;
    public bool onRamp;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _LookInFront();
        _LookAhead();
        _Move();
    }


    private void _LookInFront()
    {
        var wallHit = Physics2D.Linecast(transform.position, lookInFrontPoint.position, collisionWallLayer);

        if (wallHit)
        {
            _FlipX();
        }

        Debug.DrawLine(transform.position, lookInFrontPoint.position, Color.red);
    }


    private void _LookAhead()
    {
        isGroundAhead = Physics2D.Linecast(transform.position, lookAheadPoint.position, collisionGroundLayer);

        Debug.DrawLine(transform.position, lookAheadPoint.position, Color.green);
    }


    private void _Move()
    {
        if (isGroundAhead)
        {
            rigidbody.AddForce(Vector2.right * runForce * Time.deltaTime * transform.localScale.x);

            rigidbody.velocity *= 0.90f;
        }
        else
        {
            _FlipX();
        }

    }


    private void _FlipX()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1.0f, transform.localScale.y, transform.localScale.z);
    }
}
