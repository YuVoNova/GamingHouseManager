using UnityEngine;

public class Magnet : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)    // Money
        {
            other.GetComponent<Money>().Magnetize(transform.parent.position);
        }
    }
}
