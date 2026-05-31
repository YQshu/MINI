using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float followSpeed = 0.5f;
    [SerializeField] private Vector3 offset;

    private void Start()
    {
        if (player == null)
            player = FindObjectOfType<Player>()?.transform;
    }

    private void LateUpdate()
    {
        if (player == null) return;

        Vector3 target = player.position + offset;
        target.z = transform.position.z;
        target.y = transform.position.y;
        transform.position = Vector3.Lerp(transform.position, target, followSpeed * Time.deltaTime);
    }
}