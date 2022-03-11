namespace VUPenalty
{
    public abstract class ExperimentState
    {
        protected readonly ExperimentController _context;
        
        protected ExperimentState(ExperimentController controller)
        {
            _context = controller;
        }

        public abstract void Init();

        public abstract void Tick(float deltaTime);

        public abstract void Finish();
        
    }
}