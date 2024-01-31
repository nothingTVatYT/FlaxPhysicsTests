using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using FlaxEngine;

namespace Game;

/// <summary>
/// FallingObject Script.
/// </summary>
public class FallingObject : Script
{
    private float _startFalling;
    [NumberCategory(Utils.ValueCategory.Acceleration)]
    public float SimulatedGravity;
    [NumberCategory(Utils.ValueCategory.Speed)]
    public float MaxVelocity;
    [NumberCategory(Utils.ValueCategory.Speed)]
    public float ExpectedMaxVelocity;
    [NumberCategory(Utils.ValueCategory.Distance)]
    public float DistanceTraveled;
    [NumberCategory(Utils.ValueCategory.Time)]
    public float FallTime;
    [NumberCategory(Utils.ValueCategory.Time)]
    public float ExpectedFallTime;
    [NumberCategory(Utils.ValueCategory.Volume)]
    public float Volume;
    [NumberCategory(Utils.ValueCategory.Mass)]
    public float Mass;
    [Tooltip("Density in g/cm³")]
    public float Density;
    [Tooltip("Force needed to keep the object hovering")]
    [NumberCategory(Utils.ValueCategory.Force)]
    public float LiftForce = 510000;
    [Tooltip("Expected lift force")]
    [NumberCategory(Utils.ValueCategory.Force)]
    public float ExpectedLiftForce;
    private RigidBody _rigidBody;
    private float _startY;
    private Vector3 _lastTravelDirection;
    private float _targetHeight = 300;
    private bool _measured;

    /// <inheritdoc/>
    public override void OnStart()
    {
        _rigidBody = Actor.As<RigidBody>();
        _startY = _rigidBody.Position.Y;
        _startFalling = Time.GameTime;
        _lastTravelDirection = Vector3.Down;
        SimulatedGravity = Physics.Gravity.Length;
        if (Actor.TryGetChild(out SphereCollider collider))
            Volume = collider.Radius * collider.Radius * collider.Radius * 4 / 3 * Mathf.Pi;
        Mass = _rigidBody.Mass;
        // assuming volume is in cm³, density is g/cm³
        if (Volume > 0)
            Density = Mass * 1000 / Volume;
        ExpectedLiftForce = Mass * -Physics.Gravity.Y;
        // set damping to 0 for the free fall test
        _rigidBody.LinearDamping = 0;
    }

    /// <inheritdoc/>
    public override void OnEnable()
    {
    }

    /// <inheritdoc/>
    public override void OnDisable()
    {
        // Here you can add code that needs to be called when script is disabled (eg. unregister from events)
    }

    /// <inheritdoc/>
    public override void OnFixedUpdate()
    {
        Measure();
    }

    private void Measure()
    {
        MaxVelocity = Mathf.Max(MaxVelocity, _rigidBody.LinearVelocity.Length);
        // stopped falling?
        if (!_measured && Vector3.Dot(_lastTravelDirection, _rigidBody.LinearVelocity) < 0)
        {
            DistanceTraveled = Mathf.Abs(_rigidBody.Position.Y - _startY);
            FallTime = Time.GameTime - _startFalling;
            ExpectedFallTime = Mathf.Sqrt(2 * DistanceTraveled / Physics.Gravity.Length);
            ExpectedMaxVelocity = Physics.Gravity.Length * FallTime;
            _measured = true;
            // set a high damping for the hovering test
            _rigidBody.LinearDamping = 0.1f;
        }

        if (_measured)
        {
            // start simulation of a floating object
            var heightDiff = _targetHeight - _rigidBody.Position.Y;
            var rel = Mathf.Clamp(heightDiff / _targetHeight, -0.2f, 0.2f);
            var yVelocity = _rigidBody.LinearVelocity.Y;
            LiftForce = Mass * -(yVelocity / 100f + Physics.Gravity.Y);
            LiftForce *= 1 + rel;
            _rigidBody.AddForce(LiftForce * Vector3.Up);
        }
    }
}
