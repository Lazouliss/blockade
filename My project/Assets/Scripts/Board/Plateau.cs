using UnityEngine;
using UnityEngine.EventSystems;

public class Plateau : MonoBehaviour
{
    public GameObject case_plateau;
    private int width = 11;
    private int height = 14;
    private Material defaultMaterial; 

    void Start()
    {
        Init_Plateau();
    }
    //pion qui possède click handler
    //avoir un save du pion actuel 
    //case sur laquelle il a clické

    //Initialisation du plateau
    public void Init_Plateau()
    {
        defaultMaterial = case_plateau.GetComponent<Renderer>().sharedMaterial;//get le matériel partagé de la case

        //boucle qui va créer les cases du plateau
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(x, 0f, y);
                GameObject instance = Instantiate(case_plateau, position, Quaternion.identity);
                instance.transform.SetParent(transform);
                Debug.Log("Position: " + position);
                //CaseClickHandler clickHandler = instance.AddComponent<CaseClickHandler>(); //ajout d'un click handler pour les cases du plateau
                //clickHandler.plateau = this; //référence de l'instance du plateau à chaque CaseClickHandler
            }
        }
    }

    /*
    // Gestion des clics sur les cases du plateau
    public class CaseClickHandler : MonoBehaviour, IPointerClickHandler
    {
        public Plateau plateau;
        private Material material;

        void Start()
        {
            material = GetComponent<Renderer>().sharedMaterial; //get le matériel partagé de la case
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //fonction qui va changer la couleur du matériel lorsqu'une case est cliquée
            if (material != null)
            {
                material.color = Color.gray;
            }
        }

        public void Deselect()
        {
            //fonction qui va réinitialiser la couleur de la case à celle par défaut
            if (material != null && plateau.defaultMaterial != null)
            {
                material.color = plateau.defaultMaterial.color;
            }
        }
    }*/
}
