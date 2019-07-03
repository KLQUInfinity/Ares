using System;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.TrainingComponent
{
    class Training1: Training
    {

        protected internal Training1(): base(/*new Training1Logic()*/)
        {
            this.trainingName = TrainingName.FireExtinguishing;
        }

        public TrainingLogicCore TrainingLogicCore
        {
            get => default;
            set
            {
            }
        }

        public override void startTraining()
        {
        }
    }
}
