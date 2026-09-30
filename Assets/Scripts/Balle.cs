using UnityEngine;

// =====================================================================
//  EXERCICE 5 - Balle
//  À placer sur le prefab de la balle (avec un Rigidbody et un Collider).
// =====================================================================
[RequireComponent(typeof(Rigidbody))]
public class Balle : MonoBehaviour
{
    [Tooltip("0 = pas de gravité (tir parfaitement droit), 1 = gravité réelle.")]
    [Range(0f, 2f)] public float gravite = 0.2f;

    [Tooltip("La balle est détruite après ce temps (en secondes) si elle n'a rien touché.")]
    public float dureeDeVie = 5f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // On coupe la gravité de Unity pour appliquer la nôtre, réglable
        rb.useGravity = false;
    }

    void Start()
    {
        Destroy(gameObject, dureeDeVie);
    }

    void FixedUpdate()
    {
        // Gravité "maison" : la gravité normale multipliée par notre réglage
        rb.AddForce(Physics.gravity * gravite, ForceMode.Acceleration);
    }

    void OnCollisionEnter(Collision collision)
    {
        // La balle disparaît dès qu'elle touche quelque chose
        Destroy(gameObject);
    }
}
