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
    [SerializeField] GameObject bow1;
    [SerializeField] GameObject bow2;
    [SerializeField] GameObject bow3;
    private float animationPause;
    void Start()
    {
        animationPause = fireCooldown / 3f;
        StartCoroutine(fireArrows()); 
    }
    private void Update()
    {
        transform.rotation = getAngleToTarget();
    }
    private IEnumerator fireArrows()
    {
        for (int i = 0; i < arrowCount; i++)
        {
            bow3.SetActive(false);
            bow1.SetActive(true);
            yield return new WaitForSeconds(animationPause);
            bow1.SetActive(false);
            bow2.SetActive(true);
            yield return new WaitForSeconds(animationPause);
            bow2.SetActive(false);
            bow3.SetActive(true);
            yield return new WaitForSeconds(animationPause);
            createArrow();
        }
        Debug.Log("Ran out of arrows!");
    }
    private void createArrow()
    {
        GameObject newArrow = Instantiate(arrowPrefab, bow3.transform.position, getAngleToTarget());
        arrowCount--;
        newArrow.transform.parent = transform;
        ArrowScript newArrowScript = newArrow.GetComponent<ArrowScript>();
        newArrowScript.arrowSpeed = arrowSpeed;
    }
    private Quaternion getAngleToTarget()
    {
        Vector3 offset = target.transform.position - transform.position;
        float angle = Mathf.Atan2(offset.y, offset.x) - Mathf.PI / 2;
        return Quaternion.Euler(0, 0, angle * (180 / Mathf.PI));
    }
}
