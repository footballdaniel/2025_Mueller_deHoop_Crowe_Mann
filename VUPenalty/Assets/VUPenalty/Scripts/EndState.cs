using UnityEngine;

namespace VUPenalty
{
    internal class EndState : GameState
    {
        public EndState(Game game) : base(game)
        {
        }

        public override void Init()
        {
            Debug.Log("Experiment has finished");
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Finish()
        {
        }
    }
}