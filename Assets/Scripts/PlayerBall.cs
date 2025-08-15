using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBall : MonoBehaviour
{
    public float startPlayerBallSize = 3.5f;
    public float minPlayerBallSize = 0.5f;
    public GameObject shotPrefab;
    public float shotMaxSize = 2f;
    public float shotChargeTime = 3.5f;
    public float shotSpeed = 10f;
    public GameObject shootingPoint;
    public GameObject target;
    GameObject shot;
    private float playerBallSize;
    private float shotSize = 0f;
    private float chargeTime = 0f;
    [HideInInspector]
    public bool canShoot = true;

    public GameObject goalTrack;
    public GameObject UIPanel;
    public GameObject victoryPanel;
    void Start()
    {
        playerBallSize = startPlayerBallSize;
    }

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0) && canShoot == true)
        {
            shot = Instantiate(shotPrefab, shootingPoint.transform.position, Quaternion.identity);
            shot.GetComponent<Rigidbody>().isKinematic = true;
        }
        if (Input.GetMouseButton(0) && canShoot == true)
        {
            float shrinkSpeed = 0.6f;
            playerBallSize = Mathf.Max(minPlayerBallSize, playerBallSize - shrinkSpeed * Time.deltaTime);
            goalTrack.transform.localScale = new Vector3(transform.localScale.x, goalTrack.transform.localScale.y, goalTrack.transform.localScale.z);
            chargeTime += Time.deltaTime;
            shotSize = Mathf.Lerp(0f, shotMaxSize, chargeTime / shotChargeTime);
            shot.transform.localScale = Vector3.one * shotSize;
        }

        if (Input.GetMouseButtonUp(0) && canShoot == true)
        {
            Shoot();
            chargeTime = 0f;
            shotSize = 0f;
        }

        transform.localScale = Vector3.one * playerBallSize;

        if(canShoot == false && playerBallSize == minPlayerBallSize)
        {
            UIPanel.SetActive(true);
        }
    }
    public void StopShooting()
    {
        canShoot = false;
    }
    public void Victory()
    {
        victoryPanel.SetActive(true);
    }
    void Shoot()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        shot.GetComponent<Rigidbody>().isKinematic = false;
        shot.GetComponent<Rigidbody>().linearVelocity = direction * shotSpeed;
        if (playerBallSize != minPlayerBallSize)
        {
            canShoot = true;
        }
        else
        {
            canShoot = false;
        }
    }
}
