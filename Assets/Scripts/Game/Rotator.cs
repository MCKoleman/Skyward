using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField]
    private float minRotationSpeed;
    [SerializeField]
    private float maxRotationSpeed;
    private float rotationSpeed;
    [SerializeField]
    private bool lockX = true;
    [SerializeField]
    private bool lockY = true;
    [SerializeField]
    private bool lockZ = false;

    void Start()
    {
        rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
    }

    private void Update()
    {
        this.transform.Rotate(new Vector3(
            rotationSpeed * Time.deltaTime * MathUtils.BoolToInt(!lockX), 
            rotationSpeed * Time.deltaTime * MathUtils.BoolToInt(!lockY), 
            rotationSpeed * Time.deltaTime * MathUtils.BoolToInt(!lockZ)));
    }

    // Setters and getters
    public bool GetLockX() { return lockX; }
    public bool GetLockY() { return lockY; }
    public bool GetLockZ() { return lockZ; }
    public void SetLockX(bool _value) { lockX = _value; }
    public void SetLockY(bool _value) { lockY = _value; }
    public void SetLockZ(bool _value) { lockZ = _value; }
}
