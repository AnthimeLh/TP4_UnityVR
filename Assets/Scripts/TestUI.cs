using UnityEngine;
using TMPro;

// =====================================================================
//  EXERCICE 4 - Actions de test pour l'UI
//  À placer sur la Canvas du menu. Chaque méthode publique est reliée
//  à un élément de l'UI dans l'Inspector (On Click, On Value Changed...).
// =====================================================================
public class TestUI : MonoBehaviour
{
    [Tooltip("Un cube de la scène qui réagit au bouton et au slider.")]
    public Transform cubeTest;

    [Tooltip("Un texte (TextMeshPro) qui affiche ce qu'on a tapé.")]
    public TMP_Text texteMessage;

    // Bouton -> On Click ()
    public void ChangerCouleur()
    {
        if (cubeTest != null)
            cubeTest.GetComponent<Renderer>().material.color = Random.ColorHSV();
    }

    // Slider -> On Value Changed (choisir la version "Dynamic float")
    public void ChangerTaille(float valeur)
    {
        if (cubeTest != null)
            cubeTest.localScale = Vector3.one * valeur;
    }

    // InputField -> On Value Changed (choisir la version "Dynamic string")
    public void AfficherTexte(string texte)
    {
        if (texteMessage != null)
            texteMessage.text = "Vous avez écrit : " + texte;
    }
}
