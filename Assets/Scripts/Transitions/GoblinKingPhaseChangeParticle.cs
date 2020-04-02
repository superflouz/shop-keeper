using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinKingPhaseChangeParticle : Particle
{
    public FallingObject coin;
    public List<FallingObject> goblins = new List<FallingObject>(4);


    public override void Execute()
    {
        // Generate coins
        for (int i = 0; i < 50; i++)
        {
            FallingObject newCoin = Instantiate(coin, transform.position + Vector3.up * 1, Quaternion.identity);

            float direction = Random.Range(-2f, 2f);

            newCoin.transform.position += Vector3.right * (direction / 2);
            newCoin.Speed = Vector3.right * direction + Vector3.up * Random.Range(1f, 4f);

            Rotation rotation = newCoin.GetComponent<Rotation>();
            rotation.RotationAngle = Random.Range(-90, 90);
        }

        // Generate Goblins
        for (int i = 0; i < 4; i++)
        {
            FallingObject newGoblin = Instantiate(goblins[i], transform.position + Vector3.up * 0.3f, Quaternion.identity);

            float direction = 0;
            switch (i)
            {
                case 0:
                    direction = -2;
                    break;
                case 1:
                    direction = -0.2f;
                    break;
                case 2:
                    direction = 0.2f;
                    break;
                case 3:
                    direction = 2;
                    break;
            }

            newGoblin.transform.position += Vector3.right * (direction / 2);
            newGoblin.Speed = Vector3.right * direction + Vector3.up * 2;

            Rotation rotation = newGoblin.GetComponent<Rotation>();
            rotation.RotationAngle = Random.Range(-90, 90);
        }
    }
}
