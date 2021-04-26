using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; 
public class SecretButton : MonoBehaviour
{
    private static int counter;
    private int number = 0;
    [SerializeField]
    private GameObject bigHeart; 
    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
        number = int.Parse(this.gameObject.name); 
    }

    public void OnRaycastHit()
    {
        if (!DOTween.IsTweening(this.transform))
        {
          
            if (counter == number - 1)
            {

                counter++;
                //Suena Musiquita
                if (counter == 3)
                {
                    //done
                    SecretsController.Instance.AddNewSecret();
                    bigHeart.gameObject.SetActive(true); 
                    Destroy(this.gameObject); 
                    Debug.Log("well done");
                }
                else
                {
                    this.transform.DOMoveZ(this.transform.position.z + 0.1f, 0.2f).onComplete += () =>
                    {
                        this.transform.DOMoveZ(this.transform.position.z - 0.1f, 0.2f);
                    };
                }
            }
            else
            {
                this.transform.DOMoveZ(this.transform.position.z + 0.1f, 0.2f).onComplete += () =>
                {
                    this.transform.DOMoveZ(this.transform.position.z - 0.1f, 0.2f);
                };
                counter = -1; 
                if (number == 1)
                {
                    counter = 1; 
                }
            }
        }
    }
}
