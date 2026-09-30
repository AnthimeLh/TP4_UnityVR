using UnityEngine;

// =====================================================================
//  EXERCICE 5 - Pistolet VR
//  À placer sur l'objet "Pistolet" (celui qui a le XR Grab Interactable).
//
//  La méthode Tirer() est appelée par l'événement "Activated" du
//  XR Grab Interactable, c'est-à-dire quand on appuie sur la gâchette
//  en tenant le pistolet.
// =====================================================================
public class Pistolet : MonoBehaviour
{
    [Header("Références")]
    [Tooltip("Le prefab de la balle (petite sphère avec Rigidbody + script Balle).")]
    public GameObject prefabBalle;

    [Tooltip("Objet vide placé au bout du canon. Sa flèche bleue (Z) pointe vers l'avant.")]
    public Transform pointDeTir;

    [Header("Réglages du tir")]
    [Tooltip("Force de propulsion. Avec une balle de masse 1 : force 20 = 20 mètres/seconde.")]
    public float forceDeTir = 20f;

    [Tooltip("Temps minimum entre deux tirs, en secondes.")]
    public float delaiEntreTirs = 0.15f;

    [Tooltip("Optionnel : l'Audio Source qui joue le bruit du tir.")]
    public AudioSource sonDeTir;

    private float prochainTirPossible = 0f;

    public void Tirer()
    {
        // Cadence de tir : on ignore les appuis trop rapprochés
        if (Time.time < prochainTirPossible) return;
        prochainTirPossible = Time.time + delaiEntreTirs;

        if (prefabBalle == null || pointDeTir == null)
        {
            Debug.LogError("Pistolet : remplissez 'Prefab Balle' et 'Point De Tir' dans l'Inspector !");
            return;
        }

        // 1) Créer une balle au bout du canon, orientée comme le canon
        GameObject balle = Instantiate(prefabBalle, pointDeTir.position, pointDeTir.rotation);

        // 2) Empêcher la balle de percuter le pistolet lui-même
        Collider colliderBalle = balle.GetComponent<Collider>();
        if (colliderBalle != null)
        {
            foreach (Collider c in GetComponentsInChildren<Collider>())
                Physics.IgnoreCollision(colliderBalle, c);
        }

        // 3) Propulser la balle vers l'avant du canon (une seule poussée : Impulse)
        Rigidbody rb = balle.GetComponent<Rigidbody>();
        if (rb != null)
            rb.AddForce(pointDeTir.forward * forceDeTir, ForceMode.Impulse);

        // 4) Bruit du tir (optionnel)
        if (sonDeTir != null)
            sonDeTir.Play();
    }
}
