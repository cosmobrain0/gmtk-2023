using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public GameObject target;
    public float arrowSpeed;
    private Vector3 direction;
    private void Awake()
    {
        target = GameObject.Find("Target");
        direction = target.transform.position - transform.position;
        direction = direction.normalized;
    }
    void FixedUpdate()
    {
        transform.position += direction * arrowSpeed;
    }
}
