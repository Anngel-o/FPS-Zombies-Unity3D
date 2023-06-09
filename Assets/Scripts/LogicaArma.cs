using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModoDeDisparo {
    SemiAuto,
    FullAuto
}

public class LogicaArma : MonoBehaviour
{
    protected Animator animator;
    protected AudioSource audioSource;
    public bool tiempoNoDisparo = false;
    public bool puedeDisparar = false;
    public bool recargando = false;

    [Header("Referencia de Objetos")]
    public ParticleSystem fuegoDeArma;
    public Camera camaraPrincipal;
    public Transform puntoDeDisparo;

    [Header("Referencia de Sonidos")]
    public AudioClip SonDisparo;
    public AudioClip SonSinBalas;
    public AudioClip SonCartuchoEntra;
    public AudioClip SonCartuchoSale;
    public AudioClip SonVacio;
    public AudioClip SonDesenfundar;

    [Header("Atributos de Arma")]
    public ModoDeDisparo modoDeDisparo = ModoDeDisparo.FullAuto;
    public float danio = 20f;
    public float ritmoDeDisparo = 0.3f;
    public int balasRestantes;
    public int balasEnCartucho;
    public int tamanioDeCartucho = 12;
    public int maximoDeBalas = 100;
    public bool estaADS = false;
    public Vector3 disCadera;
    public Vector3 ADS;//bajar la mira
    public float tiempoApuntar;
    public float zoom;
    public float normal;//modo normal no apuntando
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        balasEnCartucho = tamanioDeCartucho;
        balasRestantes = maximoDeBalas;

        Invoke("HabilitarArma", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (modoDeDisparo == ModoDeDisparo.FullAuto && Input.GetButton("Fire1")) {
            RevisarDisparo();
        }
        else if (modoDeDisparo == ModoDeDisparo.SemiAuto && Input.GetButton("Fire1")) {
            RevisarDisparo();
        }

        if (Input.GetButtonDown("Reload")) {
            RevisarRecarga();
        }

        if(Input.GetMouseButton(1)) {//0 clic izquiero 1 clic derecho
            transform.localPosition = Vector3.Slerp(transform.localPosition, ADS, tiempoApuntar * Time.deltaTime);
            //se cambia la posición del vector para hacer el efecto de mira
            estaADS = true;
            camaraPrincipal.fieldOfView = Mathf.Lerp(camaraPrincipal.fieldOfView, normal, tiempoApuntar * Time.deltaTime);
            //después regresa al estado de animación normal
        }
        if(Input.GetMouseButton(1)) {
            estaADS = false;
        }
        if(estaADS == false) {
            transform.localPosition = Vector3.Slerp(transform.localPosition, disCadera, tiempoApuntar * Time.deltaTime);
            //si se hace clic derecho y se suelta regresa a la posición que estaba antes
            camaraPrincipal.fieldOfView = Mathf.Lerp(camaraPrincipal.fieldOfView, normal, tiempoApuntar * Time.deltaTime);
        }
    }


    void HabilitarArma() {
        puedeDisparar = true;
    }

    void RevisarDisparo() {
        if (!puedeDisparar) return;
        if (tiempoNoDisparo) return;
        if (recargando) return;
        if (balasEnCartucho > 0) {Disparar();}
        else {SinBalas();}
    }

    void Disparar () {
        audioSource.PlayOneShot(SonDisparo);
        tiempoNoDisparo = true;
        fuegoDeArma.Stop();
        fuegoDeArma.Play();
        ReproducirAnimacionDisparo();
        balasEnCartucho--;
        StartCoroutine(ReiniciarTiempoNoDisparo());
        DisparoDirecto();
    }

    void DisparoDirecto()
    {
        RaycastHit hit;
        if (Physics.Raycast(puntoDeDisparo.position, puntoDeDisparo.forward, out hit))
        {
            if (hit.transform.CompareTag("Enemigo"))
            {
                Vida vida = hit.transform.GetComponent<Vida>();
                if (vida == null)
                {
                    throw new System.Exception("No se encontro el componente Vida del Enemigo");
                }
                else
                {
                    vida.RecibirDanio(danio);
                }
            }
        }
    }

    public virtual void ReproducirAnimacionDisparo() {
        if (gameObject.name == "Police9mm") {
            if (balasEnCartucho > 1) {animator.CrossFadeInFixedTime("Fire", 0.1f);}
            else { animator.CrossFadeInFixedTime("FireLast", 0.1f); }
        }
        else {animator.CrossFadeInFixedTime("FireLast", 0.1f); }
        
    }

    void SinBalas() {
        audioSource.PlayOneShot(SonSinBalas);
        tiempoNoDisparo = true;
        StartCoroutine(ReiniciarTiempoNoDisparo());
    }
    IEnumerator ReiniciarTiempoNoDisparo() {
        yield return new WaitForSeconds(ritmoDeDisparo);
        tiempoNoDisparo = false;
    }

    void RevisarRecarga() {
        if (balasRestantes > 0 && balasEnCartucho < tamanioDeCartucho) {
            Recargar();
        }
    }

    void Recargar() {
        if (recargando) return;
        recargando = false;
        animator.CrossFadeInFixedTime("Reload", 0.1f);
    }

    void RecargarMuniciones() {
        int balasParaRecargar = tamanioDeCartucho - balasEnCartucho;
        int restarBalas = (balasRestantes >= balasParaRecargar) ? balasParaRecargar : balasRestantes;

        balasRestantes -= restarBalas;
        balasEnCartucho += balasParaRecargar;
    }

    public void DesenfundarOn() {
        audioSource.PlayOneShot(SonDesenfundar);
    }
    public void CartuchoEntraOn() {
        audioSource.PlayOneShot(SonCartuchoEntra);
        RecargarMuniciones();
    }
    public void CartuchoSaleOn() {
        audioSource.PlayOneShot(SonCartuchoSale);
    }
    public void VacioOn() {
        audioSource.PlayOneShot(SonVacio);
        Invoke("ReiniciarRecarga", 0.1f);
    }
    public void ReinciarRecarga() {
        recargando = true ;
    }
}
