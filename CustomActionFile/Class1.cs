using System;
using System.Collections;
using System.Configuration.Install

namespace CustomActionFile
{

    public class CustomParameters
    {
        /// &lt;summary>
        /// This inner class maintains the key names
        /// for the parameter values that may be passed on the 
        /// command line.
        /// &lt;/summary>
        public class Keys
        {
            public const string ServerUrl = "ServerUrl";
            public const string Token = "Token";
        }

        private string _serverUrl = null;
        public string ServerUrl
        {
            get { return _serverUrl; }
        }

        private string _token = null;
        public string Token
        {
            get { return _token; }
        }

        /// &lt;summary>
        /// This constructor is invoked by Install class
        /// methods that have an Install Context built from 
        /// parameters specified in the command line.
        /// Rollback, Install, Commit, and intermediate methods like
        /// OnAfterInstall will all be able to use this constructor.
        /// &lt;/summary>
        /// &lt;param name="installContext">The install context
        /// containing the command line parameters to set
        /// the strong types variables to.&lt;/param>
        public CustomParameters(InstallContext installContext)
        {
            this._serverUrl =
              installContext.Parameters[Keys.ServerUrl];
            this._token =
                  installContext.Parameters[Keys.Token];
        }

        /// &lt;summary>
        /// This constructor is used by the Install class
        /// methods that don't have an Install Context built
        /// from the command line. This method is primarily
        /// used by the Uninstall method.
        /// &lt;/summary>
        /// &lt;param name="savedState">An IDictionary object
        /// that contains the parameters that were
        /// saved from a prior installation.&lt;/param>
        public CustomParameters(IDictionary savedState)
        {
            if (savedState.Contains(Keys.ServerUrl) == true)
                this._serverUrl =
                  (string)savedState[Keys.ServerUrl];

            if (savedState.Contains(Keys.Token) == true)
                this._token =
                  (string)savedState[Keys.Token];
        }
    }
    /// &lt;summary>
    /// To cause this method to be invoked, I added the primary project output to the 
    /// setup project's custom actions, under the "Install" folder.
    /// &lt;/summary>
    /// &lt;param name="stateSaver">A dictionary object
    /// that will be retrievable during the uninstall process.&lt;/param>
    public override void Install(System.Collections.IDictionary stateSaver)
    {
        // Get the custom parameters from the install context.
        CustomParameters customParameters = new CustomParameters(this.Context);

        SaveCustomParametersInStateSaverDictionary(
                        stateSaver, customParameters);

        PrintMessage("The application is being installed.",
                     customParameters);

        base.Install(stateSaver);
    }

    /// &lt;summary>
    /// Adds or updates the state dictionary so that custom
    /// parameter values can be retrieved when 
    /// the application is uninstalled.
    /// &lt;/summary>
    /// &lt;param name="stateSaver">An IDictionary object
    /// that will contain all the objects who's state
    /// is to be persisted across installations.&lt;/param>
    /// &lt;param name="customParameters">A strong typed
    /// object of custom parameters that will be saved.&lt;/param>
    private void SaveCustomParametersInStateSaverDictionary(
            System.Collections.IDictionary stateSaver,
            CustomParameters customParameters)
    {
        // Add/update the "MyCustomParameter" entry in the
        // state saver so that it may be accessed on uninstall.
        if (stateSaver.Contains(CustomParameters.Keys.ServerUrl) == true)
            stateSaver[CustomParameters.Keys.ServerUrl] =
                              customParameters.ServerUrl;
        else
            stateSaver.Add(CustomParameters.Keys.ServerUrl,
                           customParameters.ServerUrl);

        // Add/update the "MyOtherCustomParameter" entry in the
        // state saver so that it may be accessed on uninstall.
        if (stateSaver.Contains(
                 CustomParameters.Keys.Token) == true)
            stateSaver[CustomParameters.Keys.Token] =
                       customParameters.Token;
        else
            stateSaver.Add(CustomParameters.Keys.Token,
                           customParameters.Token);
    }

    /// &lt;summary>
    /// To cause this method to be invoked,
    /// I added the primary project output to the 
    /// setup project's custom actions, under the "Uninstall" folder.
    /// &lt;/summary>
    /// &lt;param name="savedState">An IDictionary
    /// object that will contain objects that were set as 
    /// part of the installation process.&lt;/param>
    public override void Uninstall(
           System.Collections.IDictionary savedState)
    {
        // Get the custom parameters from the saved state.
        CustomParameters customParameters =
                new CustomParameters(savedState);

        PrintMessage("The application is being uninstalled.",
                     customParameters);

        base.Uninstall(savedState);
    }
}
