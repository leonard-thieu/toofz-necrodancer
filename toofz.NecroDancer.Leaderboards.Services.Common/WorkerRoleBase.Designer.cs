using System.ComponentModel;

namespace toofz.NecroDancer.Leaderboards.Services.Common
{
    partial class WorkerRoleBase<TSettings>
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
#pragma warning disable 0649
        IContainer components;
#pragma warning restore 0649

        #region IDisposable Members

        bool disposed;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                components?.Dispose();
            }

            disposed = true;
            base.Dispose(disposing);
        }

        #endregion

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        void InitializeComponent()
        {

        }

        #endregion
    }
}
