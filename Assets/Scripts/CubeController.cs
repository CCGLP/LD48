using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; 
public class CubeController : MonoBehaviour
{

    public int hardness = 1;

    [SerializeField]
    private List<Color> hardnessColor;

    [SerializeField]
    private GameObject particlePrefab; 
    public Renderer rend;

    public static int actualDifficulty = 2; 

    private void Init(int difficulty)
    {
        hardness = Random.Range(difficulty - 2, difficulty + 1);
        
    }

    private void Start()
    {
        Init(actualDifficulty);
        rend = this.GetComponent<Renderer>();
        UpdateHardnessColor();
    }

    public void ReceiveHit()
    {
        hardness--; 

        if (hardness <= 0)
        {
            SendRaycastToActivateNeighbourInDirection(Vector3.right);
            SendRaycastToActivateNeighbourInDirection(Vector3.left);
            SendRaycastToActivateNeighbourInDirection(Vector3.up);
            SendRaycastToActivateNeighbourInDirection(Vector3.down);
            SendRaycastToActivateNeighbourInDirection(Vector3.forward);
            SendRaycastToActivateNeighbourInDirection(Vector3.back);

            var particles = Instantiate(particlePrefab, this.transform.position, Quaternion.identity);
            particles.GetComponent<ParticleSystem>().startColor = rend.material.color;
            DOVirtual.DelayedCall(1, () =>
            {
                Destroy(particles);
            });
            DestroyThisObject(); 
        }
        else
        {
            var particles = Instantiate(particlePrefab, this.transform.position, Quaternion.identity);
            particles.GetComponent<ParticleSystem>().startColor = rend.material.color;
            particles.GetComponent<ParticleSystem>().maxParticles = 200; 
            DOVirtual.DelayedCall(1, () =>
            {
                Destroy(particles);
            });
            UpdateHardnessColor(); 
        }
    }


    private void DestroyThisObject()
    {
        if (this.tag == "Finish")
        {
            GenerationController.Instance.InstantiateLevel(this.transform.position); 
        }
        Destroy(this.gameObject);

    }
    private void SendRaycastToActivateNeighbourInDirection(Vector3 direction)
    {
        RaycastHit hit;
        Physics.Raycast(this.transform.position, direction, out hit, 1);
        if (hit.collider != null)
        {
            if (hit.collider.GetComponent<CubeController>() != null)
            {
                hit.collider.GetComponent<Renderer>().enabled = true;
            }
            else if (hit.collider.GetComponent<Buff>() != null)
            {
                hit.collider.GetComponent<Buff>().AllDirectionsRaycast(); 
            }
        }
    }
    private void UpdateHardnessColor()
    {
        rend.material.color = hardnessColor[Mathf.Clamp( hardness - 1, 0, hardnessColor.Count-1)];
    }
}
