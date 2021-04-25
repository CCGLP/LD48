using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; 

public class GlitchController : MonoBehaviour
{
    [SerializeField]
    private RectTransform rt;

    [SerializeField]
    private AudioSource audioSource;



    private void OnTriggerEnter(Collider other)
    {
        audioSource.Play();
        rt.DOAnchorPosY(0, 0.5f).onComplete += () =>
        {
            DOVirtual.DelayedCall(4, () =>
            {
                rt.GetComponent<CanvasGroup>().DOFade(0, 0.4f); 
            }); 
        };
        Destroy(this.gameObject); 
    }
}
