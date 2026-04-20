using UnityEngine;

public class CutLineWall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boss"))
            return;

        Destroy(other.gameObject);
    }
}