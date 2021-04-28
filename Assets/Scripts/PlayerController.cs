using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro; 

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance; 
    [SerializeField]
    private float rotationSpeed = 3f;

    [SerializeField]
    private float speed = 10f;

    private Rigidbody rb;
    private Vector3 actualRotation;
    private Transform hand;

    private Vector3 originalHandRotation;

    [SerializeField]
    private Vector3 rotationLimitUp, rotationLimitDown;


    [SerializeField]
    private float jumpStartVelocity = 10;

    [SerializeField]
    private float speedClick = 0.1f;
    [SerializeField]
    private float speedReturnClick = 0.1f;

    [SerializeField]
    private GameObject particlePrefab;

    [SerializeField]
    private float timeToBetatesting = 10; 
    private float timer = 0;

    [SerializeField]
    private AudioSource hitSound, heartSound;


    [SerializeField]
    private RectTransform textDebuffParent;
    [SerializeField]
    private GameObject textDebuffPrefab; 

    [SerializeField]
    private List<Debuff> debuffPrefabList;

    private List<Debuff> debuffList;

    private bool limitSpeed = false; 

    private float moveDebuff = 0, rotationDebuff = 0, hitDebuff = 0;
    public int numberOfHeartsCollected = 0; 
    // Start is called before the first frame update
    void Start()
    {
        Instance = this; 
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked; 
        rb = this.GetComponent<Rigidbody>();
        hand = this.transform.GetChild(0);
        debuffList = new List<Debuff>(); 
        originalHandRotation = hand.transform.rotation.eulerAngles; 
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSpeed();
        UpdateRotation();
        ClickDown();
        CheckBetaTester(); 
    }


    public void SetLimitSpeed()
    {
        limitSpeed = true; 
    }

    public void QuitLimitSpeed()
    {
        limitSpeed = false; 
    }

    public void OnBuffHit()
    {
        numberOfHeartsCollected++; 
        if (debuffList.Count > 0)
        {
            debuffList[debuffList.Count - 1].difficulty--;
            if (debuffList[debuffList.Count - 1].difficulty <= 0)
            {
                switch (debuffList[debuffList.Count - 1].type)
                {
                    case DebuffType.HITSLOW:
                        hitDebuff = -hitDebuff;
                        break;

                    case DebuffType.MOVESLOW:
                        moveDebuff = -moveDebuff;
                        break;

                    case DebuffType.ROTATESLOW:
                        rotationDebuff = -rotationDebuff;
                        break;
                }

                Destroy(debuffList[debuffList.Count - 1].tmpReference.gameObject);

                debuffList.RemoveAt(debuffList.Count - 1);
                if (debuffList.Count <= 1)
                {
                    DeathController.Instance.DeactivateDeathCounter(); 
                }
            }

            else
            {
                debuffList[debuffList.Count - 1].tmpReference.DOFade(0.2f * debuffList[debuffList.Count - 1].difficulty, 0.5f);
            }
        }
        heartSound.Play(); 
    }

    public void NextDebuff()
    {
        if (debuffPrefabList.Count > 0)
        {
            int aux = 0;
            var debuff = debuffPrefabList[aux];

            switch (debuff.type)
            {
                case DebuffType.HITSLOW:
                    hitDebuff = debuff.value;
                    break;

                case DebuffType.MOVESLOW:
                    moveDebuff = debuff.value;
                    break;

                case DebuffType.ROTATESLOW:
                    rotationDebuff = debuff.value;
                    break;
            }

            var go = Instantiate(textDebuffPrefab, textDebuffParent);
            debuff.tmpReference = go.GetComponent<TextMeshProUGUI>();
            debuff.tmpReference.text = debuff.text;
            debuffList.Add(debuff);
            if (debuffList.Count >= 2)
            {
                DeathController.Instance.ActivateDeathCounter(); 
            }
            debuffPrefabList.RemoveAt(aux);
        }
        else
        {
            //End
        }
    }

    private void CheckBetaTester()
    {
        if (rb.velocity.y < -5f)
        {
            timer += Time.deltaTime; 
            if (timer > timeToBetatesting)
            {
                timer = -99999;
                GenerationController.Instance.InstantiateBetaTesterLevel(this.transform.position); 
            }
        }

        else
        {
            timer = 0; 
        }
    }

    private void ClickDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!DOTween.IsTweening(hand))
            {


                this.transform.DOShakeRotation(0.1f, 10, 20);
                hand.DOLocalRotate(Vector3.right * 60, speedClick + hitDebuff).onComplete += () =>
                {
                    RaycastHit hit;
                    Vector3 origin = this.transform.position;

                    if (Physics.SphereCast(origin, 0.03f, this.transform.forward, out hit, 3.2f))
                    {
                        if (hit.collider != null)
                        {
                            hitSound.pitch = Random.Range(0.9f, 1.1f); 
                            hitSound.Play(); 
                            Debug.Log(hit.collider.gameObject.name);
                            var cube = hit.collider.GetComponent<CubeController>();
                            if (cube != null)
                            {
                               
                                cube.ReceiveHit();
                            }
                            else
                            {
                                var secretButton = hit.collider.GetComponent<SecretButton>(); 
                                if (secretButton!= null)
                                {
                                    secretButton.OnRaycastHit(); 
                                }
                            }
                        }
                    }

                    hand.DOLocalRotate(originalHandRotation, speedReturnClick);
                };
            }
        }

    }
    private void UpdateSpeed()
    {
        float ySpeed = rb.velocity.y;

        rb.velocity = (this.transform.forward * Input.GetAxisRaw("Vertical") * speed) + (this.transform.right * Input.GetAxisRaw("Horizontal") * speed);
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.velocity = rb.velocity.normalized;
        
        rb.velocity *= (speed - moveDebuff);
        rb.velocity = new Vector3(rb.velocity.x, ySpeed, rb.velocity.z); 

        if (limitSpeed)
        {
            rb.velocity = new Vector3(Input.GetAxisRaw("Vertical") < -0.8f ? rb.velocity.x : 0, ySpeed, Input.GetAxisRaw("Vertical") < -0.8f ? rb.velocity.z : 0);
        }

    }

    private void UpdateRotation()
    {

        actualRotation += new Vector3((rotationSpeed - rotationDebuff) * -Input.GetAxis("Mouse Y") * Time.deltaTime, (rotationSpeed - rotationDebuff) * Input.GetAxis("Mouse X") * Time.deltaTime, 0);
        actualRotation = new Vector3(Mathf.Clamp(actualRotation.x, rotationLimitDown.x, rotationLimitUp.x), actualRotation.y, 0);
        this.transform.rotation = Quaternion.Euler(actualRotation);
    }
}

[System.Serializable]
public class Debuff
{
    public DebuffType type;
    public float value;
    public string text;
    public int difficulty;
    public TextMeshProUGUI tmpReference; 
}

[System.Serializable]
public enum DebuffType
{
    HITSLOW,
    ROTATESLOW,
    MOVESLOW
}