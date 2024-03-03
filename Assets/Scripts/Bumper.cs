using UnityEngine;

public class Bumper : MonoBehaviour
{
    [SerializeField] string playerTag;
    [SerializeField] float bounceForce;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == playerTag)
        {
            print("test");
            Rigidbody otherRB = collision.rigidbody;
            //otherRB.AddForce(collision.contacts[0].normal * bounceForce);
            //Vector3 dir = collision.transform.position - transform.position;
            //dir.y = 0;
            //dir = dir.normalized;
            //otherRB.AddForce(dir * bounceForce);
            //Debug.LogError("Here");
            otherRB.AddExplosionForce(bounceForce, collision.contacts[0].point, 5);
        }
    }
}
