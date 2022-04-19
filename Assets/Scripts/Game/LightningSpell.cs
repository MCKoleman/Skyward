using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSpell : MonoBehaviour
{
    void OnParticleSystemStopped()
    {
        Destroy(this.gameObject);
    }
}
