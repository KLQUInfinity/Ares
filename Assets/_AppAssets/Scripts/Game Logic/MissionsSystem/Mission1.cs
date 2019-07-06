using UnityEngine;
namespace Assets.Scripts.MissionComponent
{
    class Mission1 : Mission
    {

        protected internal Mission1(GameObject Room) : base(Room/*new Training1Logic()*/)
        {
            this.missionName = MissionName.AsteroidAccident;
        }

        public override void startMission()
        {
        }

    }
}
