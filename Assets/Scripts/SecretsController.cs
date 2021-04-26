using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; 
public class SecretsController : MonoBehaviour
{
    public static SecretsController Instance; 
    [SerializeField]
    private TMPro.TextMeshProUGUI secretsText;
    [SerializeField]
    private int numberOfSecrets = 4;
    private int index = 0;



    public int GetSecrets()
    {
        return index; 
    }

    private void Start()
    {
        Instance = this; 
    }

    public void AddNewSecret()
    {
        secretsText.DOFade(1, 0.3f); 
        index++;
        secretsText.text = index.ToString() + "/" + numberOfSecrets.ToString(); 
    }
}
