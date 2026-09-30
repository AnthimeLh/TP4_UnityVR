using UnityEngine;
using UnityEngine.InputSystem;

// =====================================================================
//  EXERCICE 3 - Déplacement avec les joysticks
//  À placer sur l'objet "XR Origin (XR Rig)".
//
//  - Joystick GAUCHE : avancer, reculer, aller à gauche, aller à droite
//  - Joystick DROIT  : tourner par crans de 30° ou 45° ("snap turn")
//
//  En VR, on ne bouge jamais la caméra directement : c'est le casque
//  qui la pilote. On déplace et on tourne son parent, le XR Origin.
// =====================================================================
public class DeplacementJoystick : MonoBehaviour
{
    [Header("Caméra du casque")]
    [Tooltip("Laissez vide : la Main Camera est trouvée automatiquement.")]
    public Transform cameraVR;

    [Header("Déplacement (joystick gauche)")]
    [Tooltip("Vitesse en mètres par seconde.")]
    public float vitesse = 2f;

    [Header("Rotation (joystick droit)")]
    [Tooltip("Angle de chaque cran de rotation : 30 ou 45 degrés.")]
    public float angleRotation = 45f;

    [Tooltip("Jusqu'où il faut pousser le joystick pour tourner (entre 0 et 1).")]
    [Range(0.3f, 1f)] public float seuilRotation = 0.7f;

    [Header("Entrées Input System (déjà configurées)")]
    // Chemin = "manette gauche / axe 2D principal", c'est-à-dire le joystick gauche
    public InputAction joystickGauche = new InputAction(
        "Joystick gauche", InputActionType.Value,
        "<XRController>{LeftHand}/{Primary2DAxis}", expectedControlType: "Vector2");

    // Même chose pour la manette droite
    public InputAction joystickDroit = new InputAction(
        "Joystick droit", InputActionType.Value,
        "<XRController>{RightHand}/{Primary2DAxis}", expectedControlType: "Vector2");

    // Vrai quand le joystick droit est revenu au centre (évite de tourner en boucle)
    private bool peutTourner = true;

    void OnEnable()
    {
        // Une action de l'Input System doit être activée pour pouvoir être lue
        joystickGauche.Enable();
        joystickDroit.Enable();
    }

    void OnDisable()
    {
        joystickGauche.Disable();
        joystickDroit.Disable();
    }

    void Start()
    {
        if (cameraVR == null && Camera.main != null)
            cameraVR = Camera.main.transform;

        if (cameraVR == null)
            Debug.LogError("DeplacementJoystick : aucune caméra trouvée. Glissez la Main Camera dans le champ 'Camera VR'.");
    }

    void Update()
    {
        if (cameraVR == null) return;

        Deplacer();
        Tourner();
    }

    // ----- Joystick gauche : déplacement -----
    void Deplacer()
    {
        // x = gauche/droite, y = avant/arrière (valeurs entre -1 et 1)
        Vector2 entree = joystickGauche.ReadValue<Vector2>();

        // Zone morte : on ignore les tout petits mouvements du joystick
        if (entree.magnitude < 0.1f) return;

        // "Devant" = là où le joueur regarde, mais à plat (on ne s'envole pas)
        Vector3 devant = cameraVR.forward;
        devant.y = 0f;
        devant.Normalize();

        Vector3 droite = cameraVR.right;
        droite.y = 0f;
        droite.Normalize();

        Vector3 direction = devant * entree.y + droite * entree.x;

        // On déplace tout le rig : la caméra et les manettes suivent
        transform.position += direction * vitesse * Time.deltaTime;
    }

    // ----- Joystick droit : rotation par crans -----
    void Tourner()
    {
        float x = joystickDroit.ReadValue<Vector2>().x;

        if (peutTourner && Mathf.Abs(x) >= seuilRotation)
        {
            float angle = (x > 0f) ? angleRotation : -angleRotation;

            // On tourne autour de la TÊTE du joueur, pas autour du centre du rig
            transform.RotateAround(cameraVR.position, Vector3.up, angle);

            peutTourner = false; // il faudra relâcher le joystick pour retourner
        }
        else if (Mathf.Abs(x) < 0.2f)
        {
            peutTourner = true; // joystick revenu au centre
        }
    }
}
