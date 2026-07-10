using UnityEngine;

public class Background : MonoBehaviour
{
    public Transform player;
    public float parallaxFactor = 0.1f;

    private Material mat;
    private Vector2 lastPlayerPos;
    private PlayerOrbit orbit;

    void Awake()
    {
        orbit = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerOrbit>();
        mat = GetComponent<SpriteRenderer>().material;
        lastPlayerPos = player.position;
    }

    void Update()
    {
        if (!orbit.IsPlayerOrbiting)
        {
            float speed = orbit.GetComponent<Rigidbody2D>().linearVelocity.magnitude;

            float t = Mathf.InverseLerp(5f, 25f, speed);
            parallaxFactor = Mathf.Lerp(0.005f, 0.01f, t);

            Vector2 delta = (Vector2)player.position - lastPlayerPos;
            mat.mainTextureOffset += delta * parallaxFactor;
            lastPlayerPos = player.position;
        }

    }
}