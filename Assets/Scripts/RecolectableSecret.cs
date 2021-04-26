using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecolectableSecret : MonoBehaviour
{
    [SerializeField]
    private GameObject particles; 
    private void OnTriggerEnter(Collider other)
    {
        SecretsController.Instance.AddNewSecret();
        if (particles != null)
        {
            Instantiate(particles, this.transform.position, Quaternion.identity); 
        }
        Destroy(this.gameObject); 
    }
}
