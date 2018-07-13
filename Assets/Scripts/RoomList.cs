using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strata
{
    [CreateAssetMenu(menuName = "2DRL/RoomList")]
    public class RoomList : ScriptableObject
    {

        public List<RoomTemplate> roomList;
    }
}

