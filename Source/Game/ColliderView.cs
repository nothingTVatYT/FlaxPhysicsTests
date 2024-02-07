using FlaxEngine;

namespace Game;

/// <summary>
/// ColliderView Script.
/// </summary>
public class ColliderView : Script
{
    private Collider _collider;
    private bool _isColliding;

    /// <inheritdoc/>
    public override void OnStart()
    {
        _collider = Actor.As<Collider>();
    }
    
    /// <inheritdoc/>
    public override void OnEnable()
    {
        _collider.CollisionEnter += OnCollisionEnter;
        _collider.CollisionExit += OnCollisionExit;
        _collider.TriggerEnter += OnTriggerEnter;
        _collider.TriggerExit += OnTriggerExit;
    }

    /// <inheritdoc/>
    public override void OnDisable()
    {
        _collider.CollisionEnter -= OnCollisionEnter;
        _collider.CollisionExit -= OnCollisionExit;
        _collider.TriggerEnter -= OnTriggerEnter;
        _collider.TriggerExit -= OnTriggerExit;
    }

    /// <inheritdoc/>
    public override void OnUpdate()
    {
        // Here you can add code that needs to be called every frame
    }

    public override void OnDebugDraw()
    {
        if (_collider == null)
            return;

        var color = _isColliding ? Color.Orange : Color.Aquamarine;
        if (_collider is BoxCollider boxCollider)
            DebugDraw.DrawWireBox(boxCollider.Box, color);
        else if (_collider is SphereCollider sphereCollider)
            DebugDraw.DrawWireSphere(sphereCollider.Sphere, color);
        else if (_collider is CapsuleCollider capsuleCollider)
            DebugDraw.DrawWireSphere(new BoundingSphere(capsuleCollider.Center, capsuleCollider.Radius), color);
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
