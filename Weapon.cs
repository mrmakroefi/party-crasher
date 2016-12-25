using UnityEngine;
using System.Collections;
using InControl;

public class Weapon : MonoBehaviour {

    private new Transform transform;

    public bool debugRay = false;

    public int deviceIndex = 0;

    public int damagePerShot = 4;
    public float fireRate = 9;
    public float maxDistance = 12f;
    public float inaccurationInDegree = 0.02;
    public LayerMask layerMask;
    public LineRenderer traceEffect;

    public Transform  bulletSpawnPos;
     
    [HideInInspector]
    public bool fire = false;

    private float nextFire = 0;

    InputDevice device;

    void Awake() {
        transform = GetComponent<Transform>();

        device = InputManager.Devices[deviceIndex];
        traceEffectDisable();
    }

    void Update() {

        if ( debugRay ) {
            Vector3 direction = bulletSpawnPos.TransformDirection(Vector3.forward);
            Debug.DrawRay(bulletSpawnPos.position, direction * maxDistance, Color.red);
        }

        fire = device.RightTrigger;

        if (fire && Time.time >= nextFire ) {
            Fire();

            nextFire = Time.time + (1/fireRate);
        }
    }

    void Fire() {
        Vector3 direction = bulletSpawnPos.TransformDirection(Vector3.forward + (Vector3.right * Random.Range(-inaccurationInDegree, inaccurationInDegree)));

        RaycastHit hit;

        traceEffect.SetPosition(0, bulletSpawnPos.position);
        if ( Physics.Raycast(bulletSpawnPos.position, direction, out hit, maxDistance, layerMask) )  {
            print("we hit " + hit.collider.name);

            if ( hit.collider.CompareTag("Player") ) {
                hit.collider.GetComponent<Health>().Damage(damagePerShot);
            }

            traceEffect.SetPosition(1, hit.point);
        } else {
            traceEffect.SetPosition(1, bulletSpawnPos.position + direction * maxDistance);
        }

        traceEffect.enabled = true;
        Invoke("traceEffectDisable", 0.02f);
    }

    void traceEffectDisable() {
        if (traceEffect)
            traceEffect.enabled = false;
    }

}
