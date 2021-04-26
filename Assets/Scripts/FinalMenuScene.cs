using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; 
public class FinalMenuScene : MonoBehaviour
{
    public static int secrets;
    public static float timePlayed;
    public static int heartsCollected;
    [SerializeField]
    private TextMeshProUGUI heartsCollectedText, timePlayedText, secretsCollectedText;

    private void Start()
    {
        heartsCollectedText.text = "Hearts collected: " + heartsCollected.ToString();
        timePlayedText.text = "Time played: " + timePlayed.ToString("0") +"s";
        secretsCollectedText.text = "Secrets discovered: " + secrets.ToString()+"/5"; 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            SceneManager.LoadScene(0); 
        }
    }
}
