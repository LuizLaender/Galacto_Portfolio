using UnityEngine;
using System.Collections.Generic;

public class SpaceManager : MonoBehaviour
{
    public Transform player;
    public float sectorSize = 50f;
    public SolarSystemGenerator generator;
    private List<(Vector2 position, float radius)> generatedSystems = new();

    private HashSet<Vector2Int> generatedSectors = new();

    void Update()
    {
        Vector2Int playerSector = new Vector2Int(
            Mathf.FloorToInt(player.position.x / sectorSize),
            Mathf.FloorToInt(player.position.y / sectorSize)
        );

        for (int x = -1; x <= 1; x++)
        for (int y = -1; y <= 1; y++)
        {
            Vector2Int sector = playerSector + new Vector2Int(x, y);

            if (sector == Vector2Int.zero || generatedSectors.Contains(sector))
                continue;

            generatedSectors.Add(sector);

            System.Random rng = new System.Random(sector.GetHashCode());
            float offsetX = (float)rng.NextDouble() * sectorSize * 1f - sectorSize * 0.2f;
            float offsetY = (float)rng.NextDouble() * sectorSize * 1f - sectorSize * 0.2f;

            Vector2 worldPos = new Vector2(
                sector.x * sectorSize + offsetX,
                sector.y * sectorSize + offsetY
            );

            int maxPlanets = generator.maxPlanets;
            float orbitSpacing = generator._maxOrbitDistance;
            float largestOrbitRadius = 2f + (maxPlanets - 1) * orbitSpacing;

            bool overlaps = false;
            foreach (var (pos, radius) in generatedSystems)
            {
                float distance = Vector2.Distance(worldPos, pos);
                if (distance < radius + largestOrbitRadius)
                {
                    overlaps = true;
                    break;
                }
            }

            if (!overlaps)
            {
                generator.GenerateSolarSystem(worldPos);
                generatedSystems.Add((worldPos, largestOrbitRadius));
            }
        }
    }
}
