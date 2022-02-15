using UnityEngine;

public class Target : MonoBehaviour {

    public float health = 50f;

    public void TakeDamage(float damage) {
        health -= damage;
        if (health < 1) { 
            Die();
        }
    }

    void Die() {
        Destroy(gameObject);
    }
}
