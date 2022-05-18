using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private float FollowSpeed;

    private Vector3 offset;
    private Vector3 target;

    private void Awake()
    {
        offset = transform.position;
    }

    private void Start()
    {

    }

    private void FixedUpdate()
    {
        target = Player.Instance.transform.position + offset;
        transform.position = Vector3.Lerp(transform.position, target, FollowSpeed * Time.deltaTime);
    }
}
