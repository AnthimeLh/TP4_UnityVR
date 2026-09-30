using System.Collections.Generic;
using UnityEngine;

// =====================================================================
//  EXERCICE 6 - Apparition aléatoire des cibles
//  À placer sur un objet vide "SpawnerCibles".
//  La zone d'apparition est dessinée en vert dans la vue Scene.
// =====================================================================
public class SpawnerCibles : MonoBehaviour
{
    [Header("Cibles")]
    [Tooltip("Les prefabs de cibles (cube, cône...). Une est choisie au hasard à chaque fois.")]
    public GameObject[] prefabsCibles;

    [Tooltip("Nombre maximum de cibles présentes en même temps.")]
    public int nombreMaxCibles = 8;

    [Header("Temps entre deux apparitions (secondes)")]
    public float delaiMin = 1f;
    public float delaiMax = 3f;

    [Header("Zone d'apparition (centrée sur cet objet)")]
    public Vector3 tailleZone = new Vector3(6f, 2f, 2f);

    private readonly List<GameObject> ciblesPresentes = new List<GameObject>();
    private float chrono;

    void Start()
    {
        chrono = Random.Range(delaiMin, delaiMax);
    }

    void Update()
    {
        // On décompte le temps
        chrono -= Time.deltaTime;
        if (chrono > 0f) return;

        // Temps écoulé : on tire au hasard le prochain délai...
        chrono = Random.Range(delaiMin, delaiMax);

        // ...on oublie les cibles déjà détruites...
        ciblesPresentes.RemoveAll(c => c == null);

        // ...et on crée une cible s'il reste de la place
        if (ciblesPresentes.Count < nombreMaxCibles)
            FaireApparaitreCible();
    }

    void FaireApparaitreCible()
    {
        if (prefabsCibles == null || prefabsCibles.Length == 0)
        {
            Debug.LogError("SpawnerCibles : ajoutez au moins un prefab dans 'Prefabs Cibles' !");
            enabled = false;
            return;
        }

        // Un prefab au hasard
        GameObject prefab = prefabsCibles[Random.Range(0, prefabsCibles.Length)];
        if (prefab == null) return; // case de la liste laissée vide

        // Une position au hasard dans la zone
        Vector3 position = transform.position + new Vector3(
            Random.Range(-tailleZone.x / 2f, tailleZone.x / 2f),
            Random.Range(-tailleZone.y / 2f, tailleZone.y / 2f),
            Random.Range(-tailleZone.z / 2f, tailleZone.z / 2f));

        // On garde exactement l'orientation du prefab
        // (cibles plates : toujours tournées vers le joueur)
        Quaternion rotation = prefab.transform.rotation;

        ciblesPresentes.Add(Instantiate(prefab, position, rotation));
    }

    // Dessine la zone d'apparition dans la vue Scene (invisible en jeu)
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, tailleZone);
    }
}