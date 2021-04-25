using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateConstantly : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float angularVelocity = 100;
    private float actualVelocity = 0; 
    // Update is called once per frame
    void Update()
    {
        actualVelocity += angularVelocity * Time.deltaTime; 
        this.transform.rotation = Quaternion.Euler(0, actualVelocity, 0);
    }
}
