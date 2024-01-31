using FlaxEngine;

namespace Game;

/// <summary>
/// Scale Script.
/// </summary>
public class Scale : Script
{
    private RigidBody _rigidBody;
    [NumberCategory(Utils.ValueCategory.Angle)]
    public float Angle;

    [Range(0, 100000)]
    [NumberCategory(Utils.ValueCategory.Torque)]
    public float ApplyForce = 50000;
    [NumberCategory(Utils.ValueCategory.Torque)]
    public Vector3 Torque = new(0, 0, 45000);
    public Vector3 AngularVelocity;

    /// <inheritdoc/>
    public override void OnStart()
    {
        _rigidBody = Actor.As<RigidBody>();
        // we need to set the max. angular velocity to a higher value as it
        // limits the applied torque and the scale will never reach a balance
        _rigidBody.MaxAngularVelocity = 14f;
    }
    
    public override void OnFixedUpdate()
    {
        BalanceScale();
    }

    private void BalanceScale()
    {
        AngularVelocity = _rigidBody.AngularVelocity;
        Angle = _rigidBody.Orientation.EulerAngles.Z;
        //Torque.Z = ApplyForce;
        if (Mathf.Abs(Angle) > 0.01f)
            Torque.Z *= (1f - Angle/36000f);
        Torque.Z = Mathf.Clamp(Torque.Z, 1, 100000);
        _rigidBody.AddTorque(Torque);
    }
}
