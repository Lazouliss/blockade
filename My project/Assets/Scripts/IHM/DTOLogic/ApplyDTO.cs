using System.Collections.Generic;
using UnityEngine;
using blockade.Blockade_common;
using System.Collections;

namespace blockade.Blockade_IHM
{
    public class ApplyDTO : MonoBehaviour
    {

        public const float LENGTH_TILE = 1;
        public Pawn selectedPawn;
        private Stack<GameObject> stackWall;
        public float speed = 0.5f;
        public GameObject wall;

        private IHM ihm;

        // Constructor
        public void initApplyDTO(IHM ihm) 
        {
            stackWall = new Stack<GameObject>();
            this.ihm = ihm;
        }

        /// <summary>
        /// Par Thomas MONTIGNY
        /// 
        /// Encapsuler le lancement de Coroutine pour etre appeler par un autre script
        /// 
        /// Publique
        /// </summary>
        /// <param name="dto"></param>
        public void moveDTOPawn(Common.DTOPawn dto)
        {
            StartCoroutine(movePawn(dto));
        }

        /// <summary>
        /// Par Wassim BOUKHARI
        /// 
        /// Lance une coroutine qui déplace le pion 
        /// pour chaque direction
        /// 
        /// </summary>
        /// <param name="dto"></param>
        private IEnumerator movePawn(Common.DTOPawn dto)
        {

            Pawn p = this.selectedPawn;

            foreach (Common.Direction direction in (List<Common.Direction>)(dto.mooves))
            {
                yield return StartCoroutine(p.move(direction));
            }

            selectedPawn = null;

        }

        /// <summary>
        /// Par Wassim BOUKHARI
        /// 
        /// Action du mur en fonction du dto
        /// 
        /// </summary>
        /// <param name="dto"></param>
        public void actionWall(Common.DTOWall dto, Board board)
        {

            Debug.Log("Debut du actionWall");
            // test if dto.isAdd is initialized (if not, count as false) and if yes, check is value
            if (dto.isAdd.HasValue && dto.isAdd.Value)
                addWall(dto, board);
            else
                removeWall();

        }

        /// <summary>
        /// Par Wassim BOUKHARI
        /// 
        /// Deplace le mur en fonction du dto
        /// 
        /// </summary>
        /// <param name="dto"></param>
        void addWall(Common.DTOWall dto, Board b)
        {
            Debug.Log("Debut du addWall");
            int angle;
            float x, z;


            if (dto.direction == Common.Direction.UP || dto.direction == Common.Direction.DOWN)
            {

                angle = 90;
                x = Mathf.Min(dto.coord1.Item1, dto.coord2.Item1) + 0.5f;
                if (dto.direction == Common.Direction.UP)
                    z = dto.coord1.Item2 + (LENGTH_TILE);
                else
                    z = dto.coord1.Item2;

                // adjust position
                z = z - 0.5f;

            }
            else
            {

                angle = 0;
                z = Mathf.Min(dto.coord1.Item2, dto.coord2.Item2) + 0.5f;
                if (dto.direction == Common.Direction.RIGHT)
                    x = dto.coord1.Item1 + (LENGTH_TILE);
                else
                    x = dto.coord1.Item1;

                // adjust position
                x = x - 0.5f;
            }

            Quaternion rotation = Quaternion.Euler(0f, angle, 0f);

            // Get the wall directly with the prefab -> TODO : Resources.Load<GameObject>("Wall")
            GameObject newObject = Instantiate(wall, new Vector3(x, 0.5f, z), rotation);
            newObject.transform.SetParent(b.transform, false);

            Animator animator = newObject.GetComponent<Animator>();

            animator.SetTrigger("descendMur");

            // Attend la fin de l'animation avant de changer de perspective de joueur
            StartCoroutine(DelayedSwitchPlayer(animator.GetCurrentAnimatorStateInfo(0).length));

            stackWall.Push(newObject);
        }

        /// <summary>
        /// Par Thomas MONTIGNY
        /// 
        /// Attend un certain delais avant de changer de perspective de joueur
        /// </summary>
        /// <param name="delay"></param>
        /// <returns></returns>
        IEnumerator DelayedSwitchPlayer(float delay = 0)
        {
            Debug.Log("Delayed animation");
            yield return new WaitForSeconds(delay);

            // Tourne la camera
            ihm.SwitchPlayerCamera();
        }

        /// <summary>
        /// Par Wassim BOUKHARI
        /// 
        /// Supprime le mur
        /// 
        /// </summary>
        void removeWall()
        {

            // Verifie d'abord si l'objet existe avant de le supprimer
            if (stackWall.Count > 0)
            {
                GameObject wall = stackWall.Pop();


                Animator animator = wall.GetComponent<Animator>();
                animator.SetTrigger("monterMur");


            }
            else
            {
                Debug.LogWarning("La pile est vide, plus de mur à retirer");
            }

        }

    }
}