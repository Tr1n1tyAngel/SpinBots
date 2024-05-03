using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    public GameManager gameManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "P1SpinBot")
        {
            StartCoroutine(P1ApplySpeedBoost());
        }
        if (other.tag == "P2SpinBot")
        {
            StartCoroutine(P2ApplySpeedBoost());
        }
    }
    IEnumerator P1ApplySpeedBoost()
    {
        this.gameObject.transform.position = new Vector3(99999, 99999, 99999);
        gameManager.p1CurrentMovementSpeed += 2f;

        yield return new WaitForSeconds(5);


        gameManager.p1CurrentMovementSpeed -= 2f;

        this.gameObject.SetActive(false);
    }
    IEnumerator P2ApplySpeedBoost()
    {
        this.gameObject.transform.position = new Vector3(99999, 99999, 99999);
        gameManager.p2CurrentMovementSpeed += 2f;

        yield return new WaitForSeconds(5);


        gameManager.p2CurrentMovementSpeed -= 2f;

        this.gameObject.SetActive(false);
    }
}
