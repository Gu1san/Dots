using UnityEngine;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Authoring;

public class EnemyAuthoring : MonoBehaviour
{
    [Header("General")]
    public float initialLife = 100f;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public float stopDistance = 5f;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public float bulletStrength = 10f;
    public float fireRate = 1f;
    public float bulletScale = 0.5f;

    class Baker : Baker<EnemyAuthoring>
    {
        public override void Bake(EnemyAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent<EnemyTag>(entity);

            AddComponent(entity, new EnemyLife { lifeValue = authoring.initialLife });

            AddComponent(entity, new MoveSpeedEnemy{
                value = authoring.moveSpeed,
                stopValue = authoring.stopDistance
            });

            AddComponent(entity, new EnemyGun{
                bulletPrefab = GetEntity(authoring.bulletPrefab, TransformUsageFlags.Dynamic),
                bulletScale = authoring.bulletScale,
                shotStrength = authoring.bulletStrength,
                fireRate = authoring.fireRate,
                shotCooldown = 0f
            });
        }
    }
}
public struct EnemyTag : IComponentData { }

public struct MoveSpeedEnemy : IComponentData
{
    public float value;
    public float stopValue;
}
public struct EnemyLife : IComponentData
{
    public float lifeValue;
}

public struct EnemyGun : IComponentData
{
    public Entity bulletPrefab;   
    public float bulletScale;      
    public float shotStrength;     
    public float fireRate;         
    public float shotCooldown;     
}