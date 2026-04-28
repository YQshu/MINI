/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : Characterstats
{
    private Player player;

    protected override void Start()
    {
        base.Start();
        player = GetComponent<Player>();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }

    protected override void Die()
    {
        base.Die();
        player.Die();
        
        GameManager.instance.lostCurrencyAmount = PlayerManager.Instance.currency;
        PlayerManager.Instance.currency = 0;

        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }

    protected override void DecreaseHealthBy(int _damage)
    {
        base.DecreaseHealthBy(_damage);
        ItemDataEquipment currentArmor = Inventory.instance.GetEquipment(EquipmentType.Armor);
        if (currentArmor != null)
        {
            currentArmor.ExecuteItemEffect(player.transform);
        }

    }

}*/
using UnityEngine;

public class PlayerStats : Characterstats
{
    private Player player;

    protected override void Start()
    {
        base.Start();
        player = GetComponent<Player>();
    }

    public override void TakeDamage(int _damage, bool triggerEffects = true)
    {
        base.TakeDamage(_damage, triggerEffects);
    }

    public override void TakeDamage(int _damage)
    {
        TakeDamage(_damage, true);
    }

    protected override void Die()
    {
        base.Die();
        player.Die();

        GameManager.instance.lostCurrencyAmount = PlayerManager.Instance.currency;
        PlayerManager.Instance.currency = 0;

        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }

    protected override void DecreaseHealthBy(int _damage)
    {
        base.DecreaseHealthBy(_damage);

        if(_damage > GetMaxHp() *.3f)
        {
            player.SetupKnockbackPower(new Vector2 (7, 10));
        }

        ItemDataEquipment currentArmor = Inventory.instance.GetEquipment(EquipmentType.Armor);
        if (currentArmor != null)
        {
            currentArmor.ExecuteItemEffect(player.transform);
        }
    }

    protected override void TriggerMaxStackEffect(AilmentType type)
    {
        base.TriggerMaxStackEffect(type);
    }
}