using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BalanceDeArma : MonoBehaviour
{
    public float cantidad;
    public float cantidadMax;
    public float tiempo;
    private Vector3 initPos;
    public bool seBalancea;
    // Start is called before the first frame update
    void Start()
    {
        initPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        seBalancea = true;
        float movX = Input.GetAxis("Mouse X") * cantidad;
        float movY = Input.GetAxis("Mouse Y") * cantidad;
        movX = Mathf.Clamp(movX, -cantidadMax, cantidadMax);
        movX = Mathf.Clamp(movY, -cantidadMax, cantidadMax);

        Vector3 PosFinalMov = new Vector3(movX, movY, 0);
        

        if (Input.GetMouseButton(1))
        {
            seBalancea = false;
        }

        if (seBalancea == true)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, PosFinalMov + initPos, tiempo * Time.deltaTime);
        }
    }
}
