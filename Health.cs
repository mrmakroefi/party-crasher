using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

    public int maxHitPoint = 30;

    private int hitPoint;

    void Start() {
        Reset();
    }

    public void Damage(int damage) {
        hitPoint -= damage;
        if ( hitPoint <= 0 ) {
            // dead
            gameObject.SetActive(false);
        }
    }

    void Reset() {
        hitPoint = maxHitPoint;
    }
}
