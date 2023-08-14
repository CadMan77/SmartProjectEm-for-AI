using Inventor;
//using Microsoft.Win32;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SmartProjectEm
{
    /// <summary>
    /// This is the primary AddIn Server class that implements the ApplicationAddInServer interface
    /// that all Inventor AddIns are required to implement. The communication between Inventor and
    /// the AddIn is via the methods on this interface.
    /// </summary>
    [GuidAttribute("57965d6a-273a-4684-80e2-f46e3229d01f")]
    public class StandardAddInServer : Inventor.ApplicationAddInServer
    {

        // Inventor application object.
        private Inventor.Application m_inventorApplication;

        private Inventor.ApplicationEvents m_appEvents;
        private Inventor.UserInterfaceEvents m_uiEvents;

        public StandardAddInServer()
        {
        }

        #region ApplicationAddInServer Members

        public void Activate(Inventor.ApplicationAddInSite addInSiteObject, bool firstTime)
        {
            // This method is called by Inventor when it loads the addin.
            // The AddInSiteObject provides access to the Inventor Application object.
            // The FirstTime flag indicates if the addin is loaded for the first time.

            // Initialize AddIn members.
            m_inventorApplication = addInSiteObject.Application;

            // TODO: Add ApplicationAddInServer.Activate implementation.
            // e.g. event initialization, command creation etc.

            m_appEvents = m_inventorApplication.ApplicationEvents;
            m_appEvents.OnActivateDocument += new ApplicationEventsSink_OnActivateDocumentEventHandler(ApplicationEvents_OnActivateDocument);

            m_uiEvents = m_inventorApplication.UserInterfaceManager.UserInterfaceEvents;
            m_uiEvents.OnEnvironmentChange += new UserInterfaceEventsSink_OnEnvironmentChangeEventHandler(UserInterfaceEvents_OnEnvironmentChange);
        }


        public void Deactivate()
        {
            // This method is called by Inventor when the AddIn is unloaded.
            // The AddIn will be unloaded either manually by the user or
            // when the Inventor session is terminated

            // TODO: Add ApplicationAddInServer.Deactivate implementation

            // Release objects.
            m_inventorApplication = null;

            m_appEvents.OnActivateDocument -= new ApplicationEventsSink_OnActivateDocumentEventHandler(ApplicationEvents_OnActivateDocument);

            m_appEvents = null;

            m_uiEvents = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }



        private void ApplicationEvents_OnActivateDocument(_Document DocumentObject, EventTimingEnum BeforeOrAfter, NameValueMap Context, out HandlingCodeEnum HandlingCode)
        {

            HandlingCode = Inventor.HandlingCodeEnum.kEventNotHandled;

            if (BeforeOrAfter != Inventor.EventTimingEnum.kAfter)
            {

                if (DocumentObject.DocumentType == DocumentTypeEnum.kPartDocumentObject || DocumentObject.DocumentType == DocumentTypeEnum.kAssemblyDocumentObject || DocumentObject.DocumentType == DocumentTypeEnum.kPresentationDocumentObject)

                {
                    //HandlingCode = Inventor.HandlingCodeEnum.kEventHandled;

                    Camera cam = m_inventorApplication.ActiveView.Camera;

                    if (cam.Perspective)

                    {
                        ControlDefinition ctrlDef = m_inventorApplication.CommandManager.ControlDefinitions["AppViewCubePerspectiveOrthoCmd"];
                        ctrlDef.Execute();
                    }
                }
            }
        }

        private void UserInterfaceEvents_OnEnvironmentChange(Inventor.Environment Environment, EnvironmentStateEnum EnvironmentState, EventTimingEnum BeforeOrAfter, NameValueMap Context, out HandlingCodeEnum HandlingCode)
        {
            if (BeforeOrAfter == Inventor.EventTimingEnum.kAfter && (Environment.InternalName == "PMxPartSketchEnvironment" || Environment.InternalName == "AMxAssemblySketchEnvironment"))
            {
                //MessageBox.Show("!", "Part Sketch Edit");
                ControlDefinition ctrlDef = m_inventorApplication.CommandManager.ControlDefinitions["SketchSliceGraphicsCmd"];
                ctrlDef.Execute();
            }
            //HandlingCode = HandlingCodeEnum.kEventNotHandled;
            HandlingCode = HandlingCodeEnum.kEventHandled;
        }

        public void ExecuteCommand(int commandID)
        {
            // Note:this method is now obsolete, you should use the 
            // ControlDefinition functionality for implementing commands.
        }


        public object Automation
        {
            // This property is provided to allow the AddIn to expose an API 
            // of its own to other programs. Typically, this  would be done by
            // implementing the AddIn's API interface in a class and returning 
            // that class object through this property.

            get
            {
                // TODO: Add ApplicationAddInServer.Automation getter implementation
                return null;
            }
        }

        #endregion

    }
}
