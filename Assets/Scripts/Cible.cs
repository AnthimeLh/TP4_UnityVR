using UnityEngine;

// =====================================================================
//  EXERCICE 6 - Cible
//  À placer sur chaque prefab de cible (qui doit avoir un Collider).
//  Quand une balle la touche : effet de particules, son, et la cible
//  disparaît.
// =====================================================================
public class Cible : MonoBehaviour
{
    [Tooltip("Le prefab de l'effet (Particle System) : explosion, fumée...")]
    public GameObject effetImpact;

    [Tooltip("Optionnel : un son joué à l'impact.")]
    public AudioClip sonImpact;

    // Appelée automatiquement par Unity quand un objet physique nous percute
    void OnCollisionEnter(Collision collision)
    {
        // Est-ce une balle ? (on reconnaît une balle à son script "Balle")
        if (collision.gameObject.GetComponent<Balle>() == null) return;

        Debug.Log("Cible touchée : " + name);

        // 1) L'effet de particules apparaît à la place de la cible
        if (effetImpact != null)
        {
            GameObject effet = Instantiate(effetImpact, transform.position, Quaternion.identity);
            Destroy(effet, 3f); // on nettoie l'effet après 3 secondes
        }

        // 2) Le son (optionnel)
        if (sonImpact != null)
            AudioSource.PlayClipAtPoint(sonImpact, transform.position);

        // 3) La cible disparaît
        Destroy(gameObject);
    }
}
