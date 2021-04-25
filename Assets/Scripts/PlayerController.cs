using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; 
public class PlayerController : MonoBehaviour
{

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


    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined; 
        rb = this.GetComponent<Rigidbody>();
        hand = this.transform.GetChild(0);
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

    public void OnBuffHit()
    {
        Debug.Log("Funca");
        //TODO
        heartSound.Play(); 
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


                this.transform.DOShakeRotation(0.1f, 20, 20);
                hand.DOLocalRotate(Vector3.right * 60, speedClick).onComplete += () =>
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
        
        rb.velocity *= speed;
        rb.velocity = new Vector3(rb.velocity.x, ySpeed, rb.velocity.z); 
    }

    private void UpdateRotation()
    {

        actualRotation += new Vector3(rotationSpeed * Input.GetAxis("Mouse Y") * Time.deltaTime, rotationSpeed * Input.GetAxis("Mouse X") * Time.deltaTime, 0);
        actualRotation = new Vector3(Mathf.Clamp(actualRotation.x, rotationLimitDown.x, rotationLimitUp.x), actualRotation.y, 0);
        this.transform.rotation = Quaternion.Euler(actualRotation);
    }
}
