using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

namespace KlaskMP
{
    /// <summary>
    /// Networked player class implementing movement control and shooting.
    /// Contains both server and client logic in an authoritative approach.
    /// </summary> 
    public class Player : MonoBehaviourPunCallbacks, IPunObservable
    {
        /// <summary>
        /// Movement speed in all directions.
        /// </summary>
        public float moveSpeed;
        public float maxMoveSpeed;
        
        //reference to this rigidbody
        #pragma warning disable 0649
		private Rigidbody rb;
		#pragma warning restore 0649


        //initialize server values for this player
        void Awake()
        {
            //only let the master do initialization
            if(!PhotonNetwork.IsMasterClient)
                return;

            //DoSome();
        }


        /// <summary>
        /// Initialize synced values on every client.
        /// Initialize camera and input for this local client.
        /// </summary>
        void Start()
        {           
			//get corresponding team and colorize renderers in team color
            Team team = GameManager.GetInstance().teams[GetView().GetTeam()];

            //called only for this client 
            if (!photonView.IsMine)
                return;

			//set a global reference to the local player
            GameManager.GetInstance().localPlayer = this;

			//get components and set camera target
            rb = GetComponent<Rigidbody>();

            GameManager.GetInstance().ui.control.onDrag += Move;
            GameManager.GetInstance().ui.control.onDragEnd += MoveEnd;
        }


        /// <summary>
        /// This method gets called whenever player properties have been changed on the network.
        /// </summary>
        public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player player, ExitGames.Client.Photon.Hashtable playerAndUpdatedProps)
        {
            //only react on property changes for this player
            if(player != photonView.Owner)
                return;

            //update values that could change any time for visualization to stay up to date
            //SomeChange(someValue);
        }

        
        //this method gets called multiple times per second, at least 10 times or more
        void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {        
            if (stream.IsWriting)
            {             
                //here we send the turret rotation angle to other clients
                //stream.SendNext(turretRotation);
            }
            else
            {   
                //here we receive the turret rotation angle from others and apply it
                //this.turretRotation = (short)stream.ReceiveNext();
                //DoSome();
            }
        }
        
        /// <summary>
        /// Helper method for getting the current object owner.
        /// </summary>
        public PhotonView GetView()
        {
            return this.photonView;
        }


        //moves rigidbody
        void Move(Vector2 delta)
        {
            transform.rotation = Quaternion.identity;

            var clampDelta = Vector3.ClampMagnitude(delta, maxMoveSpeed) * Time.deltaTime * moveSpeed;

            var newPos = new Vector3(Mathf.Clamp(rb.position.x + clampDelta.x, -Consts.FieldWidth / 2f + Consts.StrikerRadius, Consts.FieldWidth / 2f - Consts.StrikerRadius), 
                    0f, Mathf.Clamp(rb.position.z + clampDelta.y, -Consts.FieldHeight / 2f + Consts.StrikerRadius, Consts.FieldHeight / 2f - Consts.StrikerRadius));
            
            rb.MovePosition(newPos);
        }


        //on movement drag ended
        void MoveEnd()
        {
            if (!rb.isKinematic)
            {
                //reset rigidbody physics values
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }

        /// <summary>
        /// Repositions in team area and resets camera & input variables.
        /// This should only be called for the local player.
        /// </summary>
        public void ResetPosition()
        {
            //get team area and reposition it there
            transform.position = GameManager.GetInstance().GetSpawnPosition(GetView().GetTeam());
            transform.rotation = Quaternion.identity;

            //reset forces modified by input
            if (!rb.isKinematic)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            //reset input 
            GameManager.GetInstance().ui.control.OnPointerUp(null);
        }


        //called on all clients on game end providing the winning team
        [PunRPC]
        protected void RpcGameOver(byte teamIndex)
        {
            //display game over window
            GameManager.GetInstance().DisplayGameOver(teamIndex);
        }

        void OnDestroy()
        {
            GameManager.GetInstance().ui.control.onDrag -= Move;
            GameManager.GetInstance().ui.control.onDragEnd -= MoveEnd;
        }
    }
}