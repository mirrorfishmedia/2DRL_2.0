using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strata
{
    [CreateAssetMenu(menuName = "Strata/Collections/RoomList")]
    public class RoomList : ScriptableObject
    {
        public List<RoomTemplate> roomList;

        public void RemoveEmptyEntriesThenAdd(RoomTemplate templateToAdd)
        {
            for (int i = roomList.Count - 1; i >= 0; i--)
            {
                if (roomList[i] == null)
                {
                    roomList.RemoveAt(i);
                }
            }

            roomList.Add(templateToAdd);
        }
    }

    
}

