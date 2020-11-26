using UnityEngine;
using UnityEngine.AI;

public class RunTest : MonoBehaviour
{
    public Animator animator;
    public GameObject camera;
    public NavMeshAgent agent;
    public float timeScale;
    public float speed;



    // Start is called before the first frame update
    public void Start()
    {
        animator = GetComponent<Animator>();
        camera = GameObject.Find("Main Camera");
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    public void Update()
    {
        Time.timeScale = timeScale;
        agent.speed = speed * animator.GetCurrentAnimatorStateInfo(0).speed;
        agent.SetDestination(transform.position + Vector3.forward.normalized);
    }
}
