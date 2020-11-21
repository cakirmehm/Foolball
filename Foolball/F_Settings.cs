using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Foolball
{
    public partial class F_Settings : Form
    {

        private int totalSpeed = 0, totalPower = 0;
        private int remainingPower = 0;


        CPlayer selectedPlayer = null;

        public CPlayer pLeftGK = new CPlayer();
        public CPlayer pLeftLB = new CPlayer();
        public CPlayer pLeftRB = new CPlayer();
        public CPlayer pLeftDMF = new CPlayer();
        public CPlayer pLeftLF = new CPlayer();
        public CPlayer pLeftRF = new CPlayer();




        public F_Settings()
        {
            InitializeComponent();
            vfnInitializePlayers();

            vfnUpdateRemainingSpeed();
            vfnUpdateRemainingPower();
        }

        /// <summary>
        /// 
        /// </summary>
        public void vfnInitializePlayers()
        {
            // LEFT GK
            pLeftGK.Position = EPosition.GK;
            pLeftGK.Priority = EPriority.Pass;
            pLeftGK.Speed = (ESpeed)tbarSpeedGK.Value;
            pLeftGK.Power = (EPower)tbarPowerGK.Value;

            pLeftGK.Energy = 100;
            pLeftGK.nOfDribble = 0;
            pLeftGK.nOfPass = 0;
            pLeftGK.nOfShoot = 0;
            pLeftGK.blnAlways = chkAlwaysGK.Checked;


            // LEFT LB
            pLeftLB.Position = EPosition.LB;
            pLeftLB.Priority = EPriority.Pass;
            pLeftLB.Speed = (ESpeed)tbarSpeedLB.Value;
            pLeftLB.Power = (EPower)tbarPowerLB.Value;

            pLeftLB.Energy = 100;
            pLeftLB.nOfDribble = 0;
            pLeftLB.nOfPass = 0;
            pLeftLB.nOfShoot = 0;
            pLeftLB.blnAlways = chkAlwaysLB.Checked;


            // LEFT RB
            pLeftRB.Position = EPosition.RB;
            pLeftRB.Priority = EPriority.Pass;
            pLeftRB.Speed = (ESpeed)tbarSpeedRB.Value;
            pLeftRB.Power = (EPower)tbarPowerRB.Value;

            pLeftRB.Energy = 100;
            pLeftRB.nOfDribble = 0;
            pLeftRB.nOfPass = 0;
            pLeftRB.nOfShoot = 0;
            pLeftRB.blnAlways = chkAlwaysRB.Checked;


            // LEFT DMF
            pLeftDMF.Position = EPosition.DMF;
            pLeftDMF.Priority = EPriority.Dribbling;
            pLeftDMF.Speed = (ESpeed)tbarSpeedDMF.Value;
            pLeftDMF.Power = (EPower)tbarPowerDMF.Value;

            pLeftDMF.Energy = 100;
            pLeftDMF.nOfDribble = 0;
            pLeftDMF.nOfPass = 0;
            pLeftDMF.nOfShoot = 0;
            pLeftDMF.blnAlways = chkAlwaysDMF.Checked;


            // LEFT LF
            pLeftLF.Position = EPosition.LF;
            pLeftLF.Priority = EPriority.Shoot;
            pLeftLF.Speed = (ESpeed)tbarSpeedLF.Value;
            pLeftLF.Power = (EPower)tbarPowerLF.Value;

            pLeftLF.Energy = 100;
            pLeftLF.nOfDribble = 0;
            pLeftLF.nOfPass = 0;
            pLeftLF.nOfShoot = 0;
            pLeftLF.blnAlways = chkAlwaysLF.Checked;

            // LEFT RF
            pLeftRF.Position = EPosition.RF;
            pLeftRF.Priority = EPriority.Shoot;
            pLeftRF.Speed = (ESpeed)tbarSpeedRF.Value;
            pLeftRF.Power = (EPower)tbarPowerRF.Value;

            pLeftRF.Energy = 100;
            pLeftRF.nOfDribble = 0;
            pLeftRF.nOfPass = 0;
            pLeftRF.nOfShoot = 0;
            pLeftRF.blnAlways = chkAlwaysRF.Checked;
        }


        private void vfnUpdateRemainingSpeed()
        {
            totalSpeed = (int)pLeftGK.Speed +
                (int)pLeftLB.Speed +
                (int)pLeftRB.Speed +
                (int)pLeftDMF.Speed +
                (int)pLeftLF.Speed +
                (int)pLeftRF.Speed;

            lblRemainsSPEED.Text = "Remaining: " + (12 - totalSpeed);
        }


        private void vfnUpdateRemainingPower()
        {
            totalPower = (int)pLeftGK.Power +
                (int)pLeftLB.Power +
                (int)pLeftRB.Power +
                (int)pLeftDMF.Power +
                (int)pLeftLF.Power +
                (int)pLeftRF.Power;

            lblRemainsPOWER.Text = "Remaining: " + (12 - totalPower);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbarSpeedGK_Scroll(object sender, EventArgs e)
        {
            pLeftGK.Speed = (ESpeed)tbarSpeedGK.Value;
            vfnUpdateRemainingSpeed();
        }

        private void tbarSpeedLB_Scroll(object sender, EventArgs e)
        {
            pLeftLB.Speed = (ESpeed)tbarSpeedLB.Value;
            vfnUpdateRemainingSpeed();
        }

        private void tbarSpeedRB_Scroll(object sender, EventArgs e)
        {
            pLeftRB.Speed = (ESpeed)tbarSpeedRB.Value;
            vfnUpdateRemainingSpeed();
        }

        private void tbarSpeedDMF_Scroll(object sender, EventArgs e)
        {
            pLeftDMF.Speed = (ESpeed)tbarSpeedDMF.Value;
            vfnUpdateRemainingSpeed();
        }

        private void tbarSpeedLF_Scroll(object sender, EventArgs e)
        {
            pLeftLF.Speed = (ESpeed)tbarSpeedLF.Value;
            vfnUpdateRemainingSpeed();
        }

        private void tbarSpeedRF_Scroll(object sender, EventArgs e)
        {
            pLeftRF.Speed = (ESpeed)tbarSpeedRF.Value;
            vfnUpdateRemainingSpeed();
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbarPowerGK_Scroll(object sender, EventArgs e)
        {
            pLeftGK.Power = (EPower)tbarPowerGK.Value;
            vfnUpdateRemainingPower();

        }

        private void tbarPowerLB_Scroll(object sender, EventArgs e)
        {
            pLeftLB.Power = (EPower)tbarPowerLB.Value;
            vfnUpdateRemainingPower();
        }

        private void tbarPowerRB_Scroll(object sender, EventArgs e)
        {
            pLeftRB.Power = (EPower)tbarPowerRB.Value;
            vfnUpdateRemainingPower();
        }

        private void tbarPowerDMF_Scroll(object sender, EventArgs e)
        {
            pLeftDMF.Power = (EPower)tbarPowerDMF.Value;
            vfnUpdateRemainingPower();
        }

        private void tbarPowerLF_Scroll(object sender, EventArgs e)
        {
            pLeftLF.Power = (EPower)tbarPowerLF.Value;
            vfnUpdateRemainingPower();
        }

        private void tbarPowerRF_Scroll(object sender, EventArgs e)
        {
            pLeftRF.Power = (EPower)tbarPowerRF.Value;
            vfnUpdateRemainingPower();
        }





      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbarPriorityGK_Scroll(object sender, EventArgs e)
        {
            pLeftGK.Priority = (EPriority)tbarPriorityGK.Value;
            
        }

        private void tbarPriorityLB_Scroll(object sender, EventArgs e)
        {
            pLeftLB.Priority = (EPriority)tbarPriorityLB.Value;
            pLeftLB.blnAlways = chkAlwaysLB.Checked;
        }

        private void tbarPriorityRB_Scroll(object sender, EventArgs e)
        {
            pLeftRB.Priority = (EPriority)tbarPriorityRB.Value;
            pLeftRB.blnAlways = chkAlwaysRB.Checked;
        }

        private void tbarPriorityDMF_Scroll(object sender, EventArgs e)
        {
            pLeftDMF.Priority = (EPriority)tbarPriorityDMF.Value;
            pLeftDMF.blnAlways = chkAlwaysDMF.Checked;
        }

        private void tbarPriorityLF_Scroll(object sender, EventArgs e)
        {
            pLeftLF.Priority = (EPriority)tbarPriorityLF.Value;
            pLeftLF.blnAlways = chkAlwaysLF.Checked;
        }

        private void tbarPriorityRF_Scroll(object sender, EventArgs e)
        {
            pLeftRF.Priority = (EPriority)tbarPriorityRF.Value;
            pLeftRF.blnAlways = chkAlwaysRF.Checked;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveAndClose_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void chkAlwaysGK_CheckedChanged(object sender, EventArgs e)
        {
            pLeftGK.blnAlways = chkAlwaysGK.Checked;
        }

        private void chkAlwaysLB_CheckedChanged(object sender, EventArgs e)
        {
            pLeftLB.blnAlways = chkAlwaysLB.Checked;
        }

        private void chkAlwaysRB_CheckedChanged(object sender, EventArgs e)
        {
            pLeftRB.blnAlways = chkAlwaysRB.Checked;
        }

        private void chkAlwaysDMF_CheckedChanged(object sender, EventArgs e)
        {
            pLeftDMF.blnAlways = chkAlwaysDMF.Checked;
        }

        private void chkAlwaysLF_CheckedChanged(object sender, EventArgs e)
        {
            pLeftLF.blnAlways = chkAlwaysLF.Checked;
        }

        private void chkAlwaysRF_CheckedChanged(object sender, EventArgs e)
        {
            pLeftRF.blnAlways = chkAlwaysRF.Checked;
        }



      

        


    }
}