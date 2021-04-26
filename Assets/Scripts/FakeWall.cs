using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeWall : MonoBehaviour
{
    [SerializeField]
    private GameObject bigHeart; 
    private void OnTriggerEnter(Collider other)
    {
        SecretsController.Instance.AddNewSecret();
        bigHeart.gameObject.SetActive(true); 
        Destroy(this.gameObject); 
    }
}
