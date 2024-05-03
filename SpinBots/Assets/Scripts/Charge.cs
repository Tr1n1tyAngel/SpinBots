using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : MonoBehaviour
{
    public GameManager gameManager;
   
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag== "P1SpinBot")
        {
            
            Vector3 futurePosition = gameManager.p2Rigidbody.position + gameManager.p2Rigidbody.velocity * 0.25f;
            Vector3 direction = (futurePosition - gameManager.P1.transform.position).normalized;

            gameManager.p1Rigidbody.AddForce(direction * gameManager.chargeForce, ForceMode.Impulse);
            Debug.Log("Charge");
            gameManager.p1Charge = true;
            StartCoroutine(ChargeReset());
            


        }
        if(other.tag== "P2SpinBot")
        {
            Vector3 futurePosition = gameManager.p1Rigidbody.position + gameManager.p1Rigidbody.velocity * 0.25f;
            Vector3 direction = (futurePosition - gameManager.P2.transform.position).normalized;

            gameManager.p2Rigidbody.AddForce(direction * gameManager.chargeForce, ForceMode.Impulse);
            Debug.Log("Charge");
            gameManager.p2Charge = true;
            StartCoroutine(ChargeReset());
        }
    }
    private IEnumerator ChargeReset()
    {
        this.gameObject.transform.position = new Vector3(99999, 99999, 99999);
        yield return new WaitForSeconds(1f);
        gameManager.p1Charge = false;
        gameManager.p2Charge = false;
        this.gameObject.SetActive(false);
    }
}
