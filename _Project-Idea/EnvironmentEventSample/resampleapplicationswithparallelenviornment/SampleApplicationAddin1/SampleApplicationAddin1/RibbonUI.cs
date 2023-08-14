using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Inventor;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using ApplicationOne.Properties;

namespace SampleApplicationAddin1
{
    public class ApplicationOneRibbonUI
    {
        internal class AxHostConverter : AxHost
        {
            private AxHostConverter()
                : base("")
            {
            }

            public static stdole.IPictureDisp ImageToPictureDisp(Image image)
            {
                return (stdole.IPictureDisp)GetIPictureDispFromPicture(image);
            }

            public static Image PictureDispToImage(stdole.IPictureDisp pictureDisp)
            {
                return GetPictureFromIPicture(pictureDisp);
            }
        }

        public ApplicationOneRibbonUI(Inventor.Application inApplication)
        {
            mInventorApp = inApplication;
            CreateApplicationBtn();
            CreateRibbon();
        }

        public void CreateRibbon()
        {
            try
            {
                Inventor.UserInterfaceManager userInterFaceMgr = mInventorApp.UserInterfaceManager;
                //handling addin clashing

                Icon ShapewaysLarge = Resources.ShapeInventTech32x32;
                object ShapewayslargeIcon = AxHostConverter.ImageToPictureDisp(ShapewaysLarge.ToBitmap());
                Icon ShapewaysStand = Resources.ShapeInventTech16x16;
                object ShapewaysStandIcon = AxHostConverter.ImageToPictureDisp(ShapewaysStand.ToBitmap());

                Inventor.Environments envs = userInterFaceMgr.Environments;
                string envInternalName = "ApplicatioOne";

                mEnviornment = envs.Add(envInternalName, envInternalName, null, ShapewaysStandIcon, ShapewayslargeIcon);
                mEnviornment.AdditionalVisibleRibbonTabs = new string[] { envInternalName };

                //get the ribbon associated with part document
                Inventor.Ribbons ribbons = userInterFaceMgr.Ribbons;
                Inventor.Ribbon partRibbon = ribbons["Part"];

                //get the tabs associated with part ribbon
                mShapeInventechPartTab = partRibbon.RibbonTabs.Add(envInternalName, envInternalName, "E8AC966E-B2C7-4B18-8CC5-6E3620BAED59", "id_TabEnvironments", true, true);

                //Adding Fluid volume Panel.
                RibbonPanel ShapewaysPanel = mShapeInventechPartTab.RibbonPanels.Add("Application1", "Application1", "4B2BAC4E-1058-45E6-8328-595774A50AD3", "", false);
                Inventor.CommandControl loginControl = ShapewaysPanel.CommandControls.AddButton(mApplication1BtnDefn, true, true, "", false);

                mEnviornment.DefaultRibbonTab = envInternalName;

                userInterFaceMgr.ParallelEnvironments.Add(mEnviornment);
                Inventor.ControlDefinition envCtrlDef = mInventorApp.CommandManager.ControlDefinitions[envInternalName];
                envCtrlDef.ProgressiveToolTip.ExpandedDescription = "";
                envCtrlDef.ProgressiveToolTip.Description = "";
                envCtrlDef.ProgressiveToolTip.Title = "Begin ApplicatioOne";

                ControlDefinition parallelEnvButton = mInventorApp.CommandManager.ControlDefinitions["ApplicatioOne"];
                mInventorApp.UserInterfaceManager.UserInterfaceEvents.OnEnvironmentChange += new UserInterfaceEventsSink_OnEnvironmentChangeEventHandler(FinishApplicationOne);
                
                for (int i = 1; i <= envs.Count; i++)
                    {
                        Inventor.Environment en = envs[i];
                        if (en.InternalName != "PMxPartEnvironment")
                        {
                            en.DisabledCommandList.Add(parallelEnvButton);
                        }
                    }
            }
            catch (Exception ex)
            { 
                MessageBox.Show(ex.InnerException.StackTrace);
            }
        }

        private void CreateApplicationBtn()
        {
            Icon UploadLarge = Resources.Upload32x32;
            object UploadLargeIcon = AxHostConverter.ImageToPictureDisp(UploadLarge.ToBitmap());
            Icon UploadStand = Resources.Upload6x16;
            object UploadStandIcon = AxHostConverter.ImageToPictureDisp(UploadStand.ToBitmap());

            mApplication1BtnDefn = mInventorApp.CommandManager.ControlDefinitions.AddButtonDefinition("Application1", "Application1", Inventor.CommandTypesEnum.kQueryOnlyCmdType, "9B8249C2-26AA-42FD-8F32-443BF0F71671","application1 btn","Tooltip ", UploadStandIcon, UploadLargeIcon);
            mApplication1BtnDefn.OnExecute += mApplicationBtn_OnExecute;
            mApplication1BtnDefn.ProgressiveToolTip.ExpandedDescription = "";
            mApplication1BtnDefn.ProgressiveToolTip.Description = "";
            mApplication1BtnDefn.ProgressiveToolTip.Title = "";
        }

        void mApplicationBtn_OnExecute(NameValueMap Context)
        {
            MessageBox.Show("Button Click");
        }
        
        void FinishApplicationOne(Inventor.Environment environment, EnvironmentStateEnum EnvironmentState, EventTimingEnum BeforeOrAfter, NameValueMap Context, out HandlingCodeEnum HandlingCode)
        {
            //Finish Button is clicked
            try
            {
                if (BeforeOrAfter == EventTimingEnum.kAfter)
                    if (environment.InternalName == "ApplicatioOne")
                    {
                        if (EnvironmentState == EnvironmentStateEnum.kTerminateEnvironmentState)
                        {
                           //Adding my ribbon enable/disable code
                        }
                    }
                HandlingCode = HandlingCodeEnum.kEventHandled;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                HandlingCode = HandlingCodeEnum.kEventCanceled;
            }
        }


        # region data
        private Inventor.Application mInventorApp;
        internal Inventor.Environment mEnviornment;
        internal RibbonTab mShapeInventechPartTab;
        private ButtonDefinition mApplication1BtnDefn;
        #endregion
    }
}
