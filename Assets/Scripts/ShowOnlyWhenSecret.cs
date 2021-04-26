using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOnlyWhenSecret : MonoBehaviour
{
    [SerializeField]
    private int secretNumber = 1; 


    void Start()
    {
       if (secretNumber > SecretsController.Instance.GetSecrets())
       {
            this.gameObject.SetActive(false); 
       }
    }

   
}
