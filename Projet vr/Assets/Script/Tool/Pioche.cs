using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pioche : MonoBehaviour
{
    [SerializeField] private int puissance;

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 pos = collision.contacts[0].point;
        Chunk c = collision.gameObject.GetComponent<Chunk>();
        int x = Mathf.FloorToInt(pos.x) - c.x * c.xSize;
        int y = Mathf.FloorToInt(pos.y);
        int z = Mathf.FloorToInt(pos.z) - c.z * c.zSize;

        c.data[x, y, z].terre = false;
        c.refresh();
    }
}
