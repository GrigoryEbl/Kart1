using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCar : MonoBehaviour
{
    [SerializeField] private WheelCollider[] _frontWheels;
    [SerializeField] private WheelCollider[] _rearWheels;

    [SerializeField] private Transform[] _frontWheelsModels;
    [SerializeField] private Transform[] _rearWheelsModels;

    [Header("Physics")]
    [SerializeField] private Transform _centerOfMass;
    [SerializeField] private float _torque;
    [SerializeField] private float _maxAngle;
    [SerializeField] private float _ackermannFactor;

    [SerializeField] private float _debug;

    private void Awake()
    {
        GetComponent<Rigidbody>().centerOfMass = _centerOfMass.localPosition;
    }

    private void Update()
    {
        var steering = Input.GetAxis("Horizontal");
        var acceleration = Input.GetAxis("Vertical");

        foreach (var item in _rearWheels)
        {
            item.motorTorque = acceleration * _torque;
        }

        for (int i = 0; i < _frontWheels.Length; i++)
        {
            float sign = (i == 0) ? -1f : 1f;
            float factor = 1;

            if (Mathf.Approximately (sign , Mathf.Sign(_debug)))
                factor = _ackermannFactor;

            _frontWheels[i].steerAngle = steering * _maxAngle * factor;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        foreach (var item in _rearWheels)
        {
            Gizmos.DrawRay(item.transform.position, item.transform.right * 10);
            Gizmos.DrawRay(item.transform.position, -item.transform.right * 10);
        }

        for (int i = 0; i < _frontWheels.Length; i++)
        {
            Quaternion rotation;

            rotation = Quaternion.AngleAxis(_frontWheels[i].steerAngle, Vector3.up);

            Gizmos.DrawRay(_frontWheels[i].transform.position, rotation * (_frontWheels[i].transform.right * 10));
            Gizmos.DrawRay(_frontWheels[i].transform.position, rotation * (-_frontWheels[i].transform.right * 10));
        }

    }

}
