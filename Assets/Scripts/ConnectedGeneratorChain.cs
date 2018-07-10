using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "BoardGeneration/Generators/ConnectGeneratorsWithTunnels")]
public class ConnectedGeneratorChain : Generator
{

    public GeneratorTunneler tunnelTemplate;


    public override void Generate(BoardGenerator boardGenerator)
    {
        GenerateChain(boardGenerator);
    }

    void GenerateChain(BoardGenerator boardGenerator)
    {
        List<GridPosition> positionsToConnectWithTunnels = new List<GridPosition>();
        for (int i = 0; i < boardGenerator.generatorSpaceLists.Length; i++)
        {
            

           GridPosition randomPosition = boardGenerator.generatorSpaceLists[i].GetRandomRecordedPositionFromGenerator();
            positionsToConnectWithTunnels.Add(randomPosition);
        }
        if (positionsToConnectWithTunnels.Count > 0)
        {
            ConnectRooms(boardGenerator, positionsToConnectWithTunnels[0], positionsToConnectWithTunnels);
        }
        
    }

    void ConnectRooms(BoardGenerator boardGenerator, GridPosition firstRoomPosition, List<GridPosition> roomPositions)
    {

        for (int i = 0; i < roomPositions.Count; i++)
        {
            tunnelTemplate.DigTunnel(boardGenerator, firstRoomPosition, roomPositions[i]);
        }

        tunnelTemplate.DigTunnel(boardGenerator, roomPositions[roomPositions.Count - 1], firstRoomPosition);

    }

}
