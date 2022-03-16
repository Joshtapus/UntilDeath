﻿using System.Collections.Generic;
using UnityEngine;

namespace MarsFPSKit
{
    /// <summary>
    /// This spawn system will check if enemy distance are withhin distance
    /// </summary>
    [CreateAssetMenu(menuName = "MarsFPSKit/Spawn Systems/Distance Spawn System")]
    public class Kit_SpawnSystemDistanceCheck : Kit_SpawnSystemBase
    {
        public LayerMask spawnLayersCheckForOtherPlayers;

        public float distance = 10f;

        public override bool CheckSpawnPosition(Kit_IngameMain main, Transform spawnPoint, Photon.Realtime.Player spawningPlayer)
        {
            Collider[] col = Physics.OverlapSphere(spawnPoint.position, distance, spawnLayersCheckForOtherPlayers.value);

            int team = 0;

            if (spawningPlayer.CustomProperties["team"] != null)
            {
                team = (int)spawningPlayer.CustomProperties["team"];
            }

            List<Collider> enemyColliders = new List<Collider>();

            for (int i = 0; i < col.Length; i++)
            {
                Kit_PlayerBehaviour pb = col[i].GetComponentInParent<Kit_PlayerBehaviour>();
                if (pb)
                {
                    if (!main.currentPvPGameModeBehaviour || !main.currentPvPGameModeBehaviour.isTeamGameMode || pb.myTeam != team)
                    {
                        enemyColliders.Add(col[i]);
                    }
                }
            }

            if (enemyColliders.Count <= 0)
            {
                return true;
            }
            return false;
        }

        public override bool CheckSpawnPosition(Kit_IngameMain main, Transform spawnPoint, Kit_Bot bot)
        {
            Collider[] col = Physics.OverlapSphere(spawnPoint.position, distance, spawnLayersCheckForOtherPlayers.value);

            int team = bot.team;

            List<Collider> enemyColliders = new List<Collider>();

            for (int i = 0; i < col.Length; i++)
            {
                Kit_PlayerBehaviour pb = col[i].GetComponentInParent<Kit_PlayerBehaviour>();
                if (pb)
                {
                    if (!main.currentPvPGameModeBehaviour || !main.currentPvPGameModeBehaviour.isTeamGameMode || (pb.myTeam != team || Vector3.Distance(spawnPoint.position, pb.transform.position) < 1f))
                    {
                        enemyColliders.Add(col[i]);
                    }
                }
            }

            if (enemyColliders.Count <= 0)
            {
                return true;
            }
            return false;
        }
    }
}