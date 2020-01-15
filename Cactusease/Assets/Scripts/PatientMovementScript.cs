using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientMovementScript : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed = 0.03f;
    private int dir = 1;

    [SerializeField]
    private Transform wallCheck;
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private float wallRadious = 0f;

    [SerializeField]
    private LayerMask ground;

    [SerializeField]
    private float pausePeriod = 3f;
    private float waitTime = 0f;

    bool wait = false;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(MovementCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    private void FixedUpdate()
    {
        //Walking
        Collider2D[] colliders = Physics2D.OverlapCircleAll(wallCheck.position, wallRadious, ground);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                wait = true;
                ChangeDirection();
            }
        }
        colliders = Physics2D.OverlapCircleAll(groundCheck.position, wallRadious, ground);
        bool isCollided = false;
        for (int i = 0; i < colliders.Length; ++i)
        {
            if (colliders[i].gameObject != gameObject)
            {
                isCollided = true;
                break;
            }
        }
        if (!isCollided)
        {
            ChangeDirection();
            return;
        }
        Vector3 movement = new Vector3(walkSpeed * Time.fixedDeltaTime, 0f, 0f);
        transform.position += walkSpeed * dir * movement * Time.fixedDeltaTime;
    }

    private void ChangeDirection()
    {
        dir *= -1;
        //Flip the character
        Vector2 characterScale = transform.localScale;
        if (dir < 0)
        {
            characterScale.x = -1 * Mathf.Abs(transform.localScale.x);
        }
        if (dir > 0)
        {
            characterScale.x = Mathf.Abs(transform.localScale.x);
        }

        transform.localScale = characterScale;
    }

    //public IEnumerator MovementCoroutine()
    //{ 
    //    if (wait)
    //    {
    //        wait = false;
    //        yield return new WaitForSeconds(pausePeriod);
    //    }
    //}
}
