using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

namespace Mkey
{
    public class SupraorbsGUIController : MonoBehaviour
    {
        [SerializeField]
        private Text loginText;
        [SerializeField]
        private GameObject avatarGroup;

        [DllImport("__Internal")]
        private static extern void OrbGuiLoaded();

        [DllImport("__Internal")]
        private static extern void CoinsUpdated(int coins);

        #region temp vars
        private SlotPlayer MPlayer { get { return SlotPlayer.Instance; } }
        private OrbHolder ORB { get { return OrbHolder.Instance; } }
        #endregion temp vars

        #region regular
        private void Start()
        {
            OrbHolder.LoginEvent += SupraorbsLoginHandler;
            OrbHolder.LogoutEvent += SupraorbsLogoutHandler;
            MPlayer.ChangeCoinsEvent += CoinUpdateHandler;
            Refresh();
            OrbGuiLoaded();
        }

        private void OnDestroy()
        {
            OrbHolder.LoginEvent -= SupraorbsLoginHandler;
            OrbHolder.LogoutEvent -= SupraorbsLogoutHandler;
        }
        #endregion regular

        private void Refresh()
        {
            if (loginText) loginText.text = (!ORB.IsLogined) ? "SUPRAORBS" : ORB.playerName;
        }

        #region event handlers
        private void SupraorbsLoginHandler(bool logined, string message)
        {
            Refresh();
            if (logined) MPlayer.AddFbCoins();
        }

        private void SupraorbsLogoutHandler()
        {
            Refresh();
        }

        private void CoinUpdateHandler(int coins)
        {
            CoinsUpdated(coins);
        }
        #endregion event handlers
    }
}
