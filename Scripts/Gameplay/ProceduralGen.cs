using UnityEngine;

public class SolarSystemGenerator : MonoBehaviour
{
    public GameObject starPrefab;
    public GameObject planetPrefab;

    [Header("Orbit Settings")]
    public GameObject orbitLinePrefab;
    public int orbitSegments = 100;

    public int minPlanets = 1;
    public int maxPlanets = 5;
    public float _minOrbitDistance;
    public float _maxOrbitDistance;
    private float randomOrbitDistance;

    void Start()
    {
        GenerateSolarSystem(Vector2.zero);
    }

    public void GenerateSolarSystem(Vector2 position)
    {
        randomOrbitDistance = Random.Range(_minOrbitDistance, _maxOrbitDistance);

        GameObject star = Instantiate(starPrefab, position, Quaternion.identity);
        Color[] starColors = new Color[]
        {
            new Color(0.9926069f, 1f, 0f),                  // yellow
            new Color(1f, 0.2682647f, 0f),                  // orange
            new Color(0.495283f, 0.8994794f, 1f),           // blue
            new Color(0.7533375f, 0.8975573f, 0.9339623f),  // white
        };
        Color randomColor = starColors[Random.Range(0, starColors.Length)];
        star.GetComponent<SpriteRenderer>().color = randomColor;

        int planetCount = Random.Range(minPlanets, maxPlanets + 1);

        for (int i = 0; i < planetCount; i++)
        {
            float orbitRadius = 2f + i * randomOrbitDistance; // distnce from star
            float angle = Random.Range(0f, 360f);

            Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector3.right;
            Vector2 planetPos = position + direction.normalized * orbitRadius;

            GameObject planet = Instantiate(planetPrefab, planetPos, Quaternion.identity);
            
            planet.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
            planet.transform.localScale *= Random.Range(0.5f, 1.5f);

            DrawOrbitCircle(position, orbitRadius, star.transform);

            var orbitScript = planet.GetComponent<PlanetOrbit>();
            orbitScript.center = star.transform;
            orbitScript.orbitRadius = orbitRadius;
            orbitScript.orbitSpeed = Random.Range(10f, 30f);
            orbitScript.initialAngle = angle;
        }

        float largestOrbitRadius = 2f + (planetCount - 1) * randomOrbitDistance;
        CircleCollider2D starCollider = star.GetComponent<CircleCollider2D>();
        starCollider.radius = largestOrbitRadius;
    }

    void DrawOrbitCircle(Vector2 center, float radius, Transform star)
    {
        GameObject orbitGO = Instantiate(orbitLinePrefab, center, Quaternion.identity, star);
        LineRenderer lr = orbitGO.GetComponent<LineRenderer>();
        if (lr == null)
        {
            Debug.LogWarning("missing linerenderer");
            return;
        }

        lr.positionCount = orbitSegments;
        Vector3[] points = new Vector3[orbitSegments];
        for (int i = 0; i < orbitSegments; i++)
        {
            float angle = (float)i / orbitSegments * Mathf.PI * 2;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            points[i] = new Vector3(x, y, 0);
        }
        lr.SetPositions(points);
    }
}
