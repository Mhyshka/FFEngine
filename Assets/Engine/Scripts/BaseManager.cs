namespace FF
{
    /// <summary>
    /// The base class from which all manager should inherit.
    /// </summary>
    internal abstract class BaseManager
    {
        /// <summary>
        /// Called later when Constructor setup of all the manager has been called.
        /// </summary>
        internal virtual void DoStart()
        {
        }

        /// <summary>
        /// Called on each frame
        /// </summary>
        internal virtual void DoUpdate()
        {
        }

        /// <summary>
        /// Called on each physX update
        /// </summary>
        internal virtual void DoFixedUpdate()
        {
        }

        /// <summary>
        /// Called when the engine is getting destroyed ( Game is quitting )
        /// </summary>
        internal virtual void TearDown()
        {
        }
    }
}
