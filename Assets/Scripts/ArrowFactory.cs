using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ArrowFactory : MonoBehaviour
{
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
        selectedTargetIndex = 0;
        StartCoroutine(fireArrows());
    }
    void Update()
    {
        Vector3 target = GetTarget();
        Vector3 offset = target-transform.position;
        float angle = Mathf.Atan2(offset.y, offset.x) - Mathf.PI/2;
        transform.rotation = Quaternion.Euler(0, 0, angle*180f/Mathf.PI);
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
        Vector3 target = GetTarget(); 
        Vector3 offset = target - transform.position;
        float angle = Mathf.Atan2(offset.y, offset.x) - Mathf.PI/2;
        GameObject newArrow = Instantiate(arrowPrefab, transform.position, Quaternion.Euler(0,0, angle * (180/Mathf.PI)));
        ArrowScript newArrowScript = newArrow.GetComponent<ArrowScript>();
        newArrow.transform.parent = transform.parent;
        newArrowScript.arrowSpeed = arrowSpeed;
        newArrowScript.direction = offset.normalized;
        newArrowScript.GetComponent<Rigidbody2D>().AddForce(offset.normalized * arrowSpeed);
    }

    private Vector3 GetTarget()
    {
        return GetTargetClosest();
        // return GetTargetNext();
    }


    private Vector3 GetTargetClosest()
    {
        Vector3[] targets = GameObject.FindGameObjectsWithTag("Target").Select(x => x.transform.position).ToArray();
        return targets.Aggregate((acc, val) => {
            float currentDistance = (acc-transform.position).sqrMagnitude;
            float newDistance = (val-transform.position).sqrMagnitude;
            return newDistance < currentDistance ? val : acc;
        });
    }

    private int selectedTargetIndex;
    private Vector3 GetTargetNext()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");
        selectedTargetIndex++;
        if (selectedTargetIndex >= targets.Length) selectedTargetIndex = 0;
        return targets[selectedTargetIndex].transform.position;
    }

}
