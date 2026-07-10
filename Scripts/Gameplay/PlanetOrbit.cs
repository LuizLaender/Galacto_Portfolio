using UnityEngine;

public class PlanetOrbit : MonoBehaviour
{
    public Transform center;
    public float orbitRadius = 3f;
    public float orbitSpeed = 20f;
    public float initialAngle;

    float currentAngle;

    void Start()
    {
        currentAngle = initialAngle;
    }

    void Update()
    {
        if (center == null) return;

        currentAngle += orbitSpeed * Time.deltaTime;
        float rad = currentAngle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * orbitRadius;
        transform.position = center.position + offset;
    }
}