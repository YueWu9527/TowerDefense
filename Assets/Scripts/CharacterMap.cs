using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace A2
{
    public class Team
    {
        public List<TeamPlayer> players = new List<TeamPlayer>();
    }
    public class CharacterMap : Singleton<CharacterMap>
    {
        public List<Team> teams = new List<Team>(8);
        override protected void Init()
        {
            teams.Add(new Team());
            teams.Add(new Team());
            teams.Add(new Team());
        }

        public TeamPlayer GetTarget(TeamPlayer teamPlayer, float searchRange)
        {
            int teamIndex = teamPlayer.teamIndex;
            int targetTeamIndex = Random.Range(0,teams.Count);
            while(targetTeamIndex == teamIndex)
            {
                targetTeamIndex = Random.Range(0,teams.Count);
            }

            return GetTargetByTeamIndex(teamPlayer, searchRange, targetTeamIndex);
        }

        public TeamPlayer GetTargetByTeamIndex(TeamPlayer teamPlayer, float searchRange, int targetTeamIndex)
        {
            TeamPlayer _target = null;
            float distance = searchRange;
            foreach (var target in teams[targetTeamIndex].players)
            {
                float _distance = (target.transform.position-teamPlayer.transform.position).magnitude;
                if(_distance<distance)
                {
                    _target = target;
                    distance = _distance;
                }
            }
            return _target;
        }
    }
}
