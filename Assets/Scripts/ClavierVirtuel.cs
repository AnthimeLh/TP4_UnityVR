using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

// =====================================================================
//  EXERCICE 4 - Clavier virtuel
//  À placer sur la Canvas du menu (un objet qui reste toujours actif).
//
//  Le script crée TOUT SEUL les touches du clavier à partir d'un seul
//  bouton modèle (le prefab "Touche"), puis écrit dans le InputField
//  à chaque clic.
// =====================================================================
public class ClavierVirtuel : MonoBehaviour
{
    [Header("Références (à glisser dans l'Inspector)")]
    [Tooltip("Le InputField (TextMeshPro) dans lequel on écrit.")]
    public TMP_InputField champTexte;

    [Tooltip("L'objet 'Clavier' qui porte le Grid Layout Group.")]
    public GameObject panneauClavier;

    [Tooltip("Le prefab 'Touche' (un Button - TextMeshPro).")]
    public Button prefabTouche;

    [Header("Options")]
    [Tooltip("Coché : le clavier est caché au départ et s'ouvre quand on clique sur le InputField.")]
    public bool ouvrirAuClicSurLeChamp = true;

    // Disposition AZERTY : 4 lignes de 10 touches
    private readonly string[] lignes =
    {
        "1234567890",
        "AZERTYUIOP",
        "QSDFGHJKLM",
        "WXCVBN,.?!"
    };

    private bool majuscules = false;
    private TMP_Text texteToucheMaj;

    void Start()
    {
        if (champTexte == null || panneauClavier == null || prefabTouche == null)
        {
            Debug.LogError("ClavierVirtuel : remplissez Champ Texte, Panneau Clavier et Prefab Touche dans l'Inspector !");
            return;
        }

        // 1) Les touches normales : une touche par caractère
        foreach (string ligne in lignes)
        {
            foreach (char caractere in ligne)
            {
                string lettre = caractere.ToString();
                CreerTouche(lettre, () => Ecrire(lettre));
            }
        }

        // 2) Les touches spéciales
        Button toucheMaj = CreerTouche("maj", ChangerMajuscules);
        texteToucheMaj = toucheMaj.GetComponentInChildren<TMP_Text>();
        CreerTouche("Espace", () => Ecrire(" "));
        CreerTouche("Effacer", EffacerDernierCaractere);
        CreerTouche("OK", FermerClavier);

        // 3) Le clavier s'ouvre quand on sélectionne le InputField avec le laser
        if (ouvrirAuClicSurLeChamp)
        {
            champTexte.onSelect.AddListener(_ => OuvrirClavier());
            FermerClavier();
        }
    }

    // Crée une touche : copie du prefab + texte affiché + action au clic
    Button CreerTouche(string texte, UnityAction action)
    {
        Button touche = Instantiate(prefabTouche, panneauClavier.transform);
        touche.name = "Touche " + texte;

        TMP_Text etiquette = touche.GetComponentInChildren<TMP_Text>();
        if (etiquette != null)
            etiquette.text = texte;

        touche.onClick.AddListener(action);
        return touche;
    }

    void Ecrire(string texte)
    {
        champTexte.text += majuscules ? texte.ToUpper() : texte.ToLower();
    }

    void EffacerDernierCaractere()
    {
        string t = champTexte.text;
        if (t.Length > 0)
            champTexte.text = t.Substring(0, t.Length - 1);
    }

    void ChangerMajuscules()
    {
        majuscules = !majuscules;
        if (texteToucheMaj != null)
            texteToucheMaj.text = majuscules ? "MAJ" : "maj";
    }

    public void OuvrirClavier()
    {
        panneauClavier.SetActive(true);
    }

    public void FermerClavier()
    {
        panneauClavier.SetActive(false);
    }
}
