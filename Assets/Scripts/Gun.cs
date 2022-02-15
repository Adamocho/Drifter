using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 15f;
    public float fireRate = 25f;
    public float impactForce = 30f;
    public float maxDeviation = 5f;

    public GameObject left, right, leftTarget, rightTarget;
    public GameObject spawn;
    public ParticleSystem muzzleFlash_left, muzzleFlash_right;

    private float nextTimeToFire = 0f;
    private int alternate = 0;


    void Start() {
        leftTarget.transform.position += Vector3.forward * range;
        rightTarget.transform.position += Vector3.forward * range;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire) {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }


    void Shoot() {
        // Alternate between left and right riffle
        alternate++;
        GameObject origin = alternate % 2 == 0 ? right : left;
        ParticleSystem muzzleFlash = alternate % 2 == 0 ? muzzleFlash_right : muzzleFlash_left; 

        // apply random rotation to fired bullets
        Vector3 forwardVector = Vector3.forward;
        float deviation = Random.Range(0f, maxDeviation);
        float angle = Random.Range(0f, 360f);
        forwardVector = Quaternion.AngleAxis(deviation, Vector3.up) * forwardVector;
        forwardVector = Quaternion.AngleAxis(angle, Vector3.forward) * forwardVector;
        forwardVector = origin.transform.rotation * forwardVector;

        // here play particle effect
        muzzleFlash.Play();

        RaycastHit hit;
        if (Physics.Raycast(origin.transform.position, forwardVector, out hit, range)) {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
                target.TakeDamage(damage);

            if (hit.rigidbody != null)
                hit.rigidbody.AddForce(-hit.normal * impactForce);

            GameObject hitObj = Instantiate(spawn, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(hitObj, 1f);
        }
    }
}

// make line renderer
// health
// effect of death
// enemies
// different weapons
// stage
// ui