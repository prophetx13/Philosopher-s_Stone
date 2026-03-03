using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    public static RaceManager Instance {get; private set;}
    [SerializeField]
    float startDelay = 5f;
    [SerializeField]
    ChariotController playerCC;
    [SerializeField]
    GameObject cpuCC_Container;
    public int NumLapsToComplete = 1;

    [Header("Win Lose Canvas")]
    [SerializeField]
    GameObject winCanvas;

    List<ChariotController> list_cpuCC = new();



    private void Awake() {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }

    void Start()
    {
        cpuCC_Container.GetComponentsInChildren<ChariotController>(list_cpuCC);

        playerCC.numLapsToComplete = NumLapsToComplete;
        foreach (var cpuCC in list_cpuCC)
        {
            cpuCC.numLapsToComplete = NumLapsToComplete;
        }
        StartCoroutine(DelayStart());
    }

    IEnumerator DelayStart()
    {
        playerCC.DisableMove(true);
        foreach (var cpuCC in list_cpuCC)
        {
            cpuCC.DisableMove(true);
        }
        yield return new WaitForSeconds(startDelay);
        playerCC.DisableMove(false);
        foreach (var cpuCC in list_cpuCC)
        {
            cpuCC.DisableMove(false);
        }
    }

    public void DisplayWinCanvas()
    {
        winCanvas.SetActive(true);
    }
}
