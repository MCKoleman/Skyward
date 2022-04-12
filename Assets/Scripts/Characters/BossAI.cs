using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : EnemyController
{
    [SerializeField]
    protected float roomX;
    [SerializeField]
    protected float roomZ;
    [SerializeField]
    protected Transform anchor;

    protected enum State { ASLEEP, CASTING, TELEPORT, ATTACK };
    protected State curr = State.ASLEEP;

    private float maxX, maxZ, minX, minZ;
    private int health;

    // State timers
    public float teleTime;
    public float atkTime;
    private float stateTimer = 0.0f;

    // Delays
    public float teleDelay;
    public float atkDelay;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        health = GetComponent<EnemyCharacter>().CurHealth;
        maxX = anchor.position.x + roomX;
        minX = anchor.position.x - roomX;
        maxZ = anchor.position.z + roomZ;
        minZ = anchor.position.z - roomZ;

        //testing
        curr = State.TELEPORT;
        stateTimer = teleTime;
    }

    // Update is called once per frame
    void Update()
    {
        FacePlayer();

        if (health > 0)
        {
            switch(curr)
            {
                case State.CASTING:
                    HandleCasting();
                    break;
                case State.TELEPORT:
                    HandleTeleport();
                    break;
                case State.ATTACK:
                    HandleAttack();
                    break;
            }
        }
        else
        {
            EnterDeath();
        }

        stateTimer -= Time.deltaTime;
    }

    protected void FacePlayer()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(
                new Vector3(player.transform.position.x, 0, player.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z)
                ),
            rotSpeed * Time.deltaTime);
    }

    public void WakeUp()
    {
        curr = State.CASTING;
    }

    protected Vector3 GetRandomPoint()
    {
        return new Vector3(Random.Range(minX, maxX), transform.position.y, Random.Range(minZ, maxZ));
    }

    protected void EnterDeath()
    {
        // Only want to enter death once, so set health and prevent further damaging
        health = 1;
        GetComponent<EnemyCharacter>().SetInvincible();
        StartCoroutine(Death());
    }

    protected void EnterCasting()
    {
        transform.position = new Vector3(anchor.position.x, anchor.position.z);
    }

    protected void EnterTeleport()
    {

    }

    protected void EnterAttack()
    {

    }

    protected void HandleCasting()
    {
        if (stateTimer > 0)
        {
            //stateTimer 
        }
        else
        {

        }
    }

    protected void HandleTeleport()
    {

    }

    protected void HandleAttack()
    {
        /*
        if ()
        {
            curr = State.CASTING
        }
        */
        // transform.position += new Vector3(transform.forward.x, 0, transform.forward.z) * movSpeed * Time.deltaTime;

    }

    private IEnumerator Attack()
    {
        while (stateTimer > 0)
        {

        }

        yield break;
    }

    private IEnumerator Teleport()
    {
        while (stateTimer > 0)
        {
            transform.position = GetRandomPoint();
            yield return new WaitForSeconds(teleDelay);
        }

        yield break;
    }

    private IEnumerator Death()
    {
        // Do stuff here...

        yield break;
    }

}
