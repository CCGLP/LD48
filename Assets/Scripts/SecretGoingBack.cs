using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretGoingBack : MonoBehaviour
{

    

    private void OnTriggerEnter(Collider other)
    {
        PlayerController.Instance.SetLimitSpeed();
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController.Instance.QuitLimitSpeed(); 
    }

}
