using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.TrainingComponent
{
    class Training2 :Training
    {
        protected internal Training2(): base(/*new Training2Logic()*/)
        {
            this.trainingName = TrainingName.CarryingErgonomics;
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
