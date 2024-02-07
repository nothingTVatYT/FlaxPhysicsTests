using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// ControllerView Script.
/// </summary>
public class ControllerView : Script
{
    private CharacterController _controller;
    private bool _isColliding;

    /// <inheritdoc/>
    public override void OnStart()
    {
        _controller = Actor.As<CharacterController>();
    }
    
    /// <inheritdoc/>
    public override void OnEnable()
    {
        _controller.CollisionEnter += OnCollisionEnter;
        _controller.CollisionExit += OnCollisionExit;
        _controller.TriggerEnter += OnTriggerEnter;
        _controller.TriggerExit += OnTriggerExit;
    }

    /// <inheritdoc/>
    public override void OnDisable()
    {
        _controller.CollisionEnter -= OnCollisionEnter;
        _controller.CollisionExit -= OnCollisionExit;
        _controller.TriggerEnter -= OnTriggerEnter;
        _controller.TriggerExit -= OnTriggerExit;
    }

    /// <inheritdoc/>
    public override void OnUpdate()
    {
        // Here you can add code that needs to be called every frame
    }

    public override void OnDebugDraw()
    {
        if (_controller == null)
            return;

        var color = _isColliding ? Color.Orange : Color.Aquamarine;
        DebugDraw.DrawWireSphere(new BoundingSphere(Actor.Position + _controller.Center, _controller.Radius), color);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"collision enter event on {Actor.Name}");
        _isColliding = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        _isColliding = false;
    }

    private void OnTriggerEnter(PhysicsColliderActor actor)
    {
        Debug.Log($"trigger enter event on {Actor.Name}");
        _isColliding = true;
    }

    private void OnTriggerExit(PhysicsColliderActor actor)
    {
        _isColliding = false;
    }
}
