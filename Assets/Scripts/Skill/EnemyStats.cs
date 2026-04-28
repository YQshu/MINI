/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : Characterstats
{
    private Enemy enemy;
    private ItemDrop myDropSystem;
    public Stats soulsDropAmount;

    [Header("Level details")]
    [SerializeField] private int level = 1;

    [Range(0f,1f)]
    [SerializeField] private float percantageModifler = .4f;

    protected override void Start()
    {
        soulsDropAmount.SetDefaultValue(100); 
        ApplyLevelModifiers();
        base.Start();

        enemy = GetComponent<Enemy>();
        myDropSystem = GetComponent<ItemDrop>();

    }

    private void ApplyLevelModifiers()
    {
        Modify(strength);
        Modify(agility);
        Modify(intelligence);
        Modify(vitality);

        Modify(damage);
        Modify(critChance);
        Modify(critPower);

        Modify(maxHp);
        Modify(armor);
        Modify(evasion);
        Modify(magicResitance);

        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightningDamage);
        
        Modify(soulsDropAmount);
    }

    private void Modify(Stats _stat)
    {
        for (int i = 1; i < level; i++)
        {
            float modifier = _stat.getValue() * percantageModifler;

            _stat.AddModifier(Mathf.RoundToInt(modifier));
        }
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }
    protected override void Die()
    {
        base.Die();
        enemy.Die();

        PlayerManager.Instance.currency += soulsDropAmount.getValue();
        myDropSystem.GenerateDrop();
    }

}
*/
using UnityEngine;

public class EnemyStats : Characterstats
{
    private Enemy enemy;
    private ItemDrop myDropSystem;
    public Stats soulsDropAmount;

    [Header("Level details")]
    [SerializeField] private int level = 1;

    [Range(0f, 1f)]
    [SerializeField] private float percantageModifler = .4f;

    protected override void Start()
    {
        soulsDropAmount.SetDefaultValue(100);
        ApplyLevelModifiers();
        base.Start();

        enemy = GetComponent<Enemy>();
        myDropSystem = GetComponent<ItemDrop>();
    }

    private void ApplyLevelModifiers()
    {
        Modify(strength);
        Modify(agility);
        Modify(intelligence);
        Modify(vitality);

        Modify(damage);
        Modify(critChance);
        Modify(critPower);

        Modify(maxHp);
        Modify(armor);
        Modify(evasion);
        Modify(magicResitance);

        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightningDamage);

        Modify(soulsDropAmount);
    }

    private void Modify(Stats _stat)
    {
        for (int i = 1; i < level; i++)
        {
            float modifier = _stat.getValue() * percantageModifler;
            _stat.AddModifier(Mathf.RoundToInt(modifier));
        }
    }

    public override void TakeDamage(int _damage, bool triggerEffects = true)
    {
        base.TakeDamage(_damage, triggerEffects);
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage, true);
    }

    protected override void Die()
    {
        base.Die();
        enemy.Die();

        PlayerManager.Instance.currency += soulsDropAmount.getValue();
        myDropSystem.GenerateDrop();

        Destroy(gameObject,5f);
    }

    // 敌人冰冻满层时受到额外控制效果
    protected override void TriggerMaxStackEffect(AilmentType type)
    {
        base.TriggerMaxStackEffect(type);
    }
}