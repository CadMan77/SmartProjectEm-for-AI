using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using ApplicationTwo.Properties;
using Inventor;

namespace ApplicationTwo
{
    class ApplicationTwoRibbonUI
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


        public ApplicationTwoRibbonUI(Inventor.Application inApp)
        {
            mInventorApp = inApp;
            Initialize();
        }
        private void Initialize()
        {
            CreateApplicationTwoRibbon();
            CreateApplicationTwoButton();
        }

        private void CreateApplicationTwoButton()
        {

            try
            {
                Inventor.UserInterfaceManager userInterFaceMgr = mInventorApp.UserInterfaceManager;
                //handling addin clashing

                Icon AppTwoLarge = Resources._3DPrintTech_Logo_32x321;
                object AppTwolargeIcon = AxHostConverter.ImageToPictureDisp(AppTwoLarge.ToBitmap());
                Icon AppTwoStand = Resources._3DPrinter_16;
                object AppTwoStandIcon = AxHostConverter.ImageToPictureDisp(AppTwoStand.ToBitmap());

                Inventor.Environments envs = userInterFaceMgr.Environments;
                string envInternalName = "ApplicatioTWO";

                mEnviornment = envs.Add(envInternalName, envInternalName, null, AppTwoStandIcon, AppTwolargeIcon);
                mEnviornment.AdditionalVisibleRibbonTabs = new string[] { envInternalName };

                //get the ribbon associated with part document
                Inventor.Ribbons ribbons = userInterFaceMgr.Ribbons;
                Inventor.Ribbon partRibbon = ribbons["Part"];

                //get the tabs associated with part ribbon
                mApplicationTwoTab = partRibbon.RibbonTabs.Add(envInternalName, envInternalName, "02C5BE5C-5AED-4521-B950-F153780D4AEF", "id_TabEnvironments", true, true);

                //Adding Fluid volume Panel.
                RibbonPanel ShapewaysPanel = mApplicationTwoTab.RibbonPanels.Add("ApplicatioTWO", "ApplicatioTWO", "7C6DF6CE-CDE0-4BEA-8A85-692BB370E33A", "", false);
                Inventor.CommandControl loginControl = ShapewaysPanel.CommandControls.AddButton(mApplication1BtnDefn, true, true, "", false);

                mEnviornment.DefaultRibbonTab = envInternalName;

                userInterFaceMgr.ParallelEnvironments.Add(mEnviornment);
                Inventor.ControlDefinition envCtrlDef = mInventorApp.CommandManager.ControlDefinitions[envInternalName];
                envCtrlDef.ProgressiveToolTip.ExpandedDescription = "";
                envCtrlDef.ProgressiveToolTip.Description = "";
                envCtrlDef.ProgressiveToolTip.Title = "Begin ApplicatioTWO";

                ControlDefinition parallelEnvButton = mInventorApp.CommandManager.ControlDefinitions["ApplicatioTWO"];
                mInventorApp.UserInterfaceManager.UserInterfaceEvents.OnEnvironmentChange += new UserInterfaceEventsSink_OnEnvironmentChangeEventHandler(FinishApplicationTWO);

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
        private void CreateApplicationTwoRibbon()
        {
            Icon SplitLarge = Resources.Split_32;
            object SplitLargeIcon = AxHostConverter.ImageToPictureDisp(SplitLarge.ToBitmap());
            Icon SplitStand = Resources.Split_16;
            object SplitStandIcon = AxHostConverter.ImageToPictureDisp(SplitStand.ToBitmap());

            mApplication1BtnDefn = mInventorApp.CommandManager.ControlDefinitions.AddButtonDefinition("ApplicationTWO", "ApplicationTWO", Inventor.CommandTypesEnum.kQueryOnlyCmdType, "B3AA3B84-9A86-42AD-ABA4-883F0FE9AFCD", "application1 btn", "Tooltip ", SplitStandIcon, SplitLargeIcon);
            mApplication1BtnDefn.ProgressiveToolTip.ExpandedDescription = "";
            mApplication1BtnDefn.ProgressiveToolTip.Description = "";
            mApplication1BtnDefn.ProgressiveToolTip.Title = "";
        }

        void FinishApplicationTWO(Inventor.Environment environment, EnvironmentStateEnum EnvironmentState, EventTimingEnum BeforeOrAfter, NameValueMap Context, out HandlingCodeEnum HandlingCode)
        {
            //Finish Button is clicked
            try
            {
                if (BeforeOrAfter == EventTimingEnum.kAfter)
                    if (environment.InternalName == "ApplicatioTWO")
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


        #region data
        Inventor.Application mInventorApp;
        Inventor.ButtonDefinition mApplication1BtnDefn;
        Inventor.RibbonTab mApplicationTwoTab;
        Inventor.Environment mEnviornment;
        #endregion
    }
}
