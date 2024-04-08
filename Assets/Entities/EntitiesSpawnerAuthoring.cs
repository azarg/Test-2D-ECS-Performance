using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class EntitiesSpawnerAuthoring : MonoBehaviour
{
    public Vector2 SpawnArea;
    public GameObject ItemPrefab;
    private int maxNumberOfItems = 50000;

    class Baker : Baker<EntitiesSpawnerAuthoring>
    {
        public override void Bake(EntitiesSpawnerAuthoring authoring) {
            var entity = GetEntity(authoring, TransformUsageFlags.None);
            AddComponent(entity, new EntitiesSpawner {
                SpawnArea = authoring.SpawnArea,
                ItemPrefabEntity = GetEntity(authoring.ItemPrefab, TransformUsageFlags.Dynamic),
                MaxNumberOfItems = authoring.maxNumberOfItems,
            });
        }
    }
}

public struct EntitiesSpawner : IComponentData
{
    public float2 SpawnArea;
    public Entity ItemPrefabEntity;
    public int MaxNumberOfItems;
}