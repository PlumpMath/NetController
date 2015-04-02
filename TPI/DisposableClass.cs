using System;
using System.Diagnostics;

namespace TPI
{
    public class DisposableClass : IDisposable
    {
        // Keep track if whether resources are already freed.
        public bool ResourcesAreFreed { get; private set; }

        // Free managed and unmanaged resources.
        public void Dispose()
        {
            FreeResources(true);
        }

        // Destructor to clean up unmanaged resources
        // but not managed resources.
        ~DisposableClass()
        {
            FreeResources(false);
        }

        protected virtual void FreeManagedResources()
        {

        }

        protected virtual void FreeUnManagedResources()
        {

        }

        // Free resources.
        private void FreeResources(bool freeManagedResources)
        {
            Debug.WriteLine(this.GetType().Name + ": FreeResources");
            if (!ResourcesAreFreed)
            {
                // Dispose of managed resources if appropriate.
                if (freeManagedResources)
                {
                    FreeManagedResources();
                    // Dispose of managed resources here.
                    Debug.WriteLine(this.GetType().Name + ": Dispose of managed resources");
                }

                FreeUnManagedResources();
                // Dispose of unmanaged resources here.
                Debug.WriteLine(this.GetType().Name + ": Dispose of unmanaged resources");

                // Remember that we have disposed of resources.
                ResourcesAreFreed = true;

                // We don't need the destructor because
                // our resources are already freed.
                GC.SuppressFinalize(this);
            }
        }
    }
}
