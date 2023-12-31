using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public float arrowSpeed;
    public Vector3 direction;
    private int archerTriggerCounter = 0;
    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // rb.AddForce(direction * arrowSpeed);
    }
    private void FixedUpdate()
    {
        CheckIfInScreen();
        //transform.position += direction * Time.deltaTime * arrowSpeed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "Archer") { // I don't like string literals like this but it's a game jam (Me neither but its a game jam - Felix)
            archerTriggerCounter++;
            //Because trigger occurs with archer when arrow spawns, so we want the second instance of trigger
            if (archerTriggerCounter == 2)
            {
                Debug.Log("Hit archer!");
                FindObjectOfType<LevelManagement>().NextLevel();
            } 
        } else {
            // this maths is kinda dodgy,
            // but it'll work most of the time for thin objects
            // and should prevent the weird arrow glitch thing
            float angle = collision.gameObject.transform.rotation.eulerAngles.z*Mathf.PI/180f + Mathf.PI/2;
            Vector2 normal = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            Vector2 a = -rb.velocity; // the same `a` from that diagram
            // if (Vector2.Dot(normal, a) < 0f) normal *= -1f; // can't think of a reason why this would help but it does
            Vector2 c = normal * Vector2.Dot(a, normal) / Vector2.Dot(normal, normal);
            Vector2 newVelocity = a - (a - c) * 2f;
            transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, newVelocity));
            rb.velocity = newVelocity;
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

