using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Vector2 moveDir = Vector2.zero;
    public float moveSpeed = 0f;

    public void FixedUpdate()
    {
        Vector2 newPos = transform.position;
        newPos += moveDir * moveSpeed * Time.fixedDeltaTime;
        transform.position = newPos;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        moveDir = Vector2.Reflect(moveDir, collision.contacts[0].normal);
    }
}
