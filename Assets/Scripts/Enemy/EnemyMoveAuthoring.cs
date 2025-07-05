using UnityEngine;
using Unity.Entities;

public class EnemyMoveAuthoring : MonoBehaviour
{
    public float moveSpeed;
    class Baker : Baker<EnemyMoveAuthoring>{
        public override void Bake(EnemyMoveAuthoring authoring){
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<EnemyTag>(entity);
            AddComponent(entity, new MoveSpeedEnemy(){
                value = authoring.moveSpeed

            });
        }
    }
}

public struct EnemyTag : IComponentData { }
public struct MoveSpeedEnemy : IComponentData{
    public float value;
}

