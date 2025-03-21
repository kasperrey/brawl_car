using UnityEngine;
using System.Collections.Generic;
    
public class SimpleCarController : MonoBehaviour {
    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxMotorTorque; // maximum torque the motor can apply to wheel
    public float maxSteeringAngle; // maximum steer angle the wheel can have

    private CarInputs _input;

    private void Start()
    {
        _input = GetComponent<CarInputs>();
        if (_input == null)
        {
            Debug.LogError("CarInputs component not found on the same GameObject as SimpleCarController!");
            enabled = false;
            return;
        }
    }

    private void FixedUpdate()
    {
        float motor = maxMotorTorque * _input.movement.y;
        float steering = maxSteeringAngle * _input.movement.x;
            
        foreach (AxleInfo axleInfo in axleInfos) {
            if (axleInfo.steering) {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor) {
                // Reset motor torque when there's no input
                if (Mathf.Abs(_input.movement.y) < 0.1f)
                {
                    axleInfo.leftWheel.motorTorque = 0;
                    axleInfo.rightWheel.motorTorque = 0;
                }
                else
                {
                    axleInfo.leftWheel.motorTorque = motor;
                    axleInfo.rightWheel.motorTorque = motor;
                }
            }
        }
    }
}
    
[System.Serializable]
public class AxleInfo {
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor; // is this wheel attached to motor?
    public bool steering; // does this wheel apply steer angle?
}
