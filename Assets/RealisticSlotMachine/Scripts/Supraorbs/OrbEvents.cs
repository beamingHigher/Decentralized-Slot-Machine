using UnityEngine;

namespace Mkey
{
    public class OrbEvents : MonoBehaviour
    {

        private OrbHolder ORB { get { return OrbHolder.Instance; } }

        void Start()
        {
            OrbHolder.LogoutEvent += SetDefName;
        }

        void OnDestroy()
        {
            OrbHolder.LogoutEvent -= SetDefName;
        }

        public void SetPlayerName(bool logined, string playerID, string playerName)
        {
            if (logined)
            {
                ORB.playerID = playerID;
                ORB.playerName = playerName;
            }
        }

        public void SetDefName()
        {
            ORB.playerID = "";
            ORB.playerName = "";
        }
    }
}