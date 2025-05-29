using UnityEngine;
using Photon.Pun;

namespace KlaskMP
{
    /// <summary>
    /// Base class for all derived Collectibles (health, shields, etc.) consumed or carried around.
    /// Extend this to create highly customized Collectible with specific functionality.
    /// </summary>
	public class Collectible : MonoBehaviour
	{

        /// <summary>
        /// Persistent network (PhotonView) ID of the Player that picked up this Collectible.
        /// </summary>
        [HideInInspector]
        public int carrierId = -1;
        
                  
        /// <summary>
        /// Server only: check for players colliding with this item.
        /// Possible collision are defined in the Physics Matrix.
        /// </summary>
        public virtual void OnTriggerEnter(Collider col)
		{
            if (!PhotonNetwork.IsMasterClient)
                return;
            
    		GameObject obj = col.gameObject;
			Player player = obj.GetComponent<Player>();

            Debug.Log("Goal");
		}


        /// <summary>
        /// Tries to apply the Collectible to a colliding player. Returns 'true' if consumed.
        /// Override this method in your own Collectible script to implement custom behavior.
        /// </summary>
        public virtual bool Apply(Player p)
		{
            //do something to the player
            if (p == null)
                return false;
            else
                return true;
		}


        /// <summary>
        /// Virtual implementation called when this Collectible gets picked up.
        /// This is called for CollectionType = Pickup items only.
        /// </summary>
        public virtual void OnPickup()
        {
        }


        /// <summary>
        /// Virtual implementation called when this Collectible gets dropped on player death.
        /// This is called for CollectionType = Pickup items only.
        /// </summary>
        public virtual void OnDrop()
        {
        }


        /// <summary>
        /// Virtual implementation called when this Collectible gets returned.
        /// This is called for CollectionType = Pickup items only.
        /// </summary>
        public virtual void OnReturn()
        {
        }
    }
}
