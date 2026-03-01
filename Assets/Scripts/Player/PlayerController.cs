using UnityEngine;

[RequireComponent(typeof(ChariotController))]
public class PlayerController : MonoBehaviour
{
    InputSystem_Actions inputs;
    ChariotController chariotController;
    
    private void Awake() {
        inputs = new();
        chariotController = GetComponent<ChariotController>();
    }

    private void OnEnable() {
        inputs.Player.Enable();
    }

    private void OnDisable() {
        inputs.Player.Disable();
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        chariotController.Move(inputs.Player.Move.ReadValue<Vector2>());
    }
}
