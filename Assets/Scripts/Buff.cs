using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {

        other.GetComponentInParent<PlayerController>().OnBuffHit();
        CuteText.Instance.Init(); 
        Destroy(this.gameObject); 
    }

    public void AllDirectionsRaycast()
    {
        SendRaycastToActivateNeighbourInDirection(Vector3.right);
        SendRaycastToActivateNeighbourInDirection(Vector3.left);
        SendRaycastToActivateNeighbourInDirection(Vector3.up);
        SendRaycastToActivateNeighbourInDirection(Vector3.down);
        SendRaycastToActivateNeighbourInDirection(Vector3.forward);
        SendRaycastToActivateNeighbourInDirection(Vector3.back);
    }

    private void OnDestroy()
    {
        GenerationController.Instance.hearts.Remove(this.transform.parent.gameObject);

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

        }
    }




}
