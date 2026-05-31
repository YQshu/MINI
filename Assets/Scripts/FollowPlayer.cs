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

        // 只跟随 X 轴！！！Y 和 Z 永远保持背景自己的位置，不随玩家动
        Vector3 targetPos = transform.position;
        targetPos.x = player.position.x + offset.x; // 只改左右方向

        // 平滑跟随
        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
    }
}