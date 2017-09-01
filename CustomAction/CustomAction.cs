using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration.Install;
using System.Collections;



namespace CustomAction
{
    [System.ComponentModel.RunInstallerAttribute(true)]
    public class InstallerCustomActions : System.Configuration.Install.Installer
    {
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

            // Save customParameters to a FILE
            
            // Write the string to a file.

            try
            {


                System.IO.StreamWriter file = new System.IO.StreamWriter("c:\\Config.json");

                file.Write("{\"Server\":\"");
                file.Write(customParameters.ServerURL);
                file.Write("\",\"Token\":\"");
                file.WriteLine("\"}");

                file.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            SaveCustomParametersInStateSaverDictionary(
                            stateSaver, customParameters);
            
            //PrintMessage("The application is being installed.", customParameters);

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
            if (stateSaver.Contains(CustomParameters.Keys.ServerURL) == true)
                stateSaver[CustomParameters.Keys.ServerURL] =
                                  customParameters.ServerURL;
            else
                stateSaver.Add(CustomParameters.Keys.ServerURL,
                               customParameters.ServerURL);

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

            //PrintMessage("The application is being uninstalled.", customParameters);

            base.Uninstall(savedState);
        }

        /// <summary>
        /// A helper method that prints out the passed message, and a dumps the custom parameters object to a message box.
        /// </summary>
        /// <param name="message">The message header to place in the message box.</param>
        /// <param name="customParameters">A strong typed object of valid command line parameters.</param>
        private void PrintMessage(string message, CustomParameters customParameters)
        {
            string outputMessage = string.Format("{0}\r\nThe parameters that were recorded during install are:\r\n\r\n\t{1} = {2}\r\n\t{3} = {4}",
                message,
                CustomParameters.Keys.ServerURL, customParameters.ServerURL,
                CustomParameters.Keys.Token, customParameters.Token);

            MessageBox.Show(outputMessage, "Installer Custom Action Fired!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    public class CustomParameters
    {
        /// &lt;summary>
        /// This inner class maintains the key names
        /// for the parameter values that may be passed on the 
        /// command line.
        /// &lt;/summary>
        public class Keys
        {
            public const string ServerURL =
                               "ServerUrl";
            public const string Token =
                               "Token";
        }

        private string _ServerURL = null;
        public string ServerURL
        {
            get { return _ServerURL; }
        }

        private string _Token = null;
        public string Token
        {
            get { return _Token; }
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
            this._ServerURL =
              installContext.Parameters[Keys.ServerURL];
            this._Token =
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
            if (savedState.Contains(Keys.ServerURL) == true)
                this._ServerURL =
                  (string)savedState[Keys.ServerURL];

            if (savedState.Contains(Keys.Token) == true)
                this._Token =
                  (string)savedState[Keys.Token];
        }
    }
}
