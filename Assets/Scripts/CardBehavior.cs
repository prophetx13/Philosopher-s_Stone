using UnityEngine;

public class CardBehavior : MonoBehaviour, ICardActivatable
{
    public void Activate(HandManager hand)
    {
        Debug.Log($"Activated card: {name}");

        // TODO: call real effect system 
    }
}
