using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening; 
public class CuteText : MonoBehaviour
{
    public static CuteText Instance; 
    private TextMeshProUGUI text; 

    [SerializeField]
    private List<string> cuteTexts; 
    void Start()
    {
        Instance = this; 
        text = this.GetComponent<TextMeshProUGUI>();

    }


    public void Init()
    {
        text.text = cuteTexts[Random.Range(0, cuteTexts.Count)];
        text.alpha = 1;
        text.DOFade(0, 5f);
        text.rectTransform.DOAnchorPosY(400, 5).onComplete += () =>
        {
            text.alpha = 0;
            text.rectTransform.anchoredPosition = new Vector2(0, 0);
        };

    }
    
}
