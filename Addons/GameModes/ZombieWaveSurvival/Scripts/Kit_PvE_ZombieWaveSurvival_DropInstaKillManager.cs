using Photon.Pun;
using UnityEngine;

namespace MarsFPSKit
{
    namespace ZombieWaveSurvival
    {
        public class Kit_PvE_ZombieWaveSurvival_DropInstaKillManager : MonoBehaviourPun, IPunObservable
        {
            /// <summary>
            /// There should only be one of these at all times
            /// </summary>
            public static Kit_PvE_ZombieWaveSurvival_DropInstaKillManager instance;
            [HideInInspector]
            /// <summary>
            /// Until which <see cref="PhotonNetwork.Time"/> should we live?
            /// </summary>
            public double liveUntil;

            GameObject instaKillUI;

            private void Start()
            {
                instance = this;

                instaKillUI = GameObject.Find("MarsFPSKit_IngamePrefab/UI/HUD/Root/Root (Can be hidden)/TempDropsUI/InstaKill");
                instaKillUI.SetActive(true);
                //Set initial time
                liveUntil = PhotonNetwork.Time + (float)photonView.InstantiationData[0];
            }

            private void Update()
            {
                if (photonView.IsMine)
                {
                    //Find all zombies
                    Kit_PvE_ZombieWaveSurvival_ZombieAI[] zombies = FindObjectsOfType<Kit_PvE_ZombieWaveSurvival_ZombieAI>();

                    for (int i = 0; i < zombies.Length; i++)
                    {
                        //Set HP down to 1
                        zombies[i].InstaKill();
                    }

                    if (PhotonNetwork.Time >= liveUntil)
                    {
                        for (int i = 0; i < zombies.Length; i++)
                        {
                            //Rest HP to original health 
                            zombies[i].OriginalHealth();
                        }
                        instaKillUI.SetActive(false);
                        PhotonNetwork.Destroy(gameObject);
                    }
                }
            }

            void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
            {
                if (stream.IsWriting)
                {
                    stream.SendNext(liveUntil);
                }
                else
                {
                    liveUntil = (double)stream.ReceiveNext();
                }
            }
        }
    }
}