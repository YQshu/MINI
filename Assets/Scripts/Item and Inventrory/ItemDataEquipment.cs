using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon, //武器
    Armor,  //护甲
    Amulet, //护符
    Flask   //药瓶
}

[CreateAssetMenu(fileName = "New Item", menuName = "Data/Equipment")]
public class ItemDataEquipment : ItemData
{
    public EquipmentType equipmentType;

    public float itemCoolDown;
    public ItemEffect[] itemEffect;

    [TextArea]
    public string itemEffectDescription;

    [Header("Major stats")]
    public int strength;//力量
    public int agility;//敏捷
    public int intelligence;//智力,不得不做魔法相关了
    public int vitality;//体力

    [Header("Offensive stats")]
    public int damage;
    public int critChance;//暴击率
    public int critPower;//暴击伤害  默认150%


    [Header("Defensive stats")]
    public int maxHp;
    public int armor;//护甲
    public int evasion;//闪避
    public int magicResitance;

    [Header("Magic stats")]
    public int fireDamage;
    public int iceDamage;
    public int lightningDamage;

    [Header("Craft requirements")]
    public List<InventoryItem> craftingMaterials;

    private int minDescriptionLength;

    public void ExecuteItemEffect(Transform _enemyTransform)
    {
        foreach (var item in itemEffect)
        {
            item.ExecuteEffect(_enemyTransform);
        }
    }

    public void AddModifier()
    {
        PlayerStats playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();

        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligence.AddModifier(intelligence);
        playerStats.vitality.AddModifier(vitality);

        playerStats.damage.AddModifier(damage);
        playerStats.critChance.AddModifier(critChance);
        playerStats.critPower.AddModifier(critPower);

        playerStats.maxHp.AddModifier(maxHp);
        playerStats.armor.AddModifier(armor);
        playerStats.evasion.AddModifier(evasion);
        playerStats.magicResitance.AddModifier(magicResitance);

        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.lightningDamage.AddModifier(lightningDamage);

    }
    public void RemoveModifier()
    {
        PlayerStats playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();
        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intelligence);
        playerStats.vitality.RemoveModifier(vitality);
      
        playerStats.damage.RemoveModifier(damage);
        playerStats.critChance.RemoveModifier(critChance);
        playerStats.critPower.RemoveModifier(critPower);

        playerStats.maxHp.RemoveModifier(maxHp);
        playerStats.armor.RemoveModifier(armor);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicResitance.RemoveModifier(magicResitance);

        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.lightningDamage.RemoveModifier(lightningDamage);

    }

    public override string GetDescription()
    {
        sb.Length = 0;
        minDescriptionLength = 0;

        AddItemDescription("Strength", strength);
        AddItemDescription("Agility", agility);
        AddItemDescription("Intelligence", intelligence);
        AddItemDescription("Vitality", vitality);
        AddItemDescription("Damage", damage);
        AddItemDescription("Crit Chance", critChance);
        AddItemDescription("Crit Power", critPower);
        AddItemDescription("Max HP", maxHp);
        AddItemDescription("Armor", armor);
        AddItemDescription("Evasion", evasion);
        AddItemDescription("Magic Res", magicResitance);
        AddItemDescription("Fire Damage", fireDamage);
        AddItemDescription("Ice Damage", iceDamage);
        AddItemDescription("Lightning Dmg", lightningDamage);


        /*for(int i  = 0; i < itemEffect.Length; i++)
        {
            sb.AppendLine();
            sb.Append(itemEffect[i].Effectdescription);
        }*/

        if(minDescriptionLength < 5)
        {
            for(int i = 0; i < 5 - minDescriptionLength; i++)
            {
                sb.AppendLine();
                sb.Append("");
            }
        }
        if (itemEffectDescription.Length > 0)
        {
            sb.Append(itemEffectDescription);
        }

        return sb.ToString();
    }

    private void AddItemDescription(string _name, int _value)
    {
        if(_value != 0)
        {
            if(sb.Length > 0)
            {
                sb.AppendLine();
            }
            if(_value > 0)
            {
                sb.Append("+ " + _value + " " + _name);
            }
            minDescriptionLength++;
        }
    }
}
