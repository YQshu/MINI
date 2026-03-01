/*using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public enum StatType
{
    strength,
    agllity,
    intelligence,
    vitality,
    damage,
    cirtChance,
    cirtPower,
    health,
    armor,
    evasion,
    magicRes,
    fireDamage,
    iceDamage,
    lightningDamage
}


public class Characterstats : MonoBehaviour
{
    private EntityFx fx;

    [Header("Major stats")]
    public Stats strength;//力量
    public Stats agility;//敏捷
    public Stats intelligence;//智力
    public Stats vitality;//体力

    [Header("Offensive stats")]
    public Stats damage;
    public Stats critChance;//暴击率
    public Stats critPower;//暴击伤害  默认150%


    [Header("Defensive stats")]
    public Stats maxHp;
    public Stats armor;//护甲
    public Stats evasion;//闪避
    public Stats magicResitance;

    [Header("Magic stats")]
    public Stats fireDamage;
    public Stats iceDamage;
    public Stats lightningDamage;

    public bool isIgnited;
    public bool isChilled;
    public bool isShocked;

    public int currentHp;

    public System.Action onHeathChanged;
    public bool isDead { get; private set; }

    [SerializeField] private float AilmentsDuration = 6;
    private float igniteTimer;
    private float chilledTimer;
    private float shockedTimer;

    private float igniteDamageCoolDown = .3f;
    private float igniteDamageTimer;
    private int shockDamage;
    [SerializeField] private GameObject shockStrikePrefab;
    private int igniteDamage;


    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHp = GetMaxHp();
        //封装stats是为了后面设计武器和Buff时将数据在stats里进行修改
        //damage.AddModifier(4);这样就能修改玩家伤害hp之类的属性值
        fx = GetComponent<EntityFx>();
    }

    private void Update()
    {
        igniteTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;

        igniteDamageTimer -= Time.deltaTime;
        if (igniteTimer < 0)
        {
            isIgnited = false;
        }
        if (chilledTimer < 0)
        {
            isChilled = false;
        }
        if (shockedTimer < 0)
        {
            isShocked = false;
        }
        if (isIgnited)
        {
            ApplyIgniteDamage();
        }
    }

    public virtual void IncreaseStatBy(int _modifier, float _duration, Stats _statsToModify)
    {
        StartCoroutine(StatModCoroutine(_modifier, _duration, _statsToModify));
    }

    private IEnumerator StatModCoroutine(int _modifier, float _duration, Stats _statsToModify)
    {
        _statsToModify.AddModifier(_modifier);

        yield return new WaitForSeconds(_duration);

        _statsToModify.RemoveModifier(_modifier);
    }

    private void ApplyIgniteDamage()
    {
        if (igniteDamageTimer < 0)
        {
            Debug.Log("好火呀，比夷陵之火还好" + igniteDamage);
            currentHp -= igniteDamage;
            if (currentHp < 0 && !isDead)
            {
                Die();
            }
            igniteDamageTimer = igniteDamageCoolDown;
        }
    }

    public virtual void DoDamage(Characterstats _targetStats)
    {
        if (CanAvoidAttack(_targetStats))
        {
            return;
        }

        int totalDamage = damage.getValue() + strength.getValue();

        if (canCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
        }


        totalDamage = CheckTargetArmor(_targetStats, totalDamage);

        _targetStats.TakeDamage(totalDamage);
        doMagicDamage(_targetStats);
    }

    public virtual void doMagicDamage(Characterstats _targetStats)
    {
        int _fireDamage = fireDamage.getValue();
        int _iceDamage = iceDamage.getValue();
        int _lightningDamage = lightningDamage.getValue();

        int totalMagicDamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.getValue();
        totalMagicDamage = CheckTargetResistance(_targetStats, totalMagicDamage);
        _targetStats.TakeDamage(totalMagicDamage);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) <= 0)
        {
            return;
        }
        AttemptToApplyAilements(_targetStats, _fireDamage, _iceDamage, _lightningDamage);
    }

    private void AttemptToApplyAilements(Characterstats _targetStats, int _fireDamage, int _iceDamage, int _lightningDamage)
    {
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
        bool canApplyShock = _lightningDamage > _fireDamage && _lightningDamage > _iceDamage;

        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if (Random.value < .3f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                _targetStats.ApplyAliments(canApplyIgnite, canApplyChill, canApplyShock);
                Debug.Log("hot");
            }
            if (Random.value < .5f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAliments(canApplyIgnite, canApplyChill, canApplyShock);
                Debug.Log("cold");
            }
            if (Random.value < .5f && _lightningDamage > 0)
            {
                canApplyShock = true;
                Debug.Log("shock");
                _targetStats.ApplyAliments(canApplyIgnite, canApplyChill, canApplyShock);
            }
        }
        if (canApplyIgnite)
        {
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));
        }
        if (canApplyShock)
        {
            _targetStats.SetupShockStrikeDamage(Mathf.RoundToInt(_lightningDamage * .2f));
        }
        _targetStats.ApplyAliments(canApplyIgnite, canApplyChill, canApplyShock);
    }

    private int CheckTargetResistance(Characterstats _targetStats, int totalMagicDamage)
    {
        totalMagicDamage -= _targetStats.magicResitance.getValue() + (_targetStats.intelligence.getValue() * 3);
        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 0, int.MaxValue); //防止溢出
        return totalMagicDamage;
    }

    public void ApplyAliments(bool _ignite, bool _chill, bool _shock)
    {
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isChilled && !isShocked && !isIgnited;
        bool canApplyShock = !isChilled && !isIgnited;

        if (_ignite && canApplyIgnite)
        {
            isIgnited = _ignite;
            igniteTimer = AilmentsDuration;

            fx.IgniteFxFor(AilmentsDuration);
        }
        if (_chill && canApplyChill)
        {
            chilledTimer = AilmentsDuration;
            isChilled = _chill;
            GetComponent<Entity>().SlowEntityBy(.2f, AilmentsDuration);//前面是减速的倍率，后面是持续时间，这里将时间都保持相同             
            fx.ChillFxFor(AilmentsDuration);
        }
        if (_shock && canApplyShock)
        {
            if (!isShocked)
            {
                ApplyShock(_shock);
            }
            else
            {
                //防止对玩家的伤害而形成反甲的情况，后期可以视情况删除
                if (GetComponent<Player>() != null)
                    return;

                //这一段代码是寻找最近的攻击目标，会多次用到，所以将其封装为函数
                HitNearestTargetWithShockStrike();

            }
        }
    }

    public void ApplyShock(bool _shock)
    {
        if (isShocked)
        { return; }
        shockedTimer = AilmentsDuration;
        isShocked = _shock;
        fx.ShockFxFor(AilmentsDuration);
    }

    private void HitNearestTargetWithShockStrike()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1)
            {
                float distanceEnemy = Vector2.Distance(transform.position, hit.transform.position);
                if (distanceEnemy < closestDistance)
                {
                    closestDistance = distanceEnemy;
                    closestEnemy = hit.transform;
                }
            }
            if (closestEnemy == null)
            {
                closestEnemy = transform;
            }


        }

        if (closestEnemy != null)
        {
            GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);

            newShockStrike.GetComponent<ThunderSkrike_Controller>().Setup(shockDamage, closestEnemy.GetComponent<Characterstats>());
        }
    }

    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;

    public void SetupShockStrikeDamage(int _damage) => shockDamage = _damage;




    private int CheckTargetArmor(Characterstats _targetStats, int totalDamage)
    {
        if (_targetStats.isChilled)
        {
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.getValue() * .8f);
        }
        else
        {
            totalDamage -= _targetStats.armor.getValue();
        }

        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    private bool CanAvoidAttack(Characterstats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.getValue() + _targetStats.agility.getValue();

        if (isShocked)
        {
            totalEvasion += 20;
        }

        if (Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }
        return false;
    }

    public virtual void TakeDamage(int _damage)
    {
        DecreaseHealthBy(_damage);
        GetComponent<Entity>().DamageImpact();
        fx.StartCoroutine("FlashFx");

        if (currentHp < 0 && !isDead)
        {
            Die();
        }
    }
    public virtual void IncreaseHealthBy(int _amont)
    {
        currentHp += _amont;
        if (currentHp > GetMaxHp())
        {
            currentHp = GetMaxHp();
        }
        if (onHeathChanged != null)
        {
            onHeathChanged();
        }
    }

    protected virtual void DecreaseHealthBy(int _damage)
    {
        currentHp -= _damage;
        if (onHeathChanged != null)
        {
            onHeathChanged();
        }
    }

    protected virtual void Die()
    {
        //throw new NotImplementedException();
        isDead = true;
    }
    private bool canCrit()
    {
        int totalCriticalChance = critChance.getValue() + agility.getValue();
        if (Random.Range(0, 100) < totalCriticalChance)
        {
            return true;
        }
        return false;
    }

    private int CalculateCriticalDamage(int _damage)
    {
        float totalCritPower = critPower.getValue() + strength.getValue() * .01f;

        float criticalDamage = _damage * totalCritPower * 0.01f;
        return Mathf.RoundToInt(criticalDamage);
    }

    public int GetMaxHp()
    {
        return maxHp.getValue() + vitality.getValue() * 5;
    }

    public Stats GetStat(StatType _statType)
    {
        switch (_statType)
        {
            case StatType.strength:
                return strength;
            case StatType.agllity:
                return agility;
            case StatType.intelligence:
                return intelligence;
            case StatType.vitality:
                return vitality;
            case StatType.damage:
                return damage;
            case StatType.cirtChance:
                return critChance;
            case StatType.cirtPower:
                return critPower;
            case StatType.health:
                return maxHp;
            case StatType.armor:
                return armor;
            case StatType.evasion:
                return evasion;
            case StatType.magicRes:
                return magicResitance;
            case StatType.fireDamage:
                return fireDamage;
            case StatType.iceDamage:
                return iceDamage;
            case StatType.lightningDamage:
                return lightningDamage;
            default:
                return null;
        }
    }

}*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    strength,
    agllity,
    intelligence,
    vitality,
    damage,
    cirtChance,
    cirtPower,
    health,
    armor,
    evasion,
    magicRes,
    fireDamage,
    iceDamage,
    lightningDamage
}

public class Characterstats : MonoBehaviour
{
    private EntityFx fx;

    [Header("Major stats")]
    public Stats strength;
    public Stats agility;
    public Stats intelligence;
    public Stats vitality;

    [Header("Offensive stats")]
    public Stats damage;
    public Stats critChance;
    public Stats critPower;

    [Header("Defensive stats")]
    public Stats maxHp;
    public Stats armor;
    public Stats evasion;
    public Stats magicResitance;

    [Header("Magic stats")]
    public Stats fireDamage;
    public Stats iceDamage;
    public Stats lightningDamage;

    public int currentHp;
    public System.Action onHeathChanged;
    public bool isDead { get; private set; }
    public bool isInvincible {  get; private set; }

    [Header("Ailment Settings")]
    [SerializeField] protected float ailmentBaseDuration = 6f;
    [SerializeField] protected int maxAilmentStacks = 5;
    [SerializeField] protected GameObject shockStrikePrefab;

    private bool chillAuraTriggered = false;
    private float chillAuraCooldownTimer = 0f;
    private float chillAuraCooldown = 2f; // 2秒冷却

    // 层数系统
    public class AilmentStack
    {
        public int stacks;
        public float duration;
        public float tickInterval = 0.5f;
        public float nextTickTime;
        public int baseDamagePerTick;
        public float slowAmount = 0.2f;

        public AilmentStack()
        {
            stacks = 0;
            duration = 0;
            nextTickTime = 0;
            baseDamagePerTick = 0;
        }

        public bool ShouldTick() => Time.time >= nextTickTime;
        public void OnTick() => nextTickTime = Time.time + tickInterval;
        public int GetCurrentTickDamage() => baseDamagePerTick * stacks;
    }

    public enum AilmentType
    {
        Ignite,     // 火：叠加伤害，满层回血
        Chill,      // 冰：叠加减速，满层爆发伤害
        Shock       // 电：叠加易伤，满层闪电链（不清层）
    }

    // 存储三种异常状态的层数
    protected Dictionary<AilmentType, AilmentStack> ailmentStacks = new Dictionary<AilmentType, AilmentStack>();

    // 视觉状态
    public bool isIgnited => GetStacks(AilmentType.Ignite) > 0;
    public bool isChilled => GetStacks(AilmentType.Chill) > 0;
    public bool isShocked => GetStacks(AilmentType.Shock) > 0;

    // 额外伤害加成（用于感电）
    public float incomingDamageMultiplier = 1f;
    private int shockStrikeDamage;

    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHp = GetMaxHp();
        fx = GetComponent<EntityFx>();

        // 初始化层数字典
        foreach (AilmentType type in System.Enum.GetValues(typeof(AilmentType)))
        {
            ailmentStacks[type] = new AilmentStack();
        }
    }

    private void Update()
    {
        ProcessAilments();

        if (chillAuraCooldownTimer > 0)
            chillAuraCooldownTimer -= Time.deltaTime;
        else
            chillAuraTriggered = false;
    }

    protected virtual void ProcessAilments()
    {
        foreach (var kvp in ailmentStacks)
        {
            AilmentType type = kvp.Key;
            AilmentStack stack = kvp.Value;

            if (stack.stacks <= 0) continue;

            stack.duration -= Time.deltaTime;

            // 处理持续伤害
            if (stack.ShouldTick() && stack.stacks > 0)
            {
                switch (type)
                {
                    case AilmentType.Ignite:
                        int igniteDamage = stack.GetCurrentTickDamage();
                        TakeDamage(igniteDamage, false);
                        break;

                    case AilmentType.Chill:
                        int chillDamage = stack.baseDamagePerTick;
                        TakeDamage(chillDamage, false);
                        break;

                    case AilmentType.Shock:
                        int shockDamage = stack.baseDamagePerTick;
                        TakeDamage(shockDamage, false);
                        break;
                }

                stack.OnTick();
            }

            // 时间到，减少层数
            if (stack.duration <= 0)
            {
                RemoveAilmentStack(type);
            }
        }

        UpdateShockDamageBonus();
    }

    protected virtual void RemoveAilmentStack(AilmentType type)
    {
        AilmentStack stack = ailmentStacks[type];
        stack.stacks = Mathf.Max(0, stack.stacks - 1);

        if (stack.stacks > 0)
        {
            stack.duration = ailmentBaseDuration;
            ApplyStackEffects(type);
        }
        else
        {
            ClearAilmentVisuals(type);
        }
    }

    protected virtual void ClearAilmentVisuals(AilmentType type)
    {
        switch (type)
        {
            case AilmentType.Ignite:
                break;
            case AilmentType.Chill:
                GetComponent<Entity>()?.SlowEntityBy(1f, 0);
                break;
            case AilmentType.Shock:
                incomingDamageMultiplier = 1f;
                break;
        }
    }

    public int GetStacks(AilmentType type)
    {
        return ailmentStacks.ContainsKey(type) ? ailmentStacks[type].stacks : 0;
    }

    public virtual void AddAilmentStack(AilmentType type, int damage)
    {
        AilmentStack stack = ailmentStacks[type];

        int oldStacks = stack.stacks;
        stack.stacks = Mathf.Min(maxAilmentStacks, stack.stacks + 1);
        stack.duration = ailmentBaseDuration;

        if (oldStacks == 0)
        {
            stack.baseDamagePerTick = damage;
            stack.nextTickTime = Time.time + stack.tickInterval;
        }


        ApplyStackEffects(type);

        if (stack.stacks >= maxAilmentStacks)
        {
            TriggerMaxStackEffect(type);
        }

        SetAilmentVisuals(type);
    }

    public virtual void AddAilmentStacks(AilmentType type, int damage, int count)
    {
        for (int i = 0; i < count; i++)
        {
            AddAilmentStack(type, damage);
        }
    }

    protected virtual void ApplyStackEffects(AilmentType type)
    {
        AilmentStack stack = ailmentStacks[type];

        switch (type)
        {
            case AilmentType.Chill:
                float slowAmount = stack.slowAmount * stack.stacks;
                GetComponent<Entity>()?.SlowEntityBy(slowAmount, stack.duration);
                break;

            case AilmentType.Shock:
                UpdateShockDamageBonus();
                break;
        }
    }

    protected virtual void UpdateShockDamageBonus()
    {
        int shockStacks = GetStacks(AilmentType.Shock);
        incomingDamageMultiplier = 1f + (shockStacks * 0.2f);
    }

    protected virtual void TriggerMaxStackEffect(AilmentType type)
    {
        AilmentStack stack = ailmentStacks[type];

        switch (type)
        {
            case AilmentType.Ignite:
                int explodeDamage = stack.baseDamagePerTick * 5; // 3倍伤害
                TakeDamage(explodeDamage, false);

                stack.stacks = 0;
                stack.duration = 0;
                ClearAilmentVisuals(type);
                break;

            case AilmentType.Chill:
                // ✅ 只有玩家，并且有冷却
                if (!(this is PlayerStats) || chillAuraTriggered)
                {
                    stack.stacks = 0;
                    stack.duration = 0;
                    ClearAilmentVisuals(type);
                    return;
                }

                chillAuraTriggered = true;
                chillAuraCooldownTimer = chillAuraCooldown;

                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1000f);
                int affectedCount = 0;

                foreach (var hit in colliders)
                {
                    Enemy enemy = hit.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        Characterstats stats = enemy.GetComponent<Characterstats>();
                        if (stats != null)
                        {
                            int currentStacks = stats.GetStacks(AilmentType.Chill);
                            if (currentStacks < maxAilmentStacks - 1)
                            {
                                stats.AddAilmentStack(AilmentType.Chill, stack.baseDamagePerTick);
                                affectedCount++;
                            }
                        }
                    }
                }

                stack.stacks = 0;
                stack.duration = 0;
                ClearAilmentVisuals(type);
                break;

            case AilmentType.Shock:
                HitNearestTargetWithShockStrike();
                break;
        }
    }

    protected virtual void SetAilmentVisuals(AilmentType type)
    {
        if (fx == null) return;

        switch (type)
        {
            case AilmentType.Ignite:
                fx.IgniteFxFor(ailmentBaseDuration);
                break;
            case AilmentType.Chill:
                fx.ChillFxFor(ailmentBaseDuration);
                break;
            case AilmentType.Shock:
                fx.ShockFxFor(ailmentBaseDuration);
                break;
        }
    }

    public virtual void IncreaseStatBy(int _modifier, float _duration, Stats _statsToModify)
    {
        StartCoroutine(StatModCoroutine(_modifier, _duration, _statsToModify));
    }

    private IEnumerator StatModCoroutine(int _modifier, float _duration, Stats _statsToModify)
    {
        _statsToModify.AddModifier(_modifier);
        yield return new WaitForSeconds(_duration);
        _statsToModify.RemoveModifier(_modifier);
    }

    public virtual void DoDamage(Characterstats _targetStats)
    {
        if (_targetStats.CanAvoidAttack(this))
            return;

        _targetStats.GetComponent<Entity>().SetupKnockbackDir(transform);

        int totalDamage = damage.getValue() + strength.getValue();

        if (canCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
        }

        totalDamage = Mathf.RoundToInt(totalDamage * _targetStats.incomingDamageMultiplier);
        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);

        doMagicDamage(_targetStats);
    }

    public virtual void doMagicDamage(Characterstats _targetStats)
    {
        int _fireDamage = fireDamage.getValue();
        int _iceDamage = iceDamage.getValue();
        int _lightningDamage = lightningDamage.getValue();

        int totalMagicDamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.getValue();
        totalMagicDamage = CheckTargetResistance(_targetStats, totalMagicDamage);
        totalMagicDamage = Mathf.RoundToInt(totalMagicDamage * _targetStats.incomingDamageMultiplier);
        _targetStats.TakeDamage(totalMagicDamage);

        // 100%触发，根据最高元素伤害施加DOT
        AttemptToApplyAilments(_targetStats, _fireDamage, _iceDamage, _lightningDamage);
    }

    protected virtual void AttemptToApplyAilments(Characterstats _targetStats, int _fireDamage, int _iceDamage, int _lightningDamage)
    {
        // 找出最高的元素伤害类型
        int maxDamage = Mathf.Max(_fireDamage, _iceDamage, _lightningDamage);
        if (maxDamage <= 0) return;

        // 100%触发，没有概率
        if (_fireDamage == maxDamage && _fireDamage > 0)
        {
            int igniteDamage = Mathf.RoundToInt(_fireDamage * 0.15f);
            _targetStats.AddAilmentStack(AilmentType.Ignite, igniteDamage);
        }
        else if (_iceDamage == maxDamage && _iceDamage > 0)
        {
            int chillDamage = Mathf.RoundToInt(_iceDamage * 0.1f);
            _targetStats.AddAilmentStack(AilmentType.Chill, chillDamage);
        }
        else if (_lightningDamage == maxDamage && _lightningDamage > 0)
        {
            int shockDamage = Mathf.RoundToInt(_lightningDamage * 0.1f);
            _targetStats.AddAilmentStack(AilmentType.Shock, shockDamage);
            _targetStats.SetupShockStrikeDamage(Mathf.RoundToInt(_lightningDamage * 0.2f));
        }
    }

    public void SetupShockStrikeDamage(int _damage)
    {
        shockStrikeDamage = _damage;
    }

    protected virtual void HitNearestTargetWithShockStrike()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && hit.transform != transform &&
                Vector2.Distance(transform.position, hit.transform.position) > 1)
            {
                float distanceEnemy = Vector2.Distance(transform.position, hit.transform.position);
                if (distanceEnemy < closestDistance)
                {
                    closestDistance = distanceEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }

        if (closestEnemy != null)
        {
            GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);
            newShockStrike.GetComponent<ThunderSkrike_Controller>().Setup(shockStrikeDamage, closestEnemy.GetComponent<Characterstats>());
        }
    }

    protected virtual int CheckTargetResistance(Characterstats _targetStats, int totalMagicDamage)
    {
        totalMagicDamage -= _targetStats.magicResitance.getValue() + (_targetStats.intelligence.getValue() * 3);
        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 0, int.MaxValue);
        return totalMagicDamage;
    }

    protected virtual int CheckTargetArmor(Characterstats _targetStats, int totalDamage)
    {
        if (_targetStats.isChilled)
        {
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.getValue() * 0.8f);
        }
        else
        {
            totalDamage -= _targetStats.armor.getValue();
        }

        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    protected virtual bool CanAvoidAttack(Characterstats _attackerStats)
    {
        int totalEvasion = evasion.getValue() + agility.getValue();

        if (isShocked)
        {
            totalEvasion += 20;
        }

        return Random.Range(0, 100) < totalEvasion;
    }

    public virtual void TakeDamage(int _damage, bool triggerEffects = true)
    {
        if(isInvincible)
        {
            return;
        }

        DecreaseHealthBy(_damage);

        if (triggerEffects)
        {
            GetComponent<Entity>()?.DamageImpact();
            fx?.StartCoroutine("FlashFx");
        }

        if (currentHp <= 0 && !isDead)
        {
            Die();
        }
    }

    public virtual void TakeDamage(int _damage)
    {
        TakeDamage(_damage, true);
    }

    public virtual void IncreaseHealthBy(int _amount)
    {
        currentHp += _amount;
        if (currentHp > GetMaxHp())
        {
            currentHp = GetMaxHp();
        }
        onHeathChanged?.Invoke();
    }

    protected virtual void DecreaseHealthBy(int _damage)
    {
        currentHp -= _damage;
        onHeathChanged?.Invoke();
    }

    protected virtual void Die()
    {
        isDead = true;
        foreach (var stack in ailmentStacks.Values)
        {
            stack.stacks = 0;
            stack.duration = 0;
        }
    }

    public void KillEntity()
    {
        if (!isDead) 
        { 
            Die();
        }
    }

    public void MakeInvincible(bool _invincible)
    {
        isInvincible = _invincible;
    }

    private bool canCrit()
    {
        int totalCriticalChance = critChance.getValue() + agility.getValue();
        return Random.Range(0, 100) < totalCriticalChance;
    }

    private int CalculateCriticalDamage(int _damage)
    {
        float totalCritPower = critPower.getValue() + strength.getValue() * 0.01f;
        float criticalDamage = _damage * totalCritPower * 0.01f;
        return Mathf.RoundToInt(criticalDamage);
    }

    public int GetMaxHp()
    {
        return maxHp.getValue() + vitality.getValue() * 5;
    }

    public Stats GetStat(StatType _statType)
    {
        switch (_statType)
        {
            case StatType.strength:
                return strength;
            case StatType.agllity:
                return agility;
            case StatType.intelligence:
                return intelligence;
            case StatType.vitality:
                return vitality;
            case StatType.damage:
                return damage;
            case StatType.cirtChance:
                return critChance;
            case StatType.cirtPower:
                return critPower;
            case StatType.health:
                return maxHp;
            case StatType.armor:
                return armor;
            case StatType.evasion:
                return evasion;
            case StatType.magicRes:
                return magicResitance;
            case StatType.fireDamage:
                return fireDamage;
            case StatType.iceDamage:
                return iceDamage;
            case StatType.lightningDamage:
                return lightningDamage;
            default:
                return null;
        }
    }
}