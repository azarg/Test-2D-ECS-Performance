using UnityEngine;

public class GameObjectComplexBehavior : MonoBehaviour
{
    bool doMove = false;
    bool doBoundsCheck = false;

    Vector2 direction;
    float speed = 10f;

    public void EnableMovement(bool enable) {
        this.doMove = enable;
    }
    public void EnableBoundsCheck(bool enable) {
        this.doBoundsCheck = enable;
    }

    private void Awake() {
        direction = Random.insideUnitCircle.normalized;
    }

    void Update() {
        if (doMove) transform.Translate(speed * Time.deltaTime * direction);
        if (doBoundsCheck) {
            var pos = transform.position;
            if (pos.x < 0) direction.x = Mathf.Abs(direction.x);
            if (pos.x > GameObjectsSpawner.Instance.SpawnArea.x) direction.x = -Mathf.Abs(direction.x);
            if (pos.y < 0) direction.y = Mathf.Abs(direction.y);
            if (pos.y > GameObjectsSpawner.Instance.SpawnArea.y) direction.y = -Mathf.Abs(direction.y);
        }
    }
}
