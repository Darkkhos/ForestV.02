using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float timeToReset = 0.1f;
    public Vector3 dir;

    public string compareTag = "Enemy";
    public Action onHitTarget;

    private void Update()
    {
        transform.Translate(dir * Time.deltaTime);
    }

    public void StartProjectile()
    {
        Invoke(nameof(FinishUsage), timeToReset);
    }

    private void FinishUsage()
    {
        gameObject.SetActive(false);
        onHitTarget = null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag(compareTag))
        {
            Destroy(collision.gameObject);
            onHitTarget?.Invoke();
            FinishUsage();
        }
    }
}
