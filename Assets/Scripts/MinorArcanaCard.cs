using UnityEngine;

[CreateAssetMenu(fileName = "MinorArcanaCard", menuName = "Scriptable Objects/MinorArcanaCard")]
public class MinorArcanaCard : ScriptableObject
{
    public enum Suit { Wands, Cups, Swords, Pentacles }

    public enum TargetType { Self, Opponent, Environment, Global }

    public string cardName;
    public Suit suit;
    [Range(1, 10)] public int rank = 1;

    public TargetType targetType = TargetType.Self;
    public float duration;
    public float cooldown;

    public CardEffect effect;

    [Tooltip("Main illustration for the card face.")]
    public Sprite cardArt;

    [Tooltip("S,C,P,W).")]
    public Sprite suitIcon;

    [Tooltip("Optional sprite for the card back")]
    public Sprite cardBack;

    [Tooltip("Optional frame/border overlay sprite.")]
    public Sprite frame;

    [Header("Effect Text")]
    [TextArea(1, 3)] public string effectText;      // short: "WANDS 5 — Speed of Aries"
    [TextArea(2, 6)] public string effectDetails;   

    public abstract class CardEffect : ScriptableObject
    {

    }
}

