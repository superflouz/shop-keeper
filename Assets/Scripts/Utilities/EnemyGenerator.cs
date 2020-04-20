using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum EnemyIndex
{
    GoblinClub = 0,
    GoblinSling,
    GoblinDrum,
    Harpy,
    GoblinKing,
    RedLizard,
    GreenLizard
}

[System.Serializable]
public struct ZoneParameter
{
    public List<WaveEnemy> waveEnemies;
    public int waveCount;
    public int firstWaveBudget;
    public int incrementBudget;
    public EnemyIndex boss;
}

[System.Serializable]
public enum WavePosition
{
    Front,
    Back
}

[System.Serializable]
public struct WaveEnemy
{
    public EnemyIndex enemy;
    public WavePosition position;
    public int firstWaveSpawnable;
}

public class EnemyGenerator : MonoBehaviour
{
    public ShopManager shopManager;

    // List of all enemies
    public List<Entity> Enemies;

    public List<ZoneParameter> zones;


    // Store the index for the entities of the next wave
    private List<EnemyIndex> nextWave = new List<EnemyIndex>();
    // Next wave values
    private int nextWaveNumber;
    private int nextWaveBudget;
    private int nextWaveZone;
    private bool waveReady;

    private Party party;

    // Start is called before the first frame update
    void Awake()
    {
        party = GetComponent<Party>();
    }

    private void Start()
    {
        waveReady = false;
        nextWaveNumber = 0;
        nextWaveBudget = zones[0].firstWaveBudget;
    }

    // Update is called once per frame
    void Update()
    {
        // Generate the wave if not ready
        if (!waveReady)
            GenerateNextWave();
        // Wait until the last wave is cleared before spawning the new wave
        if (waveReady)
        {
            if (party.entities.Count == 0)
            {
                // Instanciate every enemy of the wave
                int offset = 0;
                foreach (EnemyIndex enemy in nextWave)
                {
                    Entity entity = Instantiate(Enemies[(int)enemy], transform.position + Vector3.right * (20 + offset), Quaternion.identity);
                    offset += entity.slotCount;
                    entity.killEvent += shopManager.GainEntityReward;
                    party.AddToParty(entity);
                }
                waveReady = false;
                // Increment the number for the next wave
                nextWaveNumber++;
                // Set the budget of the next wave
                nextWaveBudget = zones[nextWaveZone].firstWaveBudget + (nextWaveNumber * zones[nextWaveZone].incrementBudget);
                // Clear the buffer
                nextWave.Clear();
            }
        }
    }

    /// <summary>
    /// Add an enemy to the wave.
    /// </summary>
    void GenerateNextWave()
    {
        // If the wave number is higher than the wave count, it's the boss wave
        if (nextWaveNumber >= zones[nextWaveZone].waveCount)
        {
            nextWave.Add(zones[nextWaveZone].boss);
            waveReady = true;

            // Setup the next zone
            nextWaveNumber = 0;
            if (nextWaveZone < zones.Count - 1)
                nextWaveZone++;
            else
                nextWaveZone = 0;

            nextWaveBudget = zones[nextWaveZone].firstWaveBudget;

            return;
        }

        // Normal Wave
        // Generate a random number who will be the first index checked
        int randomOffset = Random.Range(0, zones[nextWaveZone].waveEnemies.Count - 1);

        bool suitableEnemyFound = false;
        int index = randomOffset;
        // Iterate the available enemies for the wave, starting at random offset
        for (int i = 0; i < zones[nextWaveZone].waveEnemies.Count; i++)
        {
            // Caclulate the index to cycle back when it's higher than the number of enemies available
            if (index + i >= zones[nextWaveZone].waveEnemies.Count)
                index -= zones[nextWaveZone].waveEnemies.Count;

            // Check if the enemy can be spawn in this wave or if it's too early
            if (zones[nextWaveZone].waveEnemies[index + i].firstWaveSpawnable > nextWaveNumber)
                continue;
            
            // Check if the budget allow to spawn this enemy
            Buyable buyable = Enemies[(int)zones[nextWaveZone].waveEnemies[index + i].enemy].GetComponent<Buyable>();
            if (buyable.price > nextWaveBudget)
                continue;

            // If the two conditions are checked, the enemy is valid
            nextWaveBudget -= buyable.price;
            WaveEnemy selectedEnemy = zones[nextWaveZone].waveEnemies[index + i];

            // Put the enemy at the right place in the wave
            switch (selectedEnemy.position)
            {
                case WavePosition.Front:
                    nextWave.Insert(0, selectedEnemy.enemy);
                    break;
                case WavePosition.Back:
                    nextWave.Add(selectedEnemy.enemy);
                    break;
            }
            suitableEnemyFound = true;
            break;
        }

        // If no suitable enemy is found in the cycle, it means the wave is complete and ready to spawn
        waveReady = !suitableEnemyFound;
    }
}
