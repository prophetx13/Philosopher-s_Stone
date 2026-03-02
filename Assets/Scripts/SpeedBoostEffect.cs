using UnityEngine;

[CreateAssetMenu(fileName = "SpeedBoostEffect", menuName = "Tarot/Effects/Speed Boost")]
public class SpeedBoostEffect : CardEffect
{
    [SerializeField] private float boostAmount = 10f;

    public override void Activate(CardEffectContext context)
    {
        Debug.Log($"Boost activated by {context.caster?.name}, amount: {boostAmount}");
    }
}
