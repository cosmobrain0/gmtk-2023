using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowFactory : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] float arrowSpeed;
    void Start()
    {
        StartCoroutine(fireArrows());
    }
    private IEnumerator fireArrows()
    {
        while (true){
            createArrow();
            yield return new WaitForSeconds(2f);
        }
        
    }
    private void createArrow()
    {
        Vector3 offset = target.transform.position - transform.position;
        float angle = Mathf.Atan2(offset.y, offset.x) - Mathf.PI/2;
        GameObject newArrow = Instantiate(arrowPrefab, transform.position, Quaternion.Euler(0,0, angle * (180/Mathf.PI)));
        ArrowScript newArrowScript = newArrow.GetComponent<ArrowScript>();
        newArrowScript.arrowSpeed = arrowSpeed;
    }
}
