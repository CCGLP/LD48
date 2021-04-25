using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ChangeHeartRotation()); 
    }


    private IEnumerator ChangeHeartRotation()
    {
        yield return new WaitForSeconds(0.4f); 
        while (true)
        {
            var heartList = GenerationController.Instance.hearts;
            Debug.Log(heartList.Count); 
            float distance = 999999;
            Vector3 pos = Vector3.zero; 
            for (int i = 0; i< heartList.Count; i++)
            {
                 float d = Mathf.Abs(Vector3.Distance(heartList[i].transform.position, this.transform.parent.position));
                 if (distance > d)
                 {
                    distance = d; 
                    pos = heartList[i].transform.position; 
                 }
            }

            this.transform.LookAt(pos);
            yield return new WaitForSeconds(1); 
        }
        yield return null; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
