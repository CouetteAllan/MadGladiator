using UnityEngine;

public class EnemyBehavior : MonoBehaviour {
    [Header("Movement")]
    public Vector3 targetPosition = new Vector2(0f, 0f);
    [SerializeField] float speed = 0.1f;

    [Header("Animation")]
    [SerializeField] Animator animator;

    void Start() {
        InitAnim();
    }

    void Update() {
        this.transform.position = Vector2.Lerp(this.transform.position, targetPosition, speed * Time.deltaTime);
    }
    void InitAnim() {
        Vector3 direction = targetPosition - this.transform.position;
        animator.SetFloat("DirectionX", direction.x);
        animator.SetFloat("DirectionY", direction.y);
    }
}
