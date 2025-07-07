using Unity.Entities;
using UnityEngine;

public class EnemySpawnerAuthoring : MonoBehaviour
{

    public GameObject EnemyPrefab;
    public float SpawnsPerSecond;
    public float ActivationDistance;

    class Baker : Baker<EnemySpawnerAuthoring>{
        public override void Bake(EnemySpawnerAuthoring authoring){
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new ProximitySpawner{
                EnemyPrefab = GetEntity(authoring.EnemyPrefab, TransformUsageFlags.Dynamic),
                SpawnRate = authoring.SpawnsPerSecond,
                SpawnTimer = 0f,
                ActivationDistance = authoring.ActivationDistance
            });
        }
    }
}

public struct SpawnTag : IComponentData { }
public struct ProximitySpawner : IComponentData
{
    public Entity EnemyPrefab;
    public float SpawnRate;
    public float SpawnTimer;
    public float ActivationDistance;
}