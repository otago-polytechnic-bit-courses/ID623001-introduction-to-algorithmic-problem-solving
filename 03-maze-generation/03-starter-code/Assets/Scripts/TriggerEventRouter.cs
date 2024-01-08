/*
 * written by Joseph Hocking 2017
 * released under MIT license
 * text of license https://opensource.org/licenses/MIT
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void TriggerEventHandler(GameObject trigger, GameObject other);

public class TriggerEventRouter : MonoBehaviour
{
    public TriggerEventHandler callback;

    void OnTriggerEnter(Collider other)
    {
        if (callback != null)
        {
            callback(this.gameObject, other.gameObject);
        }
    }
}
