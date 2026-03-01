using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ChariotController : MonoBehaviour
{
    [SerializeField]
    float maxSpeed = 10;
    [SerializeField]
    float maxAcceleration = 10;
    [SerializeField]
    float maxJerk = 10;
    float currentAcceleration = 0;

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
        Vector3 newDir = new (dir.x, 0f, dir.y);
        rb.AddForce(newDir * maxSpeed, ForceMode.Acceleration);
    }
}
