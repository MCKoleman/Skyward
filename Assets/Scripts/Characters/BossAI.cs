using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : EnemyController
{

    [SerializeField]
    protected float roomX;
    [SerializeField]
    protected float roomY;

    protected enum State { ASLEEP, CASTING, ATTACK, DEAD };
    protected State curr = State.ASLEEP;

    private float maxX, maxY, minX, minY;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();


    }

    // Update is called once per frame
    void Update()
    {
        //Look at player

        //
    }


}
