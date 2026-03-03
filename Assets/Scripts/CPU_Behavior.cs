using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(ChariotController))]
public class CPU_Behavior : MonoBehaviour
{
    [SerializeField]
    [Min(0.001f)]
    float acceptableDistance = 5;

    [SerializeField]
    [Min(0)]
    [Tooltip("How often the CPU will update it's direction")]
    int numFramesForUpdate = 1;

    [SerializeField]
    SplineContainer guideLineContainer;

    ChariotController chariotController;
    Spline guideLine;
    int targetKnotIndex;
    Vector2 newDir = Vector2.zero;
    int frameCounter = 0;

    float unstuckCheckInterval = 10f;
    Vector3 lastPos = Vector3.zero;

    private void Awake() {
        chariotController = GetComponent<ChariotController>();
        guideLine = guideLineContainer.Spline;
    }

    void Start()
    {
        lastPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if ((int)Time.time % unstuckCheckInterval == 0) Unstuck();

        if (frameCounter % numFramesForUpdate == 0) UpdateNewDir();
        chariotController.Move(newDir);
        frameCounter++;
    }

    void UpdateNewDir()
    {
        int maxKnots = guideLine.Count;
        Vector3 targetKnotPos = (Vector3)guideLine[targetKnotIndex].Position;
        Vector3 nextTargetKnotPos = (Vector3)guideLine[(targetKnotIndex + 1) % maxKnots].Position;
        Debug.DrawLine(transform.position, nextTargetKnotPos, Color.yellow, numFramesForUpdate * Time.fixedDeltaTime);
        float distToTargetKnot = Vector3.Distance(transform.position, targetKnotPos);
        float distToNextKnot = Vector3.Distance(transform.position, nextTargetKnotPos);

        if (distToNextKnot < distToTargetKnot || distToTargetKnot < acceptableDistance)
        {
            targetKnotIndex = (targetKnotIndex + 1) % maxKnots;
        }

        Vector3 temp = (targetKnotPos - transform.position).normalized;
        newDir = new (temp.x, temp.z);
        Debug.DrawLine(transform.position, targetKnotPos, Color.purple, numFramesForUpdate * Time.fixedDeltaTime);
    }

    void Unstuck()
    {
        if (Vector3.Distance(transform.position, lastPos) < 1f)
        {
            chariotController.Death();
        }
    }

}
