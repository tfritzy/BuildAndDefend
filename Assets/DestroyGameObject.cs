using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGameObject : MonoBehaviour
{
    private void DestroyThis()
    {
        Destroy(this.gameObject);
    }
}
