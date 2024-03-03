using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class GunShoot : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    private float bulletSpeed;

    [SerializeField] private ParticleSystem particles;

    Vector3 initialPosition;
    float shakeAmount;
    float recoilAmount;
    float fovAmount;
    bool returnToFOV = false;

    float timeElapsed;
    float lerpDuration = 0.1f;

    float endValue = 60;
    float valueToLerp;

    float particlesCount;
    private bool canShoot = true;

    [SerializeField] private AudioSource holdSound;

    private void Start()
    {
        initialPosition = transform.localPosition;

        gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color = new Color(0.6f, 0.6f, 0.6f);
        gameObject.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().material.color = new Color(0.6f, 0.6f, 0.6f);
        gameObject.transform.GetChild(2).gameObject.GetComponent<MeshRenderer>().material.color = new Color(0.6f, 0.6f, 0.6f);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && canShoot)
        {
            holdSound.pitch = (Random.Range(0.95f, 1.05f));
            holdSound.Play();
        }

        if (Input.GetKey(KeyCode.Mouse0) && !Input.GetKeyUp(KeyCode.Mouse0) && canShoot)
        {
            if(shakeAmount < 0.02f)
            {
                shakeAmount += 0.00002f;
            }
            if (recoilAmount < 0.5f)
            {
                recoilAmount += 0.0005f;
            }
            if (fovAmount < 20)
            {
                fovAmount += 0.02f;
            }
            if (particlesCount < 3000)
            {
                particlesCount += 0.1f;
            }

            if(fovAmount < 7)
            {
                gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color = Color.green * 0.6f;
                gameObject.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().material.color = Color.green * 0.6f;
                gameObject.transform.GetChild(2).gameObject.GetComponent<MeshRenderer>().material.color = Color.green * 0.6f;
            }
            else if (fovAmount < 16 && fovAmount > 7)
            {
                gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow * 0.9f;
                gameObject.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow * 0.9f;
                gameObject.transform.GetChild(2).gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow * 0.9f;
            }
            else if (fovAmount < 20 && fovAmount > 16)
            {
                gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color = Color.red * 0.8f;
                gameObject.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().material.color = Color.red * 0.8f;
                gameObject.transform.GetChild(2).gameObject.GetComponent<MeshRenderer>().material.color = Color.red * 0.8f;
            }

            Camera.main.fieldOfView = 60 + fovAmount;
            transform.parent.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -recoilAmount);
            transform.localPosition = new Vector3(Random.Range(-shakeAmount, shakeAmount), Random.Range(-shakeAmount, shakeAmount), Random.Range(-shakeAmount, shakeAmount));

            if (bulletSpeed < 30)
            {
                bulletSpeed += 0.03f;
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) && canShoot)
        {
            holdSound.Stop();

            canShoot = false;
            particles.emission.SetBurst(0, new ParticleSystem.Burst(0.0f, particlesCount));
            particles.Play();

            var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            bullet.GetComponent<CreatePickleTree>().setTreeSizeMultiplier((fovAmount/20)+1);
            bullet.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * bulletSpeed;

            bulletSpeed = 0;
            transform.localPosition = initialPosition;
            shakeAmount = 0;
            recoilAmount = 0;
            transform.parent.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
            fovAmount = 0;
            returnToFOV = true;
            particlesCount = 0;
            gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color = new Color(0.6f, 0.6f, 0.6f);
            gameObject.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().material.color = new Color(0.6f, 0.6f, 0.6f);
            gameObject.transform.GetChild(2).gameObject.GetComponent<MeshRenderer>().material.color = new Color(0.6f, 0.6f, 0.6f);

            Invoke("CanShootAgain", 1);
        }

        if (timeElapsed < lerpDuration && returnToFOV)
        {
            valueToLerp = Mathf.Lerp(Camera.main.fieldOfView, endValue, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            Camera.main.fieldOfView = valueToLerp;

            if (Camera.main.fieldOfView == 60)
            {
                returnToFOV = false;
                timeElapsed = 0;
                valueToLerp = 60;
            }
        }
    }

    private void CanShootAgain()
    {
        canShoot = true;
    }
}
