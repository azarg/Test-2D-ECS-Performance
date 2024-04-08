using UnityEngine;

public enum BehaviorTypes
{
    None,
    Empty,
    Complex,
}

public class GameObjectsSpawner : MonoBehaviour
{
    public static GameObjectsSpawner Instance;

    public Vector2 SpawnArea;
    public GameObject ItemPrefab;
    
    readonly int MaxItems = 50000;

    public GameObject[] items;

    private void Awake() {
        Instance = this;
    }

    void Start() {
        items = new GameObject[MaxItems];

        for(int i = 0; i < MaxItems; i++) {
            var instance = Instantiate(ItemPrefab);
            instance.transform.position = new Vector3(Random.Range(0, SpawnArea.x), Random.Range(0, SpawnArea.y), 0);
            instance.SetActive(false);
            items[i] = instance;
        }
    }

    public void RenderItems(int count) {
        for(int i = 0; i < items.Length; i++) {
            bool enable = i < count;
            items[i].SetActive(enable);
        }
    }

    public void SetBehaviorActive<T>(bool enable) where T : MonoBehaviour {
        for (int i = 0; i < items.Length; i++) {
            items[i].GetComponent<T>().enabled = enable;
        }
    }

    public void EnableMovement(bool enable) {
        for (int i = 0; i < items.Length; i++) {
            if(items[i].TryGetComponent<GameObjectComplexBehavior>(out var behavior)) {
                behavior.EnableMovement(enable);
            }
        }
    }
    public void EnableBoundsCheck(bool enable) {
        for (int i = 0; i < items.Length; i++) {
            if (items[i].TryGetComponent<GameObjectComplexBehavior>(out var behavior)) {
                behavior.EnableBoundsCheck(enable);
            }
        }
    }
}
