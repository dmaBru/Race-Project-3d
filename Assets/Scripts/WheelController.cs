using UnityEngine;

public class WheelController : MonoBehaviour
{
    [SerializeField] WheelCollider frontRight;
    [SerializeField] WheelCollider frontLeft;
    [SerializeField] WheelCollider rearRight;
    [SerializeField] WheelCollider rearLeft;

    [SerializeField] Transform frontRightTransform;
    [SerializeField] Transform frontLeftTransform;
    [SerializeField] Transform rearRightTransform;
    [SerializeField] Transform rearLeftTransform;

    public float acceleration = 500f;
    public float breakingForce = 300f;
    public float maxTurnAngle = 15f;

    private float currentAcceleration = 0f;
    private float currentBreakForce = 0f;
    private float currentTurnAngle = 0f;


    private void FixedUpdate()
    {

        currentAcceleration = acceleration * Input.GetAxis("Vertical") * -1;

        if (Input.GetKey(KeyCode.Space))
            currentBreakForce = breakingForce;
        else
            currentBreakForce = 0f;

        frontRight.motorTorque = currentAcceleration;
        frontLeft.motorTorque = currentAcceleration;

        frontRight.brakeTorque = currentBreakForce;
        frontLeft.brakeTorque = currentBreakForce;
        rearRight.brakeTorque = currentBreakForce;
        rearLeft.brakeTorque = currentBreakForce;

        currentTurnAngle = maxTurnAngle * Input.GetAxis("Horizontal");
        frontLeft.steerAngle = currentTurnAngle;
        frontRight.steerAngle = currentTurnAngle;

        UpdateWheelLeft(frontLeft, frontLeftTransform);
        UpdateWheelRight(frontRight, frontRightTransform);
        UpdateWheelLeft(rearLeft, rearLeftTransform);
        UpdateWheelRight(rearRight, rearRightTransform);
    }

    void UpdateWheelLeft(WheelCollider col, Transform transform)
    {
        Vector3 pos = transform.position;
        Quaternion rot = transform.rotation; 
        col.GetWorldPose(out pos, out rot);

        rot = rot * Quaternion.Euler(new Vector3(0, 90, 0));
        transform.position = pos;
        transform.rotation = rot;
    }

    void UpdateWheelRight(WheelCollider col, Transform transform)
    {
        Vector3 pos = transform.position;
        Quaternion rot = transform.rotation;
        col.GetWorldPose(out pos, out rot);

        rot = rot * Quaternion.Euler(new Vector3(0, -90, 0));
        transform.position = pos;
        transform.rotation = rot;
    }

}
