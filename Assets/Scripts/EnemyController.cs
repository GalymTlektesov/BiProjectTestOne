﻿using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animation;
    public Transform Eyes;
    public Transform alertEyes;
    public Transform Front;
    
    public LayerMask mask;
    public float speed;
    public float distance = 7;
    public bool isMotion;
    public bool isAlert;
    private float maxDistance;


    Ray2D ray;
    RaycastHit2D hit;
    Vector2 direction;

    void Start()
    {
        maxDistance = distance * 2;
        rb = GetComponent<Rigidbody2D>();
        animation = GetComponent<Animator>();
        alertEyes.position = new Vector2(Eyes.position.x + distance, Eyes.position.y);
        Flip();
    }

    
    void Update()
    {
        if (!isMotion)
        {
            animation.SetInteger("State", 0);
        }
        if (isAlert)
        {
            VisibilityOverview(maxDistance, alertEyes.position);
            Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red);
            Debug.Log("Alert");
        }
        else
        {
            VisibilityOverview(distance, Eyes.position);
            Debug.DrawRay(ray.origin, ray.direction * distance, Color.black);
            Debug.Log("No Alert");
        }

        if (hit)
        {
            var player = hit.collider.GetComponent<PlayerController>();
            if (player.Popularity > 25)
            {
                isAlert = true;
                Flip();
                if (player.Popularity > 50)
                {
                    isMotion = true;
                    if (Mathf.Abs(hit.point.x - transform.position.x) > 1f)
                    {
                        Debug.Log(Mathf.Abs(hit.point.x - transform.position.x));
                        rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);
                        animation.SetInteger("State", 1);
                        //transform.position = Vector2.MoveTowards(transform.position, hit.point, speed);
                    }
                    else
                    {
                        animation.SetInteger("State", 2);
                    }
                }
                else
                {
                    isMotion = false;
                }
            }
        }
        else
        {
            isAlert = false;
            isMotion = false;
            VisibilityOverview(distance, Eyes.position);
        }

    }

    private void Flip()
    {
        if (hit.point.x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            direction = Vector2.right;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            direction = Vector2.left;
        }
    }

    private void VisibilityOverview(float distanceRay, Vector2 eyes)
    {
        ray = new Ray2D(eyes, direction);
        hit = Physics2D.Raycast(ray.origin, ray.direction, distanceRay, mask);
    }
}
