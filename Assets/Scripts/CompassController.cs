using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening; 

public class CompassController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textDistance;

    private Renderer rend;

    private Color originalColor;
    private Vector3 target;
    [SerializeField]
    private AudioSource beatSound; 
    void Start()
    {
        rend = this.GetComponentInChildren<Renderer>();
        originalColor = rend.material.color; 
        StartCoroutine(ChangeHeartRotation()); 
    }


    private IEnumerator ChangeHeartRotation()
    {
        yield return new WaitForSeconds(0.4f); 
        while (true)
        {
            var heartList = GenerationController.Instance.hearts;
            if (heartList.Count > 0)
            {
                rend.material.color = originalColor;

                float distance = 999999;
                Vector3 pos = Vector3.zero;
                int auxIndex = -1; 
                for (int i = 0; i < heartList.Count; i++)
                {
                    if (heartList[i].transform.position.y < this.transform.parent.position.y)
                    {

                        float d = Mathf.Abs(Vector3.Distance(heartList[i].transform.position, this.transform.parent.position));
                        if (distance > d)
                        {
                            auxIndex = i; 
                            distance = d;

                        }
                    }
                }

                if (auxIndex!= -1)
                {
                    beatSound.Play(); 
                    textDistance.text = distance.ToString("0.0");
                    pos = heartList[auxIndex].transform.position;
                    //this.rend.transform.DOShakeScale(0.2f, 0.1f,0,0); 
                    this.rend.transform.DOPunchScale(this.rend.transform.localScale*1.01f, 0.4f, 0, 1);             
                        
                }

                // this.transform.LookAt(pos);
                target = pos; 
            }
            else
            {
                rend.material.color = Color.black; 
            }
            yield return new WaitForSeconds(1); 
        }
        yield return null; 
    }

    // Update is called once per frame
    void Update()
    {
      
            this.transform.LookAt(target); 
        
    }
}
