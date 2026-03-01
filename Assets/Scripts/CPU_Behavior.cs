using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(ChariotController))]
public class CPU_Behavior : MonoBehaviour
{
    [SerializeField]
    [Min(0.001f)]
    float acceptableDistanceRange = 5;
    ChariotController chariotController;
    [SerializeField]
    SplineContainer guideLineContainer;
    Spline guideLine;
    int targetKnotIndex;
    Vector2 newDir = Vector2.zero;
    private void Awake() {
        chariotController = GetComponent<ChariotController>();
        guideLine = guideLineContainer.Spline;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateNewDir();
        chariotController.Move(newDir);
    }

    void UpdateNewDir()
    {
        int maxKnots = guideLine.Count;
        Vector3 targetKnotPos = (Vector3)guideLine[targetKnotIndex].Position;
        if (Vector3.Distance(transform.position, targetKnotPos) < acceptableDistanceRange)
        {
            targetKnotIndex = (targetKnotIndex + 1) % maxKnots;
        }
        Vector3 temp = (targetKnotPos - transform.position).normalized;
        newDir = new (temp.x, temp.z);
        Debug.DrawLine(transform.position, targetKnotPos, Color.purple, Time.deltaTime);

    }
}
