namespace System
{
    internal class KeyEventHandler
    {
        private Action<object, EventArgs> compiler_KeyDown;

        public KeyEventHandler(Action<object, EventArgs> compiler_KeyDown)
        {
            this.compiler_KeyDown = compiler_KeyDown;
        }
    }
}