using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ChariotController : MonoBehaviour
{
    [SerializeField]
    float maxSpeed = 10;
    [SerializeField]
    float maxAcceleration = 10;
    [SerializeField]
    float jerk = 1;
    Vector3 currentAcceleration = Vector3.zero;

    Rigidbody rb;
    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Move(Vector2 dir)
    {
        if (rb.linearVelocity.magnitude <= maxSpeed)
        {
            currentAcceleration = new(dir.x * maxAcceleration, 0f, dir.y * maxAcceleration);
            rb.AddForce(currentAcceleration, ForceMode.Acceleration);
        }

        Debug.Log(rb.linearVelocity.magnitude);
    }
}
