using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement; 

public class EndTrigger : MonoBehaviour
{

    public void OnTriggerEnter(Collider other)
    {
        DOVirtual.DelayedCall(10, () =>
        {
            DeathController.Instance.EndGameGood(); 
        });
    }
}
