using FlaxEngine;

public class FreeCamera : Script
{
    [Limit(0, 100), Tooltip("Camera movement speed factor")]
    public float MoveSpeed { get; set; } = 4;

    [Tooltip("Camera rotation smoothing factor")]
    public float CameraSmoothing { get; set; } = 20.0f;

    private float pitch;
    private float yaw;
    private bool _mouseGrabbed = true;

    public override void OnStart()
    {
        var initialEulerAngles = Actor.Orientation.EulerAngles;
        pitch = initialEulerAngles.X;
        yaw = initialEulerAngles.Y;
    }

    public override void OnUpdate()
    {
        Screen.CursorVisible = !_mouseGrabbed;
        Screen.CursorLock = _mouseGrabbed ? CursorLockMode.Locked : CursorLockMode.None;
        if (Input.GetMouseButtonUp(MouseButton.Right))
            _mouseGrabbed = !_mouseGrabbed;

        if (_mouseGrabbed)
        {
            var mouseDelta = new Float2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            pitch = Mathf.Clamp(pitch + mouseDelta.Y, -88, 88);
            yaw += mouseDelta.X;
        }
    }

    public override void OnFixedUpdate()
    {
        if (!_mouseGrabbed)
            return;
        var camTrans = Actor.Transform;
        var camFactor = Mathf.Saturate(CameraSmoothing * Time.DeltaTime);

        camTrans.Orientation = Quaternion.Lerp(camTrans.Orientation, Quaternion.Euler(pitch, yaw, 0), camFactor);

        var inputH = Input.GetAxis("Horizontal");
        var inputV = Input.GetAxis("Vertical");
        var move = new Vector3(inputH, 0.0f, inputV);
        move.Normalize();
        move = camTrans.TransformDirection(move);

        camTrans.Translation += move * MoveSpeed;

        Actor.Transform = camTrans;
    }
}
