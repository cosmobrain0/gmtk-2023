using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetControls: MonoBehaviour
{
    public float direction;
    public Vector3 bottomLeftBound;
    public Vector3 topRightBound;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        // TODO: decide if this should actually be based on screen size
        // or if there should be a fixed border size
        bottomLeftBound = Camera.main.ScreenToWorldPoint(new Vector2(0f, 0f));
        topRightBound = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(offset.y, offset.x);
        transform.rotation = Quaternion.Euler(0, 0, angle * 180f/Mathf.PI);

        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        transform.position += movement * speed * Time.deltaTime;
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, bottomLeftBound.x, topRightBound.x),
            Mathf.Clamp(transform.position.y, bottomLeftBound.y, topRightBound.y),
            0f
        );
    }
}
