using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct EnemyGun : IComponentData
{
    public Entity bulletPrefab;   
    public float bulletScale;      
    public float shotStrength;     
    public float fireRate;         
    public float shotCooldown;     
}

public class EnemyShootAuthoring : MonoBehaviour{
    public GameObject BulletPrefab;
    public float Strength = 10f;
    public float Rate = 1f;
    public float Scale = 1f;
    class EnemyGunBaker : Baker<EnemyShootAuthoring>{
        public override void Bake(EnemyShootAuthoring authoring){
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EnemyGun(){
                bulletPrefab = GetEntity(authoring.BulletPrefab, TransformUsageFlags.Dynamic),
                bulletScale = authoring.Scale,
                shotStrength = authoring.Strength,
                fireRate = authoring.Rate,
                shotCooldown = 0f
            });
        }
    }
}