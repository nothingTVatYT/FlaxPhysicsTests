using FlaxEngine;

namespace Game;

/// <summary>
/// SpringTest Script.
/// </summary>
public class SpringTest : Script
{
    [NumberCategory(Utils.ValueCategory.Force)]
    [Range(0, 20000)]
    public float OverrideBreakForce = 20000;

    private float _previousBreakForce;

    private DistanceJoint _joint;

    /// <inheritdoc/>
    public override void OnStart()
    {
        _joint = Actor.As<DistanceJoint>();
        _previousBreakForce = _joint.BreakForce;
    }
    
    /// <inheritdoc/>
    public override void OnFixedUpdate()
    {
        if (Mathf.Abs(_previousBreakForce - OverrideBreakForce) > 1)
        {
            _joint.BreakForce = OverrideBreakForce;
            _previousBreakForce = OverrideBreakForce;
            (_joint.Target as RigidBody)?.WakeUp();
        }
    }
}
