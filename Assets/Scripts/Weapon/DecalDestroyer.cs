using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalDestroyer : MonoBehaviour
{
    [SerializeField]
    private float _time;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(_time);
        Destroy(gameObject);
    }
}
