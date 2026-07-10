using UnityEngine;

public class BlackHoleSpin : MonoBehaviour
{
    public float spinSpeed = 90f;

    void Update()
    {
        transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);
    }
}
