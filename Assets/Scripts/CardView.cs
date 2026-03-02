using UnityEngine;

public class CardView : MonoBehaviour
{
    [Header("Render Targets (SpriteRenderers on this prefab)")]
    [SerializeField] private SpriteRenderer cardArtRenderer;     // main illustration
    [SerializeField] private SpriteRenderer suitIconRenderer;    // small suit icon (optional)
    [SerializeField] private SpriteRenderer frameRenderer;       // border overlay (optional)
    [SerializeField] private SpriteRenderer backRenderer;        // back art (optional)

    [field: SerializeField] public MinorArcanaCard Data { get; private set; }

    public string EffectText => Data != null ? Data.effectText : "";
    public string EffectDetails => Data != null ? Data.effectDetails : "";

    public void SetData(MinorArcanaCard data)
    {
        Data = data;
        ApplyVisuals();
    }

    private void ApplyVisuals()
    {
        if (Data == null) return;

        // Pull sprites directly from the card data
        if (cardArtRenderer != null) cardArtRenderer.sprite = Data.cardArt;
        if (suitIconRenderer != null) suitIconRenderer.sprite = Data.suitIcon;
        if (frameRenderer != null) frameRenderer.sprite = Data.frame;
        if (backRenderer != null) backRenderer.sprite = Data.cardBack;

        // Helpful debug
        Debug.Log($"[CardView] Applied visuals for {Data.cardName} ({Data.suit} {Data.rank}) " +
                  $"Art={(Data.cardArt ? Data.cardArt.name : "NULL")}");
    }

#if UNITY_EDITOR
    // card update in-editor 
    private void OnValidate()
    {
        if (!Application.isPlaying && Data != null)
            ApplyVisuals();
    }
#endif
}
