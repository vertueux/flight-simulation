using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aircraft : MonoBehaviour {
    public float maxThrust = 400;
    public float throttle = 0.5f; // Between [0;1].
    private Rigidbody body;
    private Vector3 velocity;
    private Vector3 lastVelocity;
    private Vector3 localGForce;
    private Vector3 localVelocity;
    private Vector3 localAngularVelocity;
    [SerializeField] public float angleOfAttack = 0.0f;
    [SerializeField] public float angleOfAttackYaw = 0.0f;

    void Start() {
        body = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        float dt = Time.fixedDeltaTime;

        CalculateState(dt);
        CalculateAngleOfAttack();
        CalculateGForce(dt);
    }

    void CalculateState(float dt) {
        var invRotation = Quaternion.Inverse(body.rotation);
        velocity = body.linearVelocity;
        localVelocity = invRotation * velocity;
        localAngularVelocity = invRotation * body.angularVelocity;
    }

    void CalculateAngleOfAttack() {
        if (localVelocity.sqrMagnitude < 0.1f) {
            angleOfAttack = 0.0f;
            angleOfAttackYaw = 0.0f;
        }

        angleOfAttack = Mathf.Atan2(-localVelocity.y, localVelocity.z);
        angleOfAttackYaw = Mathf.Atan2(localAngularVelocity.x, localVelocity.z);
    }

    void CalculateGForce(float dt) {
        var invRotation = Quaternion.Inverse(body.rotation);
        var acceleration = (velocity - lastVelocity) / dt;
        localGForce = invRotation * acceleration;
        lastVelocity = velocity;
    }

    void UpdateThrust() {
        body.AddRelativeForce(throttle * maxThrust * Vector3.forward);
    }
}
