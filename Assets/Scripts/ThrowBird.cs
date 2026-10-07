using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ThrowBird : MonoBehaviour
{
    public int force = 20;
    public bool isDragable = false;
    public bool isCursorInside;
    public bool triggerActivated = false;
    public Rigidbody2D rb;
    public Transform startPosition;
    public float maxPullDistance = 6f;
    private bool isInQueue = true;
    public event Action onStopped;
    private bool hasStopped = false;
    public bool isLaunched;
    public Animator animator;
    public SlingshotBand slingshotBand;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        startPosition = GameObject.Find("Rogatka/StartPosition").transform;
        animator = GetComponent<Animator>();
        animator.SetBool("isFlying", false);
        slingshotBand = GetComponent<SlingshotBand>();
    }

    void Update()
    {
        if (!isInQueue)
        {
            if (isDragable)
            {
                if (Input.GetMouseButton(0))
                {
                    Drag();
                    slingshotBand.UpdateBand();
                }
                else
                {
                    Launch();
                    animator.SetBool("isFlying", true);
                    slingshotBand.HideBand();
                    StartCoroutine(WaitOnStopped());
                }
            }
            else
            {
                if (isCursorInside && Input.GetMouseButtonDown(0))
                {
                    StartDragging();
                }
            }
            if (!hasStopped && (rb.linearVelocity.magnitude > -0.5f && rb.linearVelocity.magnitude < 0.5f) && isLaunched)
            {
                hasStopped = true;
                onStopped?.Invoke();
            }
        }
        else
        {
            
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("MouseHolder"))
        {
            Debug.Log(other.gameObject.name);
            isCursorInside = true;
            //transform.localScale *= 2f;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {

    }

    void OnTriggerExit2D(Collider2D other)
    {
        isCursorInside = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wood") || collision.gameObject.CompareTag("Pig") || collision.gameObject.CompareTag("TNT"))
            animator.SetBool("isFlying", false);
    }

    void StartDragging()
    {
        isDragable = true;
    }

    void Launch()
    {
        isDragable = false;
        isCursorInside = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
        Vector3 direction = transform.position - startPosition.position;
        rb.AddForce(direction * force * -1f, ForceMode2D.Impulse);
    }

    void Drag()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector3 direction = mousePos - startPosition.position;
        if (direction.magnitude > maxPullDistance)
        {
            direction = direction.normalized * maxPullDistance;
        }
        transform.position = startPosition.position + direction;
    }

    public void InQueue(bool state)
    {
        isInQueue = state;
    }

    private IEnumerator WaitOnStopped()
    {
        isLaunched = false;
        yield return new WaitForSeconds(1f);
        isLaunched = true;
    }
}
