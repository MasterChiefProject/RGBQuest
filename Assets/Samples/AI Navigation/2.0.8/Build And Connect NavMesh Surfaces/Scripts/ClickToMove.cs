using UnityEngine;
using UnityEngine.AI;

namespace Unity.AI.Navigation.Samples
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
    public class ClickToMove : MonoBehaviour
    {
        NavMeshAgent _agent;
        Animator _anim;
        RaycastHit _hit;

        [SerializeField] float clickRayLength = 100f;   // optional tweak

        void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _anim = GetComponent<Animator>();
        }

        void Update()
        {
            /* -----------------------------------------------------------
             * Handle click
             * --------------------------------------------------------- */
            if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out _hit, clickRayLength))
                    _agent.destination = _hit.point;
            }

            /* -----------------------------------------------------------
             * Update animator
             * --------------------------------------------------------- */
            bool moving = _agent.velocity.sqrMagnitude > 0.01f;
            _anim.SetBool("Running", moving);
        }

        /* ---------------------------------------------------------------
         *  Only keep this block if you drive the model with root-motion.
         *  Otherwise let NavMeshAgent update the transform automatically
         *  and delete the whole OnAnimatorMove method.
         * ------------------------------------------------------------- */
        /*
        void OnAnimatorMove() {
            // If you switch to root-motion, uncomment the next two lines:
            // transform.position = _agent.nextPosition;
            // transform.rotation = Quaternion.LookRotation(_agent.velocity);

            // Do NOT overwrite agent.speed every frame; just set it once.
        }
        */
    }
}
