using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorArmas : MonoBehaviour
{
    public LogicaArma[] armas;
    private int indiceArmaActual = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RevisarCambioDeArma();
    }

    void CambiarArmaActual() {
        
        for (int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        armas[indiceArmaActual].gameObject.SetActive(true);
    }

    void RevisarCambioDeArma() {
        float ruedaMouse = Input.GetAxis("Mouse ScrollWheel");
        if (ruedaMouse > 0f) {
            SeleccionarArmaAnterior();
            armas[indiceArmaActual].recargando = false;
            armas[indiceArmaActual].tiempoNoDisparo = false;
            armas[indiceArmaActual].estaADS = false;
        }
        else if (ruedaMouse < 0f)
        {
            SeleccionarArmaSiguiente();
            armas[indiceArmaActual].recargando = false;
            armas[indiceArmaActual].tiempoNoDisparo = false;
            armas[indiceArmaActual].estaADS = false;
        }
    }

    void SeleccionarArmaAnterior() {
        if(indiceArmaActual == 0) {
            indiceArmaActual = armas.Length - 1;
        }
        else {
            indiceArmaActual--;
        }
        CambiarArmaActual();
    }

    void SeleccionarArmaSiguiente()
    {
        if (indiceArmaActual >= (armas.Length - 1)) {
            indiceArmaActual = 0;
        }
        else {
            indiceArmaActual++;
        }
        CambiarArmaActual();
    }
}
