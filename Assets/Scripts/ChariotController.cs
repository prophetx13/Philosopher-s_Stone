using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ChariotController : MonoBehaviour
{
    [Header("Default Settings")]
    [SerializeField]
    float maxSpeed = 10;
    [SerializeField]
    float maxAcceleration = 10;
    float originalMaxSpeed = 10;
    float originalMaxAcceleration = 10;
    public float CurrentSpeed {get; private set;}
    Vector3 currentAcceleration = Vector3.zero;
    Rigidbody rb;

    [Header("Boost Settings")]
    [SerializeField]
    float boostSpeedMultiplier = 2;
    [SerializeField]
    float boostAccelerationMultiplier = 1.5f;
    [SerializeField]
    [Min(0.001f)]
    float boostDuration = 1f;
    bool isBoosting = false;

    [Header("Collision and Death")]
    [SerializeField]
    [Tooltip("How much slower this chariot will be relative to other chariot, to be destroyable \nLower Value = Easier to get killed, Higher Value = Harder to get killed")]
    [Min(0.01f)]
    float minKillSpeedMultiplier;
    [SerializeField]
    [Min(0.01f)]
    float respawnDuration = 1f;
    bool ignoreChariotCollision = false;
    Vector3 lastCheckpointPosition = Vector3.zero;
    
    [Header("Animations")]
    [SerializeField]
    [Min(0.01f)]
    float distFromHorse = 1.5f;
    Animator chariotAnimator;
    Animator horseAnimator;

    [SerializeField]
    bool isMoveDisabled = false;
    private void Awake() {
        rb = GetComponent<Rigidbody>();
        chariotAnimator = GetComponent<Animator>();
        horseAnimator = transform.GetChild(0).GetComponent<Animator>();
    }
    
    void Start()
    {
        originalMaxSpeed = maxSpeed;
        originalMaxAcceleration = maxAcceleration;
    }

    void Update()
    {
        CurrentSpeed = rb.linearVelocity.magnitude;
    }

    public void Move(Vector2 dir)
    {
        if (isMoveDisabled) return;

        dir = dir.normalized;
        ProcessAnimation(dir);
        currentAcceleration = new(dir.x * maxAcceleration, 0f, dir.y * maxAcceleration);
        rb.AddForce(currentAcceleration, ForceMode.Acceleration);
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }

    public void ToggleMove()
    {
        ToggleMove(!isMoveDisabled);
    }

    public void ToggleMove(bool b)
    {
        isMoveDisabled = b;
    }

    private void ProcessAnimation(Vector2 dir)
    {
        if (chariotAnimator == null) return;
        chariotAnimator.SetFloat("XDir", dir.x);
        chariotAnimator.SetFloat("YDir", dir.y);

        horseAnimator.SetFloat("XDir", dir.x);
        horseAnimator.SetFloat("YDir", dir.y);
        
        Vector3 newHorsePos = new Vector3(dir.x, 0f, dir.y) * distFromHorse;
        if (dir == Vector2.zero) newHorsePos = new Vector3(1f, 0f, 0) * distFromHorse;
        horseAnimator.transform.localPosition = newHorsePos;
    }

    public void Boost()
    {
        if (isBoosting) return;
        StartCoroutine(BoostCoroutine());
    }

    private IEnumerator BoostCoroutine()
    {
        isBoosting = true;
        float firstPartDuration = boostDuration * 0.75f;
        float secondPartDuration = boostDuration * 0.25f;
        float boostMaxSpeed = boostSpeedMultiplier * originalMaxSpeed;
        float boostMaxAcceleration = boostAccelerationMultiplier * originalMaxAcceleration;
        maxSpeed = boostSpeedMultiplier * originalMaxSpeed;
        maxAcceleration = boostAccelerationMultiplier * originalMaxAcceleration;
        yield return new WaitForSeconds(firstPartDuration);

        float timer = 0;
        while (timer < secondPartDuration)
        {
            timer += Time.fixedDeltaTime;
            float percentComplete = timer / secondPartDuration;
            maxSpeed = Mathf.Lerp(boostMaxSpeed, originalMaxSpeed, percentComplete);
            maxAcceleration = Mathf.Lerp(boostMaxAcceleration, originalMaxAcceleration, percentComplete);
            yield return new WaitForFixedUpdate();
        }

        maxAcceleration = originalMaxAcceleration;
        maxSpeed = originalMaxSpeed;
        isBoosting = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Chariot") && !ignoreChariotCollision)
        {
            ProcessExplosion(collision.gameObject.GetComponent<ChariotController>());
        }
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Trigger: " + other);
        if (other.CompareTag("Checkpoint"))
        {
            lastCheckpointPosition = other.transform.position;
        }
    }

    private void ProcessExplosion(ChariotController otherChariot)
    {
        if (CurrentSpeed * minKillSpeedMultiplier < otherChariot.CurrentSpeed)
        {
            Death();
        }
    }

    private void Death()
    {
        StartCoroutine(ProcessDeath());
    }

    private IEnumerator ProcessDeath()
    {
        Vector3 newPos = new (lastCheckpointPosition.x, 3f, lastCheckpointPosition.z);
        transform.position = newPos;
        ignoreChariotCollision = true;
        yield return new WaitForSeconds(respawnDuration);
        ignoreChariotCollision = false;
    }
}
