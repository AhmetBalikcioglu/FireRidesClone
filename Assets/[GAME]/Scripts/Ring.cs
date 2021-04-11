using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour, ICollectable
{
    public void Collect()
    {
        if (Vector3.Distance(CharacterManager.Instance.transform.position, transform.position) < .5f)
        {
            ScoreManager.Instance.score += 10f;
        }
        else
        {
            ScoreManager.Instance.score += 5f;
        }
        Destroy(transform.parent.gameObject);
    }
}
