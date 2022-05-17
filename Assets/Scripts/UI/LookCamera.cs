using UnityEngine;

public class LookCamera : MonoBehaviour
{
    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(-Camera.main.transform.forward);
    }
}
