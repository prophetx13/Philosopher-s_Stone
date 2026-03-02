using System.Collections.Generic;
using UnityEngine;

public class DeckDirector : MonoBehaviour
{
    [Header("Deck Contents")]
    [Tooltip("All possible Minor Arcana cards available to draw from.")]
    [SerializeField] private List<MinorArcanaCard> library = new();

    [Header("Deck Behavior (MVP)")]
    [Tooltip("If true, cards go to discard and reshuffle when empty. If false, draws can repeat freely.")]
    [SerializeField] private bool useDrawAndDiscardPiles = true;

    private readonly List<MinorArcanaCard> drawPile = new();
    private readonly List<MinorArcanaCard> discardPile = new();

    private void Awake()
    {
        ResetDeck();
    }

    public void ResetDeck()
    {
        drawPile.Clear();
        discardPile.Clear();

        // MVP: 1 copy of each card in the library
        drawPile.AddRange(library);

        Shuffle(drawPile);
    }

    public MinorArcanaCard Draw()
    {
        if (library.Count == 0)
        {
            Debug.LogWarning("DeckDirector: library is empty. Add MinorArcanaCard assets.");
            return null;
        }

        if (!useDrawAndDiscardPiles)
        {
            // MVP-simple: random draw with replacement
            return library[Random.Range(0, library.Count)];
        }

        // Draw pile mode:
        if (drawPile.Count == 0)
        {
            // Reshuffle discard into draw pile
            if (discardPile.Count == 0)
            {
                // Nothing to reshuffle; reset from library
                ResetDeck();
            }
            else
            {
                drawPile.AddRange(discardPile);
                discardPile.Clear();
                Shuffle(drawPile);
            }
        }

        var card = drawPile[0];
        drawPile.RemoveAt(0);
        discardPile.Add(card);
        return card;
    }

    private static void Shuffle<T>(IList<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
