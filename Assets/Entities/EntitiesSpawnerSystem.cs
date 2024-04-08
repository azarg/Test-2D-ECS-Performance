using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

/// <summary>
/// Single run system that spawns enemies when the app starts
/// Instantiates max number of items. The number is const. <see cref="EntitiesSpawnerAuthoring"/>
/// </summary>
public partial struct EntitiesSpawnerSystem : ISystem
{
    public void OnCreate(ref SystemState systemState) {
        // Wait for scene to load
        // This is achieved by waiting for at least one entity to load
        systemState.RequireForUpdate<EntitiesSpawner>();
    }

    public void OnUpdate(ref SystemState systemState) {
        // Disable the system on first use. Effectively making a single use system
        systemState.Enabled = false;

        var spawner = SystemAPI.GetSingleton<EntitiesSpawner>();
        var prefab = spawner.ItemPrefabEntity;
        var numItems = spawner.MaxNumberOfItems;
        var spawnArea = spawner.SpawnArea;

        var instances = systemState.EntityManager.Instantiate(prefab, numItems, Allocator.Temp);

        // Instantiate entities and add the built in Disabled component
        // Disabled component is similar to GameObject.SetActive(false)
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var random = new Random(2); 
        foreach(var entity in instances) {
            var transform = SystemAPI.GetComponentRW<LocalTransform>(entity);
            transform.ValueRW.Position = random.NextFloat3(new float3(spawnArea.x, spawnArea.y, 0));
            var entityData = SystemAPI.GetComponentRW<EntityData>(entity);
            entityData.ValueRW.direction = math.normalize(random.NextFloat2Direction());
            entityManager.AddComponent<Disabled>(entity);
        }
    }
}

