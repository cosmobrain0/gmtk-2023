using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowFactory : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] float fireCooldown;
    [SerializeField] float arrowSpeed;
    [SerializeField] int arrowCount;
    void Start()
    {
        StartCoroutine(fireArrows());
    }
    private IEnumerator fireArrows()
    {
        for (int i = 0; i < arrowCount; i++)
        {
            createArrow();
            yield return new WaitForSeconds(fireCooldown);
        }
        Debug.Log("Ran out of arrows!");
    }
    private void createArrow()
    {
        Vector3 offset = target.transform.position - transform.position;
        float angle = Mathf.Atan2(offset.y, offset.x) - Mathf.PI/2;
        GameObject newArrow = Instantiate(arrowPrefab, transform.position, Quaternion.Euler(0,0, angle * (180/Mathf.PI)));
        newArrow.transform.parent = transform;
        ArrowScript newArrowScript = newArrow.GetComponent<ArrowScript>();
        newArrowScript.arrowSpeed = arrowSpeed;
    }
}
