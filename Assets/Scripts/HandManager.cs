using UnityEngine;
using UnityEngine.Splines;
using System.Collections.Generic;
using DG.Tweening;

public class HandManager : MonoBehaviour
{
    [Header("Hand")]
    [SerializeField] private int maxHandSize = 5;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private DeckDirector deckDirector;

    [Header("Layout")]
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private Transform spawnPoint;

    [Header("Selection + Highlight")]
    [SerializeField] private float selectedScale = 1.1f;
    [SerializeField] private float selectedPopDistance = 0.15f;
    [SerializeField] private float tweenTime = 0.25f;

    [Header("Input")]
    [SerializeField] private KeyCode drawTestKey = KeyCode.Space;     
    [SerializeField] private KeyCode prevCardKey = KeyCode.Q;
    [SerializeField] private KeyCode nextCardKey = KeyCode.E;
    [SerializeField] private KeyCode activateKey = KeyCode.F;

    [SerializeField] private HandEffectLabel effectLabel;

    private readonly List<GameObject> handCards = new();
    private int selectedIndex = -1;

    private void Update()
    {
        // (Temporary) draw test
        if (Input.GetKeyDown(drawTestKey)) DrawCard();

        HandleNumberKeySelection();

        if (Input.GetKeyDown(prevCardKey)) CycleSelection(-1);
        if (Input.GetKeyDown(nextCardKey)) CycleSelection(+1);

        if (Input.GetKeyDown(activateKey)) ActivateSelectedCard();
    }

    // INPUT

    private void HandleNumberKeySelection()
    {
        if (handCards.Count == 0) return;

        for (int n = 1; n <= 9; n++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + n) || Input.GetKeyDown(KeyCode.Keypad0 + n))
            {
                int index = n - 1;
                if (index < handCards.Count)
                {
                    SelectCard(index);
                }
            }
        }
    }

    private void CycleSelection(int direction)
    {
        if (handCards.Count == 0) return;

        if (selectedIndex < 0) selectedIndex = 0;

        selectedIndex += direction;

        if (selectedIndex < 0) selectedIndex = handCards.Count - 1;
        if (selectedIndex >= handCards.Count) selectedIndex = 0;

        UpdateCardPositions();
    }

    private void SelectCard(int index)
    {
        if (index < 0 || index >= handCards.Count) return;
        selectedIndex = index;
        UpdateCardPositions();
    }

    // Hand Config

    private void DrawCard()
    {
        if (handCards.Count >= maxHandSize) return;

        if (deckDirector == null)
        {
            Debug.LogWarning("HandManager: No DeckDirector assigned.");
            return;
        }

        MinorArcanaCard drawn = deckDirector.Draw();
        if (drawn == null) return;

        GameObject g = Instantiate(cardPrefab, spawnPoint.position, spawnPoint.rotation);

        // Assign the drawn card data to the prefab
        if (g.TryGetComponent<CardView>(out var view))
            view.SetData(drawn);
        else
            Debug.LogWarning("Card prefab missing CardView component.");

        handCards.Add(g);

        if (selectedIndex < 0) selectedIndex = 0;
        UpdateCardPositions();

        Debug.Log($"DREW CARD: {drawn.cardName} | Suit: {drawn.suit} | Rank: {drawn.rank} | Asset: {drawn.name}");
    }

    

private void ActivateSelectedCard()
    {
        if (handCards.Count == 0) return;
        selectedIndex = Mathf.Clamp(selectedIndex, 0, handCards.Count - 1);

        GameObject selected = handCards[selectedIndex];
        if (selected == null) return;

        if (selected.TryGetComponent<ICardActivatable>(out var activatable))
        {
            activatable.Activate(this); // pass context if needed
        }
        else
        {
            Debug.LogWarning($"Selected card '{selected.name}' has no ICardActivatable component.");
        }

        // Optional
        DiscardCardAt(selectedIndex);
    }

    private void DiscardCardAt(int index)
    {
        if (index < 0 || index >= handCards.Count) return;

        GameObject card = handCards[index];
        handCards.RemoveAt(index);

        if (card != null) Destroy(card);

        if (handCards.Count == 0)
        {
            selectedIndex = -1;
            return;
        }

        // Keep selection sensible (stay on same slot if possible)
        selectedIndex = Mathf.Clamp(selectedIndex, 0, handCards.Count - 1);
        UpdateCardPositions();
    }

    // -------------------- LAYOUT --------------------

    private void UpdateCardPositions()
    {
        if (handCards.Count == 0) return;
        selectedIndex = Mathf.Clamp(selectedIndex, 0, handCards.Count - 1);

        float cardSpacing = 1f / maxHandSize;
        float firstCardPosition = 0.5f - (handCards.Count - 1) * cardSpacing / 2f;

        Spline spline = splineContainer.Spline;

        // Pop direction: toward camera if available, otherwise up
        Vector3 popDir = Vector3.up;
        if (Camera.main != null) popDir = -Camera.main.transform.forward;

        for (int i = 0; i < handCards.Count; i++)
        {
            float p = firstCardPosition + i * cardSpacing;

            Vector3 splinePos = spline.EvaluatePosition(p);
            Vector3 forward = spline.EvaluateTangent(p);
            Vector3 up = spline.EvaluateUpVector(p);

            Quaternion rot = Quaternion.LookRotation(up, Vector3.Cross(up, forward).normalized);

            bool isSelected = (i == selectedIndex);

            Vector3 targetPos = splinePos + (isSelected ? popDir.normalized * selectedPopDistance : Vector3.zero);
            Vector3 targetScale = isSelected ? Vector3.one * selectedScale : Vector3.one;

            Transform t = handCards[i].transform;

            t.DOMove(targetPos, tweenTime).SetEase(Ease.OutQuad);
            t.DORotateQuaternion(rot, tweenTime).SetEase(Ease.OutQuad);
            t.DOScale(targetScale, tweenTime).SetEase(Ease.OutQuad);
            UpdateEffectLabel();
        }
    }

    private void UpdateEffectLabel()
    {
        if (effectLabel == null) return;

        GameObject selected = GetSelectedCard();
        if (selected == null)
        {
            effectLabel.Hide();
            return;
        }

        if (!selected.TryGetComponent<CardView>(out var view) || view.Data == null)
        {
            effectLabel.Hide();
            return;
        }

        effectLabel.SetTarget(selected.transform);
        effectLabel.Show(view.EffectText); // or $"{view.EffectText}\n<size=70%>{view.EffectDetails}</size>"
    }

    // Optional external access
    public GameObject GetSelectedCard()
    {
        if (selectedIndex < 0 || selectedIndex >= handCards.Count) return null;
        return handCards[selectedIndex];
    }


}