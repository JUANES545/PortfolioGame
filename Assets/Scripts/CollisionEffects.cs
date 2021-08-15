using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEffects : MonoBehaviour
{
    public GameObject hitEffectPrefab;

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Doggy");
        Instantiate(hitEffectPrefab, other.GetContact(0).point, Quaternion.identity);
    }
}
