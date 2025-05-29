namespace KlaskMP
{
    /// <summary>
    /// List of all keys saved on the user's device, be it for settings or selections.
    /// </summary>
    public class PrefsKeys
    {
        /// <summary>
		/// PlayerPrefs key for player name: UserXXXX
		/// </summary>
        public const string playerName = "playerName";

        /// <summary>
        /// PlayerPrefs key for selected network mode: 0, 1 or 2
        /// </summary>
        public const string networkMode = "networkMode";

        /// <summary>
        /// PlayerPrefs key for selected game mode.
        /// </summary>
        public const string gameMode = "gameMode";

        /// <summary>
        /// Server address for manual connection, e.g. in LAN games.
        /// This is only used when using Photon Networking, as UNET
        /// does support broadcast and automatic server discovery.
        /// </summary>
        public const string serverAddress = "serverAddress";

        /// <summary>
        /// PlayerPrefs key for background music state: true/false
        /// </summary>
        public const string playMusic = "playMusic";

        /// <summary>
        /// PlayerPrefs key for global audio volume: 0-1 range
        /// </summary>
        public const string appVolume = "appVolume";
    }
}
