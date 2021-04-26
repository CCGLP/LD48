using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class DeathController : MonoBehaviour
{
    public static DeathController Instance; 
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private float timeToDeath = 30;

    [SerializeField]
    private Image fadeImage; 

    private RectTransform rt;

    private bool isCountdown = false;
    private int previousInt;
    private float timer = 0; 

    void Start()
    {
        Instance = this; 
        previousInt = (int) timeToDeath; 
        rt = text.GetComponent<RectTransform>();
        text.alpha = 0; 
    }

    public void ActivateDeathCounter()
    {
        isCountdown = true;
        text.DOFade(1, 0.4f); 
    }

    public void DeactivateDeathCounter()
    {
        isCountdown = false;
        text.DOFade(0, 0.4f); 
    }

    public void EndGameGood()
    {
        FinalMenuScene.timePlayed = timer;
        FinalMenuScene.secrets = SecretsController.Instance.GetSecrets();
        FinalMenuScene.heartsCollected = PlayerController.Instance.numberOfHeartsCollected; 
        fadeImage.DOFade(1, 0.7f).onComplete += () =>
        {
            DOVirtual.DelayedCall(2f, () =>
            {
                DOTween.KillAll();
                SceneManager.LoadScene(1);
            });
        };
    }

    void Update()
    {
        timer += Time.deltaTime; 
        if (isCountdown)
        {
            timeToDeath -= Time.deltaTime;
            text.text = timeToDeath.ToString("0"); 
            if ((int)timeToDeath != previousInt)
            {
                previousInt = (int)timeToDeath;
                rt.DOPunchScale(Vector3.one * 1.1f, 0.5f); 
            }
            if (timeToDeath<= 0)
            {
                fadeImage.DOFade(1, 0.7f).onComplete+= () =>
                {
                    DOVirtual.DelayedCall(2f, () =>
                    {
                        DOTween.KillAll();
                        SceneManager.LoadScene(0);
                    });
                };
            }

        }
    }
}
