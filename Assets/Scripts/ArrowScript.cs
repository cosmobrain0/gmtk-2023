using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public GameObject target;
    public float arrowSpeed;
    private Vector3 direction;
    private Rigidbody2D rb;
    private int archerTriggerCounter = 0;
    private void Awake()
    {
        target = FindObjectOfType<TargetControls>().gameObject;
        direction = (target.transform.position - transform.position).normalized;
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(direction * arrowSpeed);
    }
    private void FixedUpdate()
    {
        CheckIfInScreen();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) - Mathf.PI/2;
        transform.rotation = Quaternion.Euler(0f,0f, angle * (180/Mathf.PI));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        archerTriggerCounter++;
        //Because trigger occurs with archer when arrow spawns, so we want the second instance of trigger
        if (archerTriggerCounter == 2)
        {
            Debug.Log("Hit archer!");
            //TODO: Switch level/reduce archer HP
        }
        
    }
    private void CheckIfInScreen()
    {
        Vector3 bounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));
        if (Mathf.Abs(transform.position.x) > bounds.x || Mathf.Abs(transform.position.y) > bounds.y)
        {
            Destroy(gameObject);
        }
    }
}
