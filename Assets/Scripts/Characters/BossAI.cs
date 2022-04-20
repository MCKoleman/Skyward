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

    // States
    protected enum State { ASLEEP, CASTING, TELEPORT, ATTACK, HALF };
    protected State curr = State.ASLEEP;

    // Misc.
    private bool gameOver = false;
    private bool halfSet = false;
    private int threshold;
    private float maxX, maxZ, minX, minZ;
    protected float dist;
    protected Vector3 dest;
    protected EnemyCharacter stats;
    protected BossAbilities abilities;

    // State timers
    public float minStateTime;
    public float maxStateTime;
    private float stateTimer = 0.0f;

    // Delays
    public float teleDelay;
    public float atkDelay;
    public float castDelay;
    public float halfDelay;
    public float deathDelay;

    // VFX
    //public ParticleSystem deathEffect; (see BossAbilities)

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        stats = GetComponent<EnemyCharacter>();
        abilities = GetComponent<BossAbilities>();
        threshold = stats.CurHealth/2;
        maxX = anchor.position.x + roomX;
        minX = anchor.position.x - roomX;
        maxZ = anchor.position.z + roomZ;
        minZ = anchor.position.z - roomZ;
    }

    // Update is called once per frame
    void Update()
    {
        dist = Vector3.Distance(transform.position, player.transform.position);

        if (stats.CurHealth > 0)
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
                case State.HALF:
                    HandleHalf();
                    break;
            }
        }
        else if (gameOver)
        {
            Debug.Log("DIE BITCH");
            EnterDeath();
        }

        stateTimer -= Time.deltaTime;
    }

    /*===============================UTILITY===============================*/
    protected void TurnToPlayer()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(
                new Vector3(player.transform.position.x, 0, player.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z)
                ),
            rotSpeed * Time.deltaTime);
    }

    protected void LockToPlayer()
    {
        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
    }

    public void WakeUp()
    {
        EnterCasting();
    }

    protected Vector3 GetRandomPoint()
    {
        float x;
        Random.InitState(System.DateTime.Now.Millisecond);

        if (maxX - transform.position.x > maxX/2)
        {
            x = Random.Range(maxX/2, maxX);
        }
        else
        {
            x = Random.Range(minX, maxX/2);
        }

        return new Vector3(x, transform.position.y, Random.Range(minZ, maxZ));
    }

    protected float GetStateTime()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        return Random.Range(minStateTime, maxStateTime);
    }

    /*===============================TRANSITION TO STATE===============================*/
    protected void EnterDeath()
    {
        // Only want to enter death once, so freeze AI and Character script manages invincibility
        gameOver = true;
        StopAllCoroutines();
        abilities.DisableAbilities();
        StartCoroutine(Death());
    }

    protected void EnterCasting()
    {
        Debug.Log("BOSS STATE | CASTING");
        transform.position = new Vector3(anchor.position.x, transform.position.y, anchor.position.z);
        curr = State.CASTING;
        StartCoroutine(Casting());
    }

    protected void EnterTeleport()
    {
        Debug.Log("BOSS STATE | TELEPORT");
        curr = State.TELEPORT;
        StartCoroutine(Teleport());
    }

    protected void EnterAttack()
    {
        Debug.Log("BOSS STATE | ATTACK");
        curr = State.ATTACK;
        dest = transform.position;
        StartCoroutine(Attack());
    }

    protected void EnterHalf()
    {
        Debug.Log("BOSS STATE | HALF-WAY");
        curr = State.HALF;
        halfSet = true;
        StartCoroutine(Half());
    }

    /*===============================HANDLE UPDATE-RELATED INFORMATION===============================*/
    protected void HandleCasting()
    {
        //TurnToPlayer();
    }

    protected void HandleTeleport()
    {
        LockToPlayer();
    }

    protected void HandleAttack()
    {
        //(this was causing issues with RotateAround; not needed so left out until fixed)
        //enabling it with 30 barrier rotSpeed was pretty interesting
        //TurnToPlayer();

        //move rb instead? no problems right now
        transform.position = Vector3.MoveTowards(transform.position, dest, movSpeed * Time.deltaTime);
    }

    protected void HandleHalf()
    {
        //Enter things here...
    }

    /*===============================STATE BEHAVIOURS===============================*/
    private IEnumerator Casting()
    {

        yield return new WaitForSeconds(castDelay); //animation placeholder

        //Maybe instead of having Half only occur once, make it available whenever health < threshold?
        if (!halfSet && stats.CurHealth <= threshold) {
            EnterHalf();
        }
        else
        {
            Random.InitState(System.DateTime.Now.Millisecond);
            if (Random.value > 0.5f)
            {
                EnterAttack();
            }
            else
            {
                EnterTeleport();
            }
        }

        yield break;
    }

    private IEnumerator Teleport()
    {
        yield return new WaitForSeconds(abilities.Teleport()); //initial teleport

        //don't want to waste state time on setting things up
        stateTimer = GetStateTime();

        while (stateTimer > 0)
        {
            transform.position = GetRandomPoint();
            yield return new WaitForSeconds(teleDelay); //animation placeholder
            yield return StartCoroutine(abilities.MagicMissiles()); //fire
            yield return new WaitForSeconds(abilities.Teleport());  //teleport
        }

        EnterCasting();
        yield break;
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(atkDelay);  //animation placeholder
        yield return StartCoroutine(abilities.MagicBarriers()); //generate barriers

        //don't want to waste state time on setting thigns up
        stateTimer = GetStateTime();

        while (stateTimer > 0)
        {
            if (transform.position == dest)
            {
                dest = GetRandomPoint(); //actual movement done in handler
            }

            yield return null;  //do this or INFINITE LOOP!
        }

        abilities.ReleaseBarriers();
        dest = transform.position;  //stop moving
        yield return new WaitForSeconds(abilities.Teleport());  //teleport
        EnterCasting();
        yield break;
    }

    private IEnumerator Half()
    {
        yield return new WaitForSeconds(halfDelay);  //animation placeholder
        yield return new WaitForSeconds(abilities.Teleport()); //initial teleport

        //Hide the boss (either shove it away somewhere off the screen, or disable renderer and collision, either works)
        transform.position = new Vector3(maxX + 10, transform.position.y, maxZ + 10);
        yield return StartCoroutine(abilities.SpawnBaddies(maxX, minX, minZ, maxZ));

        //don't want to waste state time on setting things up
        stateTimer = GetStateTime();

        while (stateTimer > 0)
        {
            yield return null;  //busy wait
        }

        yield return StartCoroutine(abilities.GoAwayBaddies());
        EnterCasting();
        yield break;
    }

    private IEnumerator Death()
    {
        yield return new WaitForSeconds(deathDelay); //Placeholder for any animation or dialogue
        yield return new WaitForSeconds(abilities.SelfDestruct());
        Destroy(this.gameObject);
    }
}
