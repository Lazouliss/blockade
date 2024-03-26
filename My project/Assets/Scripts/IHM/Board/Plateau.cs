using UnityEngine;
using UnityEngine.EventSystems;
using blockade.Blockade_common;

//ABERKANE Doha
public class Plateau : MonoBehaviour
{
    public GameObject case_plateau;
    private int width = 11;
    private int height = 14;

    private Common.DTOPawn DTO_joueur1_pion1, DTO_joueur1_pion2;
    private Common.DTOPawn DTO_joueur2_pion1, DTO_joueur2_pion2;

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

        //initialisation des pions de départ
        //pour le joueur 1
        DTO_joueur1_pion1.startPos = (4, 4);
        DTO_joueur1_pion2.startPos = (8, 4);

        //joueur1_pion1;

        //pour le joueur 2
        DTO_joueur2_pion1.startPos = (4, 11);
        DTO_joueur2_pion2.startPos = (8, 11);

        //creation des pions 
        /*
        GameObject joueur1_pion1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        GameObject joueur1_pion2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        GameObject joueur2_pion1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        GameObject joueur2_pion2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        //affectation d'une taille aux sphères
        joueur1_pion1.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); 
        joueur1_pion2.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); 
        joueur2_pion1.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); 
        joueur2_pion2.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); 
        -------------------------------------------------------------------

        //affectation couleurs aux pions sphériques
        //joueur1
        Renderer pionRendererJ1P1 = joueur1_pion1.GetComponent<Renderer>();
        Renderer pionRendererJ1P2 = joueur1_pion2.GetComponent<Renderer>();

        pionRendererJ1P1.material.SetColor("_Color", Color.magenta);
        pionRendererJ1P2.material.SetColor("_Color", Color.magenta);


        //joueur2
        Renderer pionRendererJ2P1 = joueur2_pion1.GetComponent<Renderer>();
        Renderer pionRendererJ2P2 = joueur2_pion2.GetComponent<Renderer>();

        pionRendererJ2P1.material.SetColor("_Color", Color.green);
        pionRendererJ2P2.material.SetColor("_Color", Color.green);
        */
        //initialisation case couleur
        Color couleurCase;

        //boucle qui va créer les cases du plateau
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //case_plateau = new GameObject((x,y).ToString());
                
                //creation du cube 
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

                //afficher les coordonnées sur le nom des cubes
                cube.name = (x,y).ToString();

                //affectation d'un collider pour qu'il puisse être clickable
                cube.AddComponent<BoxCollider>();

                //affection d'une texture
                var cubeRenderer = cube.GetComponent<Renderer>();   

                //ajout d'une couleur change 1 fois sur 2 
                if ((x + y) % 2 == 0) {
                    couleurCase = Color.white;
                } else {
                    couleurCase = Color.black;
                }               
                
                cubeRenderer.material.SetColor("_Color", couleurCase);

                //creation de la case de position 
                Vector3 position = new Vector3(x, 0f, y);

                //intantiation du cube à la case correspondante dans la plateau
                GameObject plateau = Instantiate(cube, position, Quaternion.identity);
                plateau.transform.SetParent(transform);
                Debug.Log("Position: " + position);

                CaseClickHandler clickHandler = plateau.AddComponent<CaseClickHandler>(); //ajout d'un click handler pour les cases du plateau
                clickHandler.plateau = this; //référence de du plateau à chaque CaseClickHandler

                /*
                 DTO_joueur1_pion1.startPos = (4, 4);
                 DTO_joueur1_pion2.startPos = (8, 4);
                 DTO_joueur2_pion1.startPos = (4, 11);
                 DTO_joueur2_pion2.startPos = (8, 11);
                */
                if (x == 3 && y == 3)
                {
                    GameObject joueur1_pion1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    joueur1_pion1.transform.position = position + new Vector3(0, 0.5f, 0); 
                    Renderer pionRendererJ1P1 = joueur1_pion1.GetComponent<Renderer>();
                    pionRendererJ1P1.material.SetColor("_Color", Color.magenta);


                }
                else if (x == 7 && y == 3)
                {
                    GameObject joueur1_pion2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    joueur1_pion2.transform.position = position + new Vector3(0, 0.5f, 0); 
                    Renderer pionRendererJ1P2 = joueur1_pion2.GetComponent<Renderer>();
                    pionRendererJ1P2.material.SetColor("_Color", Color.magenta);


                }
                else if (x == 3 && y == 10)
                {
                    GameObject joueur2_pion1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    joueur2_pion1.transform.position = position + new Vector3(0, 0.5f, 0); 
                    Renderer pionRendererJ2P1 = joueur2_pion1.GetComponent<Renderer>();    
                    pionRendererJ2P1.material.SetColor("_Color", Color.green);

               
                }
                else if (x == 7 && y == 10)
                {
                    GameObject joueur2_pion2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    joueur2_pion2.transform.position = position + new Vector3(0, 0.5f, 0);
                    Renderer pionRendererJ2P2 = joueur2_pion2.GetComponent<Renderer>();    
                    pionRendererJ2P2.material.SetColor("_Color", Color.green);
                }
            }
        }
    }
    
    //fonction qui va effectuer des actions lorsqu'on click sur une case 
    public void GestionCaseClick(Vector3 position)
    {
        Debug.Log("Case cliquée : " + (position.x,position.y));
    }
}

//ABERKANE Doha
public class CaseClickHandler : MonoBehaviour
{
    public Plateau plateau;

    private void OnMouseDown()
    {
        // Lorsqu'un cube est cliqué, appeler la fonction de gestion des cliques dans le script du plateau
        plateau.GestionCaseClick(transform.position);
    }
}
