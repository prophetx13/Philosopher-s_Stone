using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ChariotController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI tmp;
    InputSystem_Actions inputs;
    ChariotController chariotController;

    bool canCrossFinishLine = true;
    int currentLap = 0;
    
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

    // Update is called once per frame
    void FixedUpdate()
    {
        chariotController.Move(inputs.Player.Move.ReadValue<Vector2>());
    }

    private void Boost(InputAction.CallbackContext context)
    {
        chariotController.Boost();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("FinishLine") && canCrossFinishLine)
        {
            currentLap++;
            tmp.text = currentLap.ToString();
        }

        if (other.CompareTag("EnableFinishLine"))
        {
            canCrossFinishLine = true;
        }
    }
}
