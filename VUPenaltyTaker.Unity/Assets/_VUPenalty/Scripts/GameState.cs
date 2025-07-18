﻿namespace VUPenalty
{
    public abstract class GameState
    {
        protected readonly Game _context;

        protected GameState(Game game)
        {
            _context = game;
        }

        public abstract void Init();

        public abstract void Tick(float deltaTime);

        public abstract void Finish();
    }
}