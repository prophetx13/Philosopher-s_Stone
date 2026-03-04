using System.Collections;
using TMPro;
using UnityEngine;

public class RaceTimer : MonoBehaviour
{
    [SerializeField] 
    TextMeshProUGUI tmp;
    float startDelay = 6f;
    bool timerActive = false;

    void Start()
    {
        StartCoroutine(Timer());
    }

    void Update()
    {
        if (timerActive) UpdateText();
    }

    public IEnumerator Timer()
    {
        yield return new WaitForSeconds(startDelay);
        timerActive = true;
    }

    private void UpdateText()
    {
        float finalTime = Time.time - startDelay;
        int min = (int)finalTime / 60;
        float sec = finalTime % 60;
        tmp.text = $"{min:00}:{sec:00.00}";
    }
}
