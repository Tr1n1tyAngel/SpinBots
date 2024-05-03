using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public GameManager gameManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "P1SpinBot")
        {
            StartCoroutine(P1ApplyShield());
        }
        if (other.tag == "P2SpinBot")
        {
            StartCoroutine(P2ApplyShield());
        }
    }
    IEnumerator P1ApplyShield()
    {
        this.gameObject.transform.position = new Vector3(99999, 99999, 99999);
        gameManager.p1Shield=true;

        yield return new WaitForSeconds(10);


        gameManager.p1Shield = false;

        this.gameObject.SetActive(false);
    }
    IEnumerator P2ApplyShield()
    {
        this.gameObject.transform.position = new Vector3 (99999,99999,99999);
        gameManager.p2Shield = true;

        yield return new WaitForSeconds(10);


        gameManager.p2Shield = false;

        this.gameObject.SetActive(false);
    }
}
