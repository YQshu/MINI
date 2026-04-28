using UnityEngine;

[CreateAssetMenu(fileName = "Heal Effect", menuName = "Data/Item Effect/Heal Effect")]
public class HealEffect : ItemEffect
{

    [Range(0f, 1f)]
    [SerializeField] private float healPercent;

    public override void ExecuteEffect(Transform _enemyTransform)
    {
        PlayerStats playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();
        int healAmount = Mathf.RoundToInt(playerStats.GetMaxHp() * healPercent);
        playerStats.IncreaseHealthBy(healAmount);
    }
}
