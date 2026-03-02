using UnityEngine;
using TMPro;
using DG.Tweening;

public class HandEffectLabel : MonoBehaviour
{
    [SerializeField] private TMP_Text tmp;
    [SerializeField] private Vector3 worldOffset = new Vector3(0f, 0.25f, 0f);
    [SerializeField] private float followSpeed = 25f;

    [Header("Animation")]
    [SerializeField] private float fadeTime = 0.12f;

    private Transform followTarget;

    private void Awake()
    {
        if (tmp == null) tmp = GetComponent<TMP_Text>();
        SetAlpha(0f);
    }

    private void LateUpdate()
    {
        if (followTarget == null) return;

        Vector3 targetPos = followTarget.position + worldOffset;
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);

        // Optional: face camera for readability
        if (Camera.main != null)
            transform.forward = Camera.main.transform.forward;
    }

    public void SetTarget(Transform t)
    {
        followTarget = t;
    }

    public void Show(string text)
    {
        if (tmp == null) return;

        tmp.text = text;

        tmp.DOKill();
        tmp.DOFade(1f, fadeTime).SetEase(Ease.OutQuad);
    }

    public void Hide()
    {
        if (tmp == null) return;

        tmp.DOKill();
        tmp.DOFade(0f, fadeTime).SetEase(Ease.OutQuad);
    }

    private void SetAlpha(float a)
    {
        if (tmp == null) return;
        var c = tmp.color;
        c.a = a;
        tmp.color = c;
    }
}
