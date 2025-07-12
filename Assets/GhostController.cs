using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Linq;

public class GhostController : MonoBehaviour
{
    public enum GhostID { Blinky, Pinky, Inky, Clyde }
    public GhostID id;
    public float scatterTime = 7, chaseTime = 20;

    NavMeshAgent agent;
    Transform player;
    Transform[] nodes;

    enum Mode { Scatter, Chase }
    Mode mode = Mode.Scatter;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nodes = GameObject.FindGameObjectsWithTag("Node")
                           .Select(go => go.transform).ToArray();
    }
    void Start() => StartCoroutine(ModeLoop());

    /*----------------------------------------------------- MAIN UPDATE */
    void Update()
    {
        if (agent.remainingDistance > 0.1f) return;   // still moving

        Vector3 target = (mode == Mode.Scatter) ? CornerTarget() : ChaseTarget();
        Transform next = NearestNodeTo(target);
        agent.SetDestination(next.position);
    }

    /*------------------------- Pac-Man targets ------------------------*/
    Vector3 CornerTarget()
    {
        switch (id)
        {
            case GhostID.Blinky: return nodes.OrderBy(n => n.position.sqrMagnitude).Last().position;
            case GhostID.Pinky: return new Vector3(-10, 0, 10);   // top-left corner
            case GhostID.Inky: return new Vector3(10, 0, 10);   // top-right
            default: return new Vector3(-10, 0, -10);   // bottom-left
        }
    }
    Vector3 ChaseTarget()
    {
        Vector3 pPos = player.position;
        switch (id)
        {
            case GhostID.Blinky:                         // directly at player
                return pPos;
            case GhostID.Pinky:                          // 4 tiles (≈4 m) ahead
                return pPos + player.forward * 4f;
            case GhostID.Inky:                           // vector trick
                Vector3 bl = GameObject.Find("Ghost_Red").transform.position;
                Vector3 twoAhead = pPos + player.forward * 2f;
                return twoAhead + (twoAhead - bl);
            case GhostID.Clyde:                          // chase far, scatter near
                return Vector3.Distance(transform.position, pPos) < 8f
                     ? CornerTarget() : pPos;
            default:
                return pPos;
        }
    }

    Transform NearestNodeTo(Vector3 pos) =>
        nodes.OrderBy(n => (n.position - pos).sqrMagnitude).First();

    /*------------------------- Mode scheduler -------------------------*/
    IEnumerator ModeLoop()
    {
        while (true)
        {
            mode = Mode.Scatter;
            yield return new WaitForSeconds(scatterTime);
            mode = Mode.Chase;
            yield return new WaitForSeconds(chaseTime);
        }
    }
}
