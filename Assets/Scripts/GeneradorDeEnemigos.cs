using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorDeEnemigos : MonoBehaviour
{
    public GameObject zombiePrefab;
    public Transform[] puntosDeGeneracion;
    public float tiempoDeGeneracion = 5f;
    // Use this for initialization
    void Start()
    {
        puntosDeGeneracion = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            puntosDeGeneracion[i] = transform.GetChild(i);

        }

        StartCoroutine(AparecerEnemigo());
    }

    IEnumerator AparecerEnemigo()
    {
        while (true)
        {
            for (int i = 0; i < puntosDeGeneracion.Length; i++)
            {
                Transform puntoDeGeneracion = puntosDeGeneracion[i];
                Instantiate(zombiePrefab, puntoDeGeneracion.position, puntoDeGeneracion.rotation);
            }
            yield return new WaitForSeconds(tiempoDeGeneracion);
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
