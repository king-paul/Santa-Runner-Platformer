using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: James Kemeny
// Edited by : Paul King

public class NewLevelSpawner : MonoBehaviour
{
    private const float PLAYER_DIST_SPAWN_LEVEL_CHUNK = 70f; // How far away level parts spawn from the player

    private GameObject m_Player; // Player
    private int spawnIndex;

    //
    // Level Spawner Requirements:
    // - Spawned chunks need an end position node with the name EndPosition
    // - This is updated each time one is spawned, so that every chunk is sure to follow on from the last.
    //

    [Header("Level Parts")]
    [SerializeField] private Transform m_Start_Chunk = null; // Starting level part
    [SerializeField] private List<string> m_ChunkList = null; // SCUFFED List of chunk names 
    private Vector3 m_LastEndPos;
    
    [Space(10)]
    [Header("Level Settings")]
    [SerializeField] private float m_KillboxHeight = -30;
    
    void Start()
    {
        // Get the pool instance and find the player
        m_Player = GameObject.FindGameObjectWithTag("Player");
        spawnIndex = 0;

        // If theres no starting chunk set, setup a new one
        if (!m_Start_Chunk)
        {
            m_LastEndPos = SpawnLevelPart().Find("EndPosition").position;

        }
        else
        {
            // Set the last end pos to the start chunk's end pos
            m_LastEndPos = m_Start_Chunk.Find("EndPosition").position;
        }

    }

    void Update()
    {
        // Check the players distance for object spawning
        if(Vector3.Distance(m_Player.transform.position, m_LastEndPos) < PLAYER_DIST_SPAWN_LEVEL_CHUNK)
        {
            SpawnLevelPart();            
        }
    }

    /// <summary>
    /// Spawns the next levelPart object from a pool using the names of the objects you'd like to spawn
    /// Chunks must have an end position node, and lastLevelPos stores the last one and is updated each time one is spawned
    /// </summary>
    /// <returns></returns>
    private Transform SpawnLevelPart()
    {
        string chosenLevelPart = m_ChunkList[spawnIndex++];

        // reset back to the first part if there are no more in the list
        if (spawnIndex >= m_ChunkList.Count)
            spawnIndex = 0;

        //GameObject levelPart = m_ObjectPooler.SpawnFromPool(chosenLevelPart, m_LastEndPos, Quaternion.identity);

        //    if(levelPart.transform.Find("EndPosition") != null)
        //    {
        //        m_LastEndPos = levelPart.transform.Find("EndPosition").position;
        //    }

        //    //Spawn killbox below chunk
        //    GameObject killbox = new GameObject("KillBox");
        //    killbox.tag = "KillBox";

        //    killbox.transform.position = ((levelPart.transform.position + m_LastEndPos) / 2) + (transform.position + (Vector3.up * m_KillboxHeight));
        //    killbox.AddComponent<BoxCollider>().size = new Vector3(150, 40, 30);
        //    killbox.GetComponent<BoxCollider>().isTrigger = true;
        //    killbox.transform.SetParent(levelPart.transform);

        //    return levelPart.transform;

        return null;
    }
    

    private void OnDrawGizmos()
    {
        float height = 100;
        float length = 1000;
        float space = 10;
        Vector2Int dimensions = new Vector2Int(20, 10);


        Gizmos.color = Color.magenta;
        for (int x = 0; x < dimensions.x; x++)
        {
            for (int y = 0; y < dimensions.y; y++)
            {
                Vector3 coordy = Vector3.zero + (Vector3.up * (y * space));
                Gizmos.DrawLine(coordy, coordy + (Vector3.right * length));
                
            }
            Vector3 coordx = Vector3.zero + (Vector3.right * (x * space));
            Gizmos.DrawLine(coordx, coordx + (Vector3.up * height));
        }
        
        
        //Gizmos.color = Color.blue;
        //Gizmos.DrawCube(m_LastEndPos, Vector3.one * 3);
    }
}
