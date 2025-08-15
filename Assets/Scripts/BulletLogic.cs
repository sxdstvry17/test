using System.Collections;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    public float infectionRadius = 2f;
    public float destroyDelay = 1f;
    public Color emissionColor = Color.yellow;
    private void Update()
    {
        infectionRadius = gameObject.GetComponent<Renderer>().bounds.extents.magnitude * 1.5f;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            StartCoroutine(TriggerExplosion());
        }

        if (collision.gameObject.CompareTag("Goal"))
        {
            StartCoroutine(PathToTheGoal(collision.gameObject));
        }
    }
    IEnumerator TriggerExplosion()
    {
        Collider[] affectedObjects = Physics.OverlapSphere(transform.position, infectionRadius);

        foreach (Collider col in affectedObjects)
        {
            if (col.CompareTag("Obstacle"))
            {
                StartCoroutine(Infect(col.gameObject));
            }
        }
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
    IEnumerator Infect(GameObject obstacle)
    {
        Renderer renderer = obstacle.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.SetColor("_EmissionColor", emissionColor);
            yield return new WaitForSeconds(destroyDelay);
            Destroy(obstacle);
        }
    }
    IEnumerator PathToTheGoal(GameObject goal)
    {
        gameObject.GetComponent<Renderer>().enabled = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Animator>().SetBool("Bounce", true);
        GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerBall>().StopShooting();
        GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<Animator>().SetBool("Camera", true);
        yield return new WaitForSeconds(0.7f);
        goal.transform.parent.GetComponent<Animator>().SetBool("Open", true);
        yield return new WaitForSeconds(3f);
        GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerBall>().Victory();
    }
        void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, infectionRadius);
    }
}
