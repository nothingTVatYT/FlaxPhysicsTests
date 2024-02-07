using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// MoveIt Script.
/// </summary>
public class MoveIt : Script
{
    [NumberCategory(Utils.ValueCategory.Distance)]
    public Vector3 RelativePosition = Vector3.Zero;
    private Vector3 _originalPosition;
    private RigidBody _rigidBody;

    /// <inheritdoc/>
    public override void OnStart()
    {
        _originalPosition = Actor.Position;
        if (Actor is RigidBody)
            _rigidBody = Actor.As<RigidBody>();
    }

    /// <inheritdoc/>
    public override void OnEnable()
    {
        // Here you can add code that needs to be called when script is enabled (eg. register for events)
    }

    /// <inheritdoc/>
    public override void OnDisable()
    {
        // Here you can add code that needs to be called when script is disabled (eg. unregister from events)
    }

    /// <inheritdoc/>
    public override void OnUpdate()
    {
        var amount = Mathf.Sin(Time.GameTime);
        if (_rigidBody != null)
            _rigidBody.Position = Vector3.Lerp(_originalPosition, _originalPosition + RelativePosition, amount);
        else
            Actor.Position = Vector3.Lerp(_originalPosition, _originalPosition + RelativePosition, amount);
    }
}
