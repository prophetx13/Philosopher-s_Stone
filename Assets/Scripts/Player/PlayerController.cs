using System;
using UnityEngine;
using UnityEngine.InputSystem;

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

        // Temporary
        inputs.Player.Jump.performed += Boost;
        
    }

    

    private void OnDisable() {
        inputs.Player.Disable();

        // Temporary
        inputs.Player.Jump.performed -= Boost;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        chariotController.Move(inputs.Player.Move.ReadValue<Vector2>());
    }

    private void Boost(InputAction.CallbackContext context)
    {
        chariotController.Boost();
    }
}
