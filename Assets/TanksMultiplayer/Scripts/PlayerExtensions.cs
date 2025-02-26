/*  This file is part of the "Tanks Multiplayer" project by FLOBUK.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace TanksMP
{
    /// <summary>
    /// This class extends Photon's PhotonPlayer object by custom properties.
    /// Provides several methods for setting and getting variables out of them.
    /// </summary>
    public static class PlayerExtensions
    {
        //keys for saving and accessing values in custom properties Hashtable
        public const string team = "team";


        /// <summary>
        /// Returns the networked player nick name.
        /// Offline: bot name. Online: PhotonPlayer name.
        /// </summary>
        public static string GetName(this PhotonView player)
        {
            if (PhotonNetwork.OfflineMode == true)
            {
                PlayerBot bot = player.GetComponent<PlayerBot>();
                if (bot != null)
                {
                    return bot.myName;
                }
            }

            return player.Owner.NickName;
        }

        /// <summary>
        /// Offline: returns the team number of a bot stored in PlayerBot.
        /// Fallback to online mode for the master or in case offline mode was turned off.
        /// </summary>
        public static int GetTeam(this PhotonView player)
        {
            if (PhotonNetwork.OfflineMode == true)
            {
                PlayerBot bot = player.GetComponent<PlayerBot>();
                if (bot != null)
                {
                    return bot.teamIndex;
                }
            }

            return player.Owner.GetTeam();
        }

        /// <summary>
        /// Online: returns the networked team number of the player out of properties.
        /// </summary>
        public static int GetTeam(this Photon.Realtime.Player player)
        {
            return System.Convert.ToInt32(player.CustomProperties[team]);
        }

        /// <summary>
        /// Offline: synchronizes the team number of a PlayerBot locally.
        /// Fallback to online mode for the master or in case offline mode was turned off.
        /// </summary>
        public static void SetTeam(this PhotonView player, int teamIndex)
        {
            if (PhotonNetwork.OfflineMode == true)
            {
                PlayerBot bot = player.GetComponent<PlayerBot>();
                if (bot != null)
                {
                    bot.teamIndex = teamIndex;
                    return;
                }
            }

            player.Owner.SetTeam(teamIndex);
        }

        /// <summary>
        /// Online: synchronizes the team number of the player for all players via properties.
        /// </summary>
        public static void SetTeam(this Photon.Realtime.Player player, int teamIndex)
        {
            player.SetCustomProperties(new Hashtable() { { team, (byte)teamIndex } });
        }

        /// <summary>
        /// Offline: clears all properties of a PlayerBot locally.
        /// Fallback to online mode for the master or in case offline mode was turned off.
        /// </summary>
        public static void Clear(this PhotonView player)
        {
            if (PhotonNetwork.OfflineMode == true)
            {
                PlayerBot bot = player.GetComponent<PlayerBot>();
                if (bot != null)
                {
                    //DoClear();
                    return;
                }
            }

            player.Owner.Clear();
        }


        /// <summary>
        /// Online: Clears all networked variables of the player via properties in one instruction.
        /// </summary>
        public static void Clear(this Photon.Realtime.Player player)
        {
            /*player.SetCustomProperties(new Hashtable() { { key1, (byte)0 },
                                                         { key2, (byte)0 },
                                                         { key3, (byte)0 } });*/
        }
    }
}
