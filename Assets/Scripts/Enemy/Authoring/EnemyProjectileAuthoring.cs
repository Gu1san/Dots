using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class ProjectileAuthoring : MonoBehaviour
{
    public float projectDamage;
    public float projectLifeTime;
    class Baker : Baker<ProjectileAuthoring>
    {
        public override void Bake(ProjectileAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<ProjectileTag>(entity);
            AddComponent(entity, new ProjectileMoveData(){
                damage = authoring.projectDamage
            });
            AddComponent(entity, new ProjectileLifetime(){
                timeRemaining = authoring.projectLifeTime
            });
        }
    }
}
public struct ProjectileTag : IComponentData { }
public struct ProjectileMoveData : IComponentData
{
    public float3 Direction;
    public float Speed;
    public float damage;
}

public struct ProjectileLifetime : IComponentData{
    public float timeRemaining;
}